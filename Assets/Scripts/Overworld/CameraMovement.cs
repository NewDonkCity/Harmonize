using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    //list containing all objects to focus on
    private List<GameObject> focus = new List<GameObject>();

    //TODO: delete these once there's a way to add things to focus
    [SerializeField] private GameObject focus1;
    [SerializeField] private GameObject focus2;

    //speed, default distance, and zoom out rate for camera
    [SerializeField] private float cameraSpeed = 5.0f;
    [SerializeField] private float cameraDistance = 3.0f;
    [SerializeField] private float cameraZoomRate = 1.1f;

    //================================================================================
    // Start
    //================================================================================
    void Start()
    {
        //TODO: delete these once there's a way to add things to focus
        addFocus(focus1);
        addFocus(focus2);
    }

    //================================================================================
    // Update
    //================================================================================
    void Update()
    {
        lerpCameraPosition(findCameraPosition());
    }

    /// <summary>
    /// Finds where the new camera position should be
    /// </summary>
    /// <returns>
    /// The new camera position
    /// </returns>
    Vector3 findCameraPosition()
    {
        float distance = Mathf.Max(cameraDistance, largestFocusGap() * cameraZoomRate);
        return focusPoint() - (distance * transform.forward);
    }

    /// <summary>
    /// finds the largest gap between all objects that the camera needs to focus on
    /// </summary>
    /// <returns>
    /// The largest distance between all focus objects
    /// </returns>
    float largestFocusGap()
    {
        float largestDistance = 0.0f;
        for (int i = 0; i < focus.Count - 1; i++)
        {
            for (int j = i + 1; j < focus.Count; j++)
            {
                float dis = Vector3.Distance(focus[i].transform.position, focus[j].transform.position);
                if (largestDistance < dis)
                {
                    largestDistance = dis;
                }
            }
        }
        return largestDistance;
    }

    /// <summary>
    /// finds the centerpoint of all the focus objects
    /// </summary>
    /// <returns>
    /// Returns the midpoint of all the focus objects
    /// </returns>
    Vector3 focusPoint()
    {
        Vector3 lookAt = new Vector3(0, 0, 0);
        foreach (GameObject g in focus)
        {
            lookAt = lookAt + g.transform.position;
        }
        lookAt *= 1.0f / (float)focus.Count;
        return lookAt;
    }

    /// <summary>
    /// uses linear interpolation to move camera smoothly
    /// </summary>
    /// <param name="position">
    /// The position of where the camera is moving to
    /// </param>
    void lerpCameraPosition(Vector3 position)
    {
        float interpolation = cameraSpeed * Time.deltaTime;
        Vector3 cameraPosition = this.transform.position;
        cameraPosition = Vector3.Lerp(this.transform.position, position, interpolation);
        this.transform.position = cameraPosition;
    }

    /// <summary>
    /// Add an object for the camera to focus on
    /// Remember to remove it afterwards
    /// </summary>
    /// <param name="obj">
    /// The object to focus
    /// </param>
    public void addFocus(GameObject obj)
    {
        focus.Add(obj);
    }

    /// <summary>
    /// Remove a focus object
    /// </summary>
    /// <param name="obj">
    /// The object to remove
    /// </param>
    public void removeFocus(GameObject obj)
    {
        focus.Remove(obj);
    }
}
