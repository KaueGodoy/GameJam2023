using UnityEngine;

public class RoamingBehavior : MonoBehaviour, IEnemyBehavior
{

    private Vector2 originalPosition;
    private Vector2 destination;
    private Rigidbody2D rb;

    public float moveSpeed = 3f;
    public float patrolDistance = 5f;

    public bool isMovingRight;

    private FlipSprite flipSprite;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalPosition = rb.position;
        destination = originalPosition + new Vector2(patrolDistance, 0f);

        flipSprite = GetComponent<FlipSprite>();
    }

    public void UpdateBehavior()
    {
        if (isMovingRight)
        {
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
            if (rb.position.x >= destination.x)
            {
                flipSprite.FlipRoam();
                isMovingRight = false;
            }
        }
        else
        {
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
            if (rb.position.x <= originalPosition.x)
            {
                flipSprite.FlipRoam();
                isMovingRight = true;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(patrolDistance, 0f));
    }

    public void Disable()
    {
        rb.velocity = Vector2.zero;
    }

}
