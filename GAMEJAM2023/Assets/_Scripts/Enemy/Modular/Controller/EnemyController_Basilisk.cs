using UnityEngine;

public class EnemyController_Basilisk : MonoBehaviour
{
    private IEnemyBehavior behavior;

    DistanceCheck distanceCheck;

    IdleBehavior idleBehavior;
    EnemyBehavior_Bite biteAttack;


    private void Start()
    {
        distanceCheck = GetComponent<DistanceCheck>();

        idleBehavior = GetComponent<IdleBehavior>();
        biteAttack = GetComponent<EnemyBehavior_Bite>();

        behavior = idleBehavior;
    }

    private void Update()
    {
        if (distanceCheck.IsPlayerInAttackRange())
        {
            // Switch to attack behavior
            ChangeBehavior(biteAttack);

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
