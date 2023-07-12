using UnityEngine;

public class DistanceCheck : MonoBehaviour
{
    private Transform playerTransform;
    private FlipSprite flipSprite;

    public float roamDistance = 8f;
    public float chaseDistance = 4f;
    public float attackDistance = 1f;

    private void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
        }

        flipSprite = GetComponent<FlipSprite>();
    }

    public bool IsPlayerInAttackRange()
    {
        float distance = Vector2.Distance(transform.position, playerTransform.position);
        flipSprite.FlipBasedOnPlayerPosition();
        return distance <= attackDistance;
    }

    public bool IsPlayerInChaseRange()
    {
        float distance = Vector2.Distance(transform.position, playerTransform.position);
        flipSprite.FlipBasedOnPlayerPosition();
        return distance <= chaseDistance;
    }

    public bool IsPlayerInRoamingDistance()
    {
        float distance = Vector2.Distance(transform.position, playerTransform.position);
        return distance <= roamDistance;
    }

    private void OnDrawGizmosSelected()
    {
        // Draw chase distance Gizmo
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);

        // Draw attack distance Gizmo
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);  
        
        // Draw roaming distance Gizmo
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, roamDistance);
    }
}
