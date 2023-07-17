using UnityEngine;

public class ChaseBehavior : MonoBehaviour, IEnemyBehavior
{
    private Transform playerTransform;
    private Rigidbody2D rb;

    public float moveSpeed = 3f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

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
        rb.velocity = Vector2.zero;
    }

}
