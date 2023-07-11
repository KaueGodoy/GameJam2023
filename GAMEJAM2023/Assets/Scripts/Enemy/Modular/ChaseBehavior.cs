using UnityEngine;

public class ChaseBehavior : MonoBehaviour, IEnemyBehavior
{
    private Transform playerTransform;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    public float moveSpeed = 3f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
        }
    }

    public void UpdateBehavior()
    {

        Vector2 direction = (playerTransform.position - transform.position).normalized;
        rb.velocity = direction * moveSpeed;
    }

    public void Disable()
    {
        // Stop the enemy's movement or reset relevant variables
        rb.velocity = Vector2.zero; // Assuming you have a Rigidbody2D component for movement
        // Other clean-up or deactivation logic specific to the chase behavior
    }

}
