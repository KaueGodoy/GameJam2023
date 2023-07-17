using UnityEngine;

public class ModularEnemy : MonoBehaviour
{
    private DistanceCheck distanceCheck;
    private ChaseBehavior chaseBehavior;
    private AttackBehavior attackBehavior;
    private RoamingBehavior roamingBehavior;
    private IdleBehavior idleBehavior;

    private IEnemyBehavior behavior;

    private void Start()
    {
        // Initialize the default behavior (e.g., RoamingBehavior)

        distanceCheck = GetComponent<DistanceCheck>();

        chaseBehavior = GetComponent<ChaseBehavior>();
        attackBehavior = GetComponent<AttackBehavior>();
        roamingBehavior = GetComponent<RoamingBehavior>();
        idleBehavior = GetComponent<IdleBehavior>();

        behavior = idleBehavior;
    }

    private void Update()
    {
        if (distanceCheck.IsPlayerInAttackRange())
        {
            // Switch to attack behavior
            ChangeBehavior(attackBehavior);
            attackBehavior.enabled = true;

        }
        else if (distanceCheck.IsPlayerInChaseRange())
        {
            // Switch to chase behavior
            ChangeBehavior(chaseBehavior);
            chaseBehavior.enabled = true;

        }
        //else if (distanceCheck.IsPlayerInRoamingDistance())
        //{
        //    // Switch to roaming behavior
        //    ChangeBehavior(roamingBehavior);
        //    chaseBehavior.enabled = false;
        //    attackBehavior.enabled = false;
        //}
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
