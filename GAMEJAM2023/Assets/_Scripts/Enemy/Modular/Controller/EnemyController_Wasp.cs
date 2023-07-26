using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController_Wasp : MonoBehaviour
{
    private IEnemyBehavior _behavior;

    private DistanceCheck _distanceCheck;

    private EnemyBehavior_Idle _idle;
    private EnemyBehavior_Shoot _shoot;
    private EnemyBehavior_Patrol _patrol;

    private void Awake()
    {
        _distanceCheck = GetComponent<DistanceCheck>();

        _idle = GetComponent<EnemyBehavior_Idle>();
        _shoot = GetComponent<EnemyBehavior_Shoot>();
        _patrol = GetComponent<EnemyBehavior_Patrol>();
    }

    private void Start()
    {
        _behavior = _idle;
    }

    private void Update()
    {
        if (_distanceCheck.IsPlayerInAttackRange())
        {
            ChangeBehavior(_shoot);
        }
        else if (_distanceCheck.IsPlayerInChaseRange())
        {

        }
        else if (_distanceCheck.IsPlayerInPatrolRange())
        {
            ChangeBehavior(_patrol);
        }
        else
        {
            ChangeBehavior(_idle);
        }

        _behavior.UpdateBehavior();
    }

    public void ChangeBehavior(IEnemyBehavior newBehavior)
    {
        if (_behavior != null)
        {
            _behavior.Disable();
        }

        _behavior = newBehavior;
    }
}

