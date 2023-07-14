using UnityEngine;

public class AttackExplosion : MonoBehaviour, IEnemyBehavior
{
    public float attackCooldown = 2f;
    public int attackDamage = 10;
    public float movementSpeed = 10f;
    public float positionThreshold = 0.1f;

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

            // Set attack cooldown
            canAttack = false;
            Invoke(nameof(ResetAttack), attackCooldown);
        }
    }

    private void ResetAttack()
    {
        canAttack = true;
        targetPosition = Vector3.zero; // Reset the stored target position
    }

    public void Disable()
    {
        // Clean up any ongoing attack logic if needed
        // For example, if the enemy is currently moving towards the target position, you can stop it here
    }
}
