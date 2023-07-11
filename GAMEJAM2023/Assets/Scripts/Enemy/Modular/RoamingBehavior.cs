using UnityEngine;

public class RoamingBehavior : MonoBehaviour, IEnemyBehavior
{
    public float moveSpeed = 3f;
    public float patrolDistance = 5f;

    private bool isMovingRight = true;
    private Vector2 originalPosition;
    private Vector2 destination;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalPosition = rb.position;
        destination = originalPosition + new Vector2(patrolDistance, 0f);
    }

    public void UpdateBehavior()
    {
        if (isMovingRight)
        {
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
            if (rb.position.x >= destination.x)
            {
                isMovingRight = false;
                FlipSprite();
            }
        }
        else
        {
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
            if (rb.position.x <= originalPosition.x)
            {
                isMovingRight = true;
                FlipSprite();
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
        rb.velocity = Vector2.zero; // Assuming you have a Rigidbody2D component for movement

    }

    public void FlipSprite()
    {
        spriteRenderer.flipX = !spriteRenderer.flipX;
    }
}
