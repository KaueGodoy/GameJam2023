using UnityEngine;

public class AttackExplosion : MonoBehaviour, IEnemyBehavior
{
    public float attackCooldown = 2f;
    public int attackDamage = 10;
    public float movementSpeed = 10f;
    public float positionThreshold = 0.1f;

    [SerializeField]
    private float explosionRadius = 5f;

    [SerializeField]
    private GameObject explosionEffect;

    public float ExplosionRadius
    {
        get { return explosionRadius; }
        set { explosionRadius = value; }
    }

    public int explosionDamage = 20;

    private bool canAttack = true;
    private Transform playerTransform;
    private Vector3 targetPosition;

    private void Start()
    {
        // Find the player's transform
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void UpdateBehavior()
    {
        if (canAttack)
        {
            Attack();
        }
    }

    private void Attack()
    {
        if (targetPosition == Vector3.zero)
        {
            // Set the target position as the player's current position
            targetPosition = playerTransform.position;
        }

        // Move towards the target position
        float step = movementSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

        // Check if the enemy has reached the target position
        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);
        if (distanceToTarget <= positionThreshold)
        {
            // Perform attack logic here
            // For example, reduce player's health by attackDamage
            Debug.Log("Enemy Attacked");

            // Explode and deal AOE damage
            Explode();

            // Set attack cooldown
            canAttack = false;
            Invoke(nameof(ResetAttack), attackCooldown);
        }
    }

    private void Explode()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (Collider2D collider in colliders)
        {
            // Check if the collider belongs to the player
            if (collider.CompareTag("Player"))
            {
                // Reduce player's health by explosionDamage
                // You might have to access the player's health component or script to do this
                // For example: collider.GetComponent<Health>().ReduceHealth(explosionDamage);
                Debug.Log("Player damaged by explosion");
            }
        }

        // Explosion effect
        GameObject pfExplosionEffect = Instantiate(explosionEffect, transform.position, transform.rotation);
        Destroy(pfExplosionEffect, 0.2f);

        // Destroy enemy
        //Destroy(gameObject);
    }



    private void ResetAttack()
    {
        canAttack = true;
        targetPosition = Vector3.zero; // Reset the stored target position
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Explode();
        }
    }

    public void Disable()
    {
        // Clean up any ongoing attack logic if needed
        // For example, if the enemy is currently moving towards the target position, you can stop it here
    }

    private void OnDrawGizmosSelected()
    {
        // Draw a wire circle gizmo to visualize the explosion radius
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
