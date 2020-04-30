using System.Collections.Generic;
using System.Linq;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.U2D.Common;
using UnityEngine.Experimental.U2D.IK;
using UnityEngine.Profiling;

namespace UnityEditor.Experimental.U2D.IK
{
    internal class IKEditorManager : ScriptableSingleton<IKEditorManager>
    {
        private readonly HashSet<IKManager2D> m_DirtyManagers = new HashSet<IKManager2D>();
        private readonly HashSet<Solver2D> m_IKSolvers = new HashSet<Solver2D>();
        private readonly List<IKManager2D> m_IKManagers = new List<IKManager2D>();
        private readonly Dictionary<IKChain2D, Vector3> m_ChainPositionOverrides = new Dictionary<IKChain2D, Vector3>();
        private readonly List<Vector3> m_TargetPositions = new List<Vector3>();

        private GameObject m_Helper;
        private GameObject[] m_SelectedGameobjects;
        private bool m_IgnorePostProcessModifications = false;
        private HashSet<Transform> m_IgnoreTransformsOnUndo = new HashSet<Transform>();
        internal bool isDraggingATool { get; private set; }
        internal bool isDragging { get { return IKGizmos.instance.isDragging || isDraggingATool; } }


        [InitializeOnLoadMethod]
        private static void Setup()
        {
            instance.Create();
        }

        private void Create() {}

        private void OnEnable()
        {
            SetupLateUpdateHelper();
            RegisterCallbacks();
            Initialize();
        }

        private void OnDisable()
        {
            UnregisterCallbacks();
            DestroyLateUpdateHelper();
        }

        private void RegisterCallbacks()
        {
            EditorApplication.hierarchyChanged += Initialize;
            Undo.postprocessModifications += OnPostProcessModifications;
#if UNITY_2019_1_OR_NEWER
            SceneView.duringSceneGui += OnSceneGUI;
#else
            SceneView.onSceneGUIDelegate += OnSceneGUI;
#endif
            Selection.selectionChanged += OnSelectionChanged;
        }

        private void UnregisterCallbacks()
        {
            EditorApplication.hierarchyChanged -= Initialize;
            Undo.postprocessModifications -= OnPostProcessModifications;
#if UNITY_2019_1_OR_NEWER
            SceneView.duringSceneGui -= OnSceneGUI;
#else
            SceneView.onSceneGUIDelegate -= OnSceneGUI;
#endif
            Selection.selectionChanged -= OnSelectionChanged;
        }

        private void OnSelectionChanged()
        {
            m_SelectedGameobjects = null;
        }

        private void SetupLateUpdateHelper()
        {
            if (m_Helper != null)
                return;

            m_Helper = new GameObject("IKEditorManagerHelper");
            m_Helper.hideFlags = HideFlags.HideAndDontSave;
            var helper = m_Helper.AddComponent<IKEditorManagerHelper>();
            helper.onLateUpdate.AddListener(OnLateUpdate);
        }

        private void DestroyLateUpdateHelper()
        {
            if (m_Helper != null)
                GameObject.DestroyImmediate(m_Helper);
        }

        public void Initialize()
        {
            m_IKManagers.Clear();
            m_IKSolvers.Clear();
            m_DirtyManagers.Clear();
            m_ChainPositionOverrides.Clear();

            var currentStage = StageUtility.GetCurrentStageHandle();
            var managers =  currentStage.FindComponentsOfType<IKManager2D>().Where(x => x.gameObject.scene.isLoaded).ToArray();
            m_IKManagers.AddRange(managers);

            foreach (IKManager2D manager in m_IKManagers)
            {
                foreach (Solver2D solver in manager.solvers)
                {
                    if (solver)
                        m_IKSolvers.Add(solver);
                }
            }
        }

        public IKManager2D FindManager(Solver2D solver)
        {
            foreach (IKManager2D manager in m_IKManagers)
            {
                if (manager == null)
                    continue;

                foreach (Solver2D s in manager.solvers)
                {
                    if (s == null)
                        continue;

                    if (s == solver)
                        return manager;
                }
            }

            return null;
        }

        public void Record(Solver2D solver, string undoName)
        {
            var manager = FindManager(solver);

            DoUndo(manager, undoName, true);
        }

        public void RegisterUndo(Solver2D solver, string undoName)
        {
            var manager = FindManager(solver);

            DoUndo(manager, undoName, false);
        }

        public void Record(IKManager2D manager, string undoName)
        {
            DoUndo(manager, undoName, true);
        }

        public void RegisterUndo(IKManager2D manager, string undoName)
        {
            DoUndo(manager, undoName, false);
        }

        private void DoUndo(IKManager2D manager, string undoName, bool record)
        {
            foreach (var solver in manager.solvers)
            {
                if (solver == null || !solver.isActiveAndEnabled)
                    continue;

                if (!solver.isValid)
                    solver.Initialize();

                if (!solver.isValid)
                    continue;

                for (int i = 0; i < solver.chainCount; ++i)
                {
                    var chain = solver.GetChain(i);

                    if (record)
                    {
                        foreach(var t in chain.transforms)
                        {
                            if(m_IgnoreTransformsOnUndo.Contains(t))
                                continue;

                            Undo.RecordObject(t, undoName);
                        }
                        

                        if(chain.target && !m_IgnoreTransformsOnUndo.Contains(chain.target))
                            Undo.RecordObject(chain.target, undoName);
                    }
                    else
                    {
                        foreach(var t in chain.transforms)
                        {
                            if(m_IgnoreTransformsOnUndo.Contains(t))
                                continue;

                            Undo.RegisterCompleteObjectUndo(t, undoName);
                        }

                        if(chain.target && !m_IgnoreTransformsOnUndo.Contains(chain.target))
                            Undo.RegisterCompleteObjectUndo(chain.target, undoName);
                    }
                }

                m_IgnorePostProcessModifications = true;
            }
        }

        public void UpdateManagerImmediate(IKManager2D manager, bool recordRootLoops)
        {
            SetManagerDirty(manager);
            UpdateDirtyManagers(recordRootLoops);
        }

        public void UpdateSolverImmediate(Solver2D solver, bool recordRootLoops)
        {
            SetSolverDirty(solver);
            UpdateDirtyManagers(recordRootLoops);
        }

        public void UpdateHierarchyImmediate(Transform hierarchyRoot, bool recordRootLoops)
        {
            SetDirtyUnderHierarchy(hierarchyRoot);
            UpdateDirtyManagers(recordRootLoops);
        }

        public void SetChainPositionOverride(IKChain2D chain, Vector3 position)
        {
            m_ChainPositionOverrides[chain] = position;
        }

        private bool IsViewToolActive()
        {
            int button = Event.current.button;
            return Tools.current == Tool.View || Event.current.alt || (button == 1) || (button == 2);
        }

        private bool IsDraggingATool()
        {
            //If a tool has used EventType.MouseDrag, we won't be able to detect it. Instead we check for delta magnitude
            return GUIUtility.hotControl != 0 && Event.current.button == 0 && Event.current.delta.sqrMagnitude > 0f && !IsViewToolActive();
        }

        private void OnSceneGUI(SceneView sceneView)
        {
            if (m_SelectedGameobjects == null)
                m_SelectedGameobjects = Selection.gameObjects;

            foreach (Solver2D solver in m_IKSolvers)
                IKGizmos.instance.DoSolverGUI(solver);

            if (!IKGizmos.instance.isDragging && IsDraggingATool())
            {
                //We expect the object to be selected while dragged
                foreach (var gameObject in m_SelectedGameobjects)
                {
                    if (gameObject != null && gameObject.transform != null)
                        SetDirtySolversAffectedByTransform(gameObject.transform);
                }

                if(m_DirtyManagers.Count > 0 && !isDraggingATool)
                {
                    isDraggingATool = true;
                    Undo.SetCurrentGroupName("IK Update");

                    RegisterUndoForDirtyManagers();
                }
            }

            if(GUIUtility.hotControl == 0)
                isDraggingATool = false;
        }

        internal void OnLateUpdate()
        {
            if (Application.isPlaying)
                return;

            Profiler.BeginSample("IKEditorManager.OnLateUpdate");

            SetAllManagersDirty();
            UpdateDirtyManagers(false);

            Profiler.EndSample();
        }

        private bool ProcessTransformPropertyModification(UndoPropertyModification modification, out Transform transform)
        {
            transform = null;
            var targetType = modification.currentValue.target.GetType();
            if ((targetType == typeof(Transform) || targetType.IsSubclassOf(typeof(Transform))))
            {
                transform = (Transform)modification.currentValue.target;
                return true;
            }

            return false;
        }

        private UndoPropertyModification[] OnPostProcessModifications(UndoPropertyModification[] modifications)
        {
            if(!m_IgnorePostProcessModifications && !isDragging)
            {
                //Prepare transforms that already have an undo modification
                foreach (var modification in modifications)
                {
                    Transform transform;
                    if (ProcessTransformPropertyModification(modification, out transform))
                        m_IgnoreTransformsOnUndo.Add(transform);
                }

                var processedObjectList = new HashSet<Object>();

                foreach (var modification in modifications)
                {
                    var target = modification.currentValue.target;

                    if(processedObjectList.Contains(target))
                        continue;

                    processedObjectList.Add(target);

                    var targetType = target.GetType();
                    Transform transform;
                    if (ProcessTransformPropertyModification(modification, out transform))
                    {
                        SetDirtySolversAffectedByTransform(transform);
                        RegisterUndoForDirtyManagers();
                    }
                    if (targetType == typeof(Solver2D) || targetType.IsSubclassOf(typeof(Solver2D)))
                    {
                        var solver = (Solver2D)modification.currentValue.target;
                        SetSolverDirty(solver);
                        RegisterUndoForDirtyManagers();
                    }
                    if (targetType == typeof(IKManager2D))
                    {
                        var dirtyManager = (IKManager2D)modification.currentValue.target;
                        SetManagerDirty(dirtyManager);
                        RegisterUndoForDirtyManagers();
                    }
                }

                m_IgnoreTransformsOnUndo.Clear();
            }

            m_IgnorePostProcessModifications = false;

            return modifications;
        }

        private void SetSolverDirty(Solver2D solver)
        {
            if (solver && solver.isValid && solver.isActiveAndEnabled)
                SetManagerDirty(FindManager(solver));
        }

        private void SetManagerDirty(IKManager2D manager)
        {
            if (manager && manager.isActiveAndEnabled)
                m_DirtyManagers.Add(manager);
        }

        private void SetAllManagersDirty()
        {
            m_DirtyManagers.Clear();

            foreach (IKManager2D manager in m_IKManagers)
                SetManagerDirty(manager);
        }

        private void SetDirtyUnderHierarchy(Transform hierarchyRoot)
        {
            if (hierarchyRoot == null)
                return;

            foreach (Solver2D solver in m_IKSolvers)
            {
                if (solver.isValid)
                {
                    for (int i = 0; i < solver.chainCount; ++i)
                    {
                        var chain = solver.GetChain(i);

                        if(chain.target == null)
                            continue;

                        if (hierarchyRoot == chain.target ||
                            IKUtility.IsDescendentOf(chain.target, hierarchyRoot) ||
                            IKUtility.IsDescendentOf(chain.effector, hierarchyRoot))
                        {
                            SetSolverDirty(solver);
                            break;
                        }
                    }
                }
            }
        }

        private void SetDirtySolversAffectedByTransform(Transform transform)
        {
            foreach (Solver2D solver in m_IKSolvers)
            {
                if (solver.isValid)
                {
                    for (int i = 0; i < solver.chainCount; ++i)
                    {
                        var chain = solver.GetChain(i);

                        if(chain.target == null)
                            continue;

                        if (!(IKUtility.IsDescendentOf(chain.target, transform) && IKUtility.IsDescendentOf(chain.rootTransform, transform)) &&
                            (chain.target == transform || IKUtility.IsDescendentOf(chain.target, transform) || IKUtility.IsDescendentOf(chain.effector, transform)))
                        {
                            SetSolverDirty(solver);
                            break;
                        }
                    }
                }
            }
        }

        private void RegisterUndoForDirtyManagers()
        {
            foreach (var manager in m_DirtyManagers)
                RegisterUndo(manager, Undo.GetCurrentGroupName());
        }

        private void UpdateDirtyManagers(bool recordRootLoops)
        {
            foreach (var manager in m_DirtyManagers)
            {
                if (manager == null || !manager.isActiveAndEnabled)
                    continue;

                foreach (var solver in manager.solvers)
                {
                    if (solver == null || !solver.isActiveAndEnabled)
                        continue;

                    if (!solver.isValid)
                        solver.Initialize();

                    if (!solver.isValid)
                        continue;

                    if(solver.allChainsHaveTargets)
                        solver.UpdateIK(manager.weight);
                    else if(PrepareTargetOverrides(solver))
                        solver.UpdateIK(m_TargetPositions, manager.weight);

                    for (int i = 0; i < solver.chainCount; ++i)
                    {
                        var chain = solver.GetChain(i);

                        if (recordRootLoops)
                            InternalEngineBridge.SetLocalEulerHint(chain.rootTransform);

                        if(solver.constrainRotation && chain.target != null)
                            InternalEngineBridge.SetLocalEulerHint(chain.effector);
                    }
                }
            }

            m_DirtyManagers.Clear();
            m_ChainPositionOverrides.Clear();
        }

        private bool PrepareTargetOverrides(Solver2D solver)
        {
            m_TargetPositions.Clear();

            for (int i = 0; i < solver.chainCount; ++i)
            {
                var chain = solver.GetChain(i);

                Vector3 positionOverride;
                if (!m_ChainPositionOverrides.TryGetValue(chain, out positionOverride))
                {
                    m_TargetPositions.Clear();
                    return false;
                }

                m_TargetPositions.Add(positionOverride);
            }

            return true;
        }
    }
}
