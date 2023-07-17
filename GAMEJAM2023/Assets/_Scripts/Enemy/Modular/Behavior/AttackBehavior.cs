using UnityEngine;

public class AttackBehavior : MonoBehaviour, IEnemyBehavior
{
    public float attackCooldown = 2f;
    public int attackDamage = 10;

    private bool canAttack = true;

    public void UpdateBehavior()
    {
        if (canAttack)
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
}
