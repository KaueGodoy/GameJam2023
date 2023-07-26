using UnityEngine;

public class ModularEnemy : MonoBehaviour
{
    private DistanceCheck distanceCheck;
    private EnemyBehavior_Chase _chase;
    private AttackBehavior attackBehavior;
    private EnemyBehavior_Patrol patrolBehavior;
    private EnemyBehavior_Idle idleBehavior;

    private IEnemyBehavior behavior;

    private void Start()
    {
        // Initialize the default behavior (e.g., RoamingBehavior)

        distanceCheck = GetComponent<DistanceCheck>();

        _chase = GetComponent<EnemyBehavior_Chase>();
        attackBehavior = GetComponent<AttackBehavior>();
        patrolBehavior = GetComponent<EnemyBehavior_Patrol>();
        idleBehavior = GetComponent<EnemyBehavior_Idle>();

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
            ChangeBehavior(_chase);
            _chase.enabled = true;

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
