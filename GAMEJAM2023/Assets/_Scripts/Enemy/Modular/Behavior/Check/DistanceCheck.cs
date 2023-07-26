using UnityEngine;

public class DistanceCheck : MonoBehaviour
{
    private Transform _playerTransform;
    private FlipSprite _flipSprite;

    public float patrolDistance = 8f;
    public float chaseDistance = 4f;
    public float attackDistance = 1f;

    public float attackLongDistance = 1f;
    public float attackMidDistance = 1f;
    public float attackShortDistance = 1f;

    private void Awake()
    {
        _flipSprite = GetComponent<FlipSprite>();
    }

    private void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null) _playerTransform = playerObject.transform;
    }

    public bool IsPlayerInAttackRange()
    {
        float distance = Vector2.Distance(transform.position, _playerTransform.position);
        //_flipSprite.FlipBasedOnPlayerPosition();
        _flipSprite.FlipTest();
        return distance <= attackDistance;
    }

    public bool IsPlayerInChaseRange()
    {
        float distance = Vector2.Distance(transform.position, _playerTransform.position);
        return distance <= chaseDistance;
    }

    public bool IsPlayerInPatrolRange()
    {
        float distance = Vector2.Distance(transform.position, _playerTransform.position);
        return distance <= patrolDistance;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);

        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, patrolDistance);
    }
}
