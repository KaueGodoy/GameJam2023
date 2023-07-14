using UnityEngine;

public class ModularEnemyWorm : MonoBehaviour
{
    private IEnemyBehavior behavior;

    DistanceCheck distanceCheck;

    IdleBehavior idleBehavior;
    AttackExplosion attackExplosion;

    private void Start()
    {
        // Initialize the default behavior (e.g., RoamingBehavior)
        distanceCheck = GetComponent<DistanceCheck>();
        idleBehavior = GetComponent<IdleBehavior>();
        attackExplosion = GetComponent<AttackExplosion>();

        behavior = idleBehavior;
    }

    private void Update()
    {
        if (distanceCheck.IsPlayerInAttackRange())
        {
            // Switch to attack behavior
            ChangeBehavior(attackExplosion);

        }
        else if (distanceCheck.IsPlayerInChaseRange())
        {
            // Switch to chase behavior
            // ChangeBehavior(idleBehavior);

        }
        else
        {
            ChangeBehavior(idleBehavior);
        }

        // Update the current behavior
        behavior.UpdateBehavior();
    }

    public void ChangeBehavior(IEnemyBehavior newBehavior)
    {
        // Disable the current behavior
        if (behavior != null)
        {
            behavior.Disable();
        }

        // Enable and assign the new behavior
        behavior = newBehavior;
    }
}
