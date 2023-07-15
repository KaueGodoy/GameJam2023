using UnityEngine;

public class EnemyController_Worm : MonoBehaviour
{
    private IEnemyBehavior behavior;

    DistanceCheck distanceCheck;

    IdleBehavior idleBehavior;
    WormAttack wormAttack;


    private void Start()
    {
        // Initialize the default behavior (e.g., RoamingBehavior)
        distanceCheck = GetComponent<DistanceCheck>();
        idleBehavior = GetComponent<IdleBehavior>();
        wormAttack = GetComponent<WormAttack>();

        behavior = idleBehavior;
    }

    private void Update()
    {
        if (distanceCheck.IsPlayerInAttackRange())
        {
            // Switch to attack behavior
            ChangeBehavior(wormAttack);

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
