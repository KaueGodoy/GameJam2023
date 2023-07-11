using UnityEngine;

public class AttackBehavior : MonoBehaviour, IEnemyBehavior
{
    public float attackCooldown = 2f;
    public int attackDamage = 10;
    
    private DistanceCheck distanceCheck;

    private bool canAttack = true;

    private void Start()
    {
        distanceCheck = GetComponent<DistanceCheck>();
    }

    public void UpdateBehavior()
    {
        if (distanceCheck.IsPlayerInAttackRange() && canAttack)
        {
            Attack();
        }
    }

    private void Attack()
    {
        // Perform attack logic here
        // For example, reduce player's health by attackDamage
        Debug.Log("Enemy Attacked");
        // Set attack cooldown
        canAttack = false;
        Invoke(nameof(ResetAttack), attackCooldown);
    }

    private void ResetAttack()
    {
        canAttack = true;
    }

    public void Disable()
    {

    }

    public void FlipSprite()
    {
        throw new System.NotImplementedException();
    }
}
