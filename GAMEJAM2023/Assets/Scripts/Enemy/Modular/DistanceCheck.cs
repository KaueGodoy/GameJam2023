using UnityEngine;

public class DistanceCheck : MonoBehaviour
{
    private Transform playerTransform;
    
    public float chaseDistance = 4f;
    public float attackDistance = 1f;

    private void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
        }
    }

    public bool IsPlayerInChaseRange()
    {
        float distance = Vector2.Distance(transform.position, playerTransform.position);
        return distance <= chaseDistance;
    }

    public bool IsPlayerInAttackRange()
    {
        float distance = Vector2.Distance(transform.position, playerTransform.position);
        return distance <= attackDistance;
    }

    private void OnDrawGizmosSelected()
    {
        // Draw chase distance Gizmo
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);

        // Draw attack distance Gizmo
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);
    }
}
