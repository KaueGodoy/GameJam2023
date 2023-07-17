using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController_Wasp : MonoBehaviour
{
    private IEnemyBehavior behavior;

    DistanceCheck distanceCheck;

    IdleBehavior idleBehavior;
    EnemyBehavior_Shoot shoot;

    private void Start()
    {
        distanceCheck = GetComponent<DistanceCheck>();

        idleBehavior = GetComponent<IdleBehavior>();
        shoot = GetComponent<EnemyBehavior_Shoot>();

        behavior = idleBehavior;
    }

    private void Update()
    {
        if (distanceCheck.IsPlayerInAttackRange())
        {
            // Switch to attack behavior
            ChangeBehavior(shoot);

        }
        else if (distanceCheck.IsPlayerInChaseRange())
        {
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

