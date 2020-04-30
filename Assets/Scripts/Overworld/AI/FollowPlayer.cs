using UnityEngine;
using UnityEngine.AI;

/// <summary>
///   Simple script which allows for a NavMeshAgent to follow the nearest player.
/// </summary>
public class FollowPlayer : MonoBehaviour
{
    //============================================================================
    // Dependencies
    //============================================================================
    public PlayerPositionSystem playerPositionSystem;
    public NavMeshAgent navMeshAgent;

    //============================================================================
    // Unity Methods
    //============================================================================
    void Update()
    {
        if (GetComponent<Liftable>().get_isLifted())
        {
            // Disable the NavMeshAgent so that the enemy does not immediately
            // snap onto the NavMesh surface after being thrown. Re-enabling is
            // done by the OnCollisionEnter() code. 
            navMeshAgent.enabled = false;
        }
        else if (navMeshAgent.enabled)
        {
            Vector3 pos = navMeshAgent.transform.position;
            navMeshAgent.destination = playerPositionSystem.FindClosest(pos);
        }
    }
    
    /// <summary>
    ///   Logic to handle the AI touching the ground. This reenables the
    ///   NavMeshAgent which then snaps the AI to the NavMesh surface again.
    /// </summary>
    private void OnCollisionEnter(Collision other)
    {
        // Check that we are on ground (Jumpable) but do not trigger on
        // the player, which is also Jumpable.
        if (other.gameObject.GetComponent<Jumpable>() != null &&
            other.gameObject.GetComponent<PlayerMovement>() == null)
        {            
            navMeshAgent.enabled = true;

            // Undo any rotation caused by physics interactions.
            navMeshAgent.transform.rotation = Quaternion.identity;
        }
    }
}
