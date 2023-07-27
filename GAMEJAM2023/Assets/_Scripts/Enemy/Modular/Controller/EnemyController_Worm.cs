using UnityEngine;

public class EnemyController_Worm : MonoBehaviour
{
    private IEnemyBehavior _behavior;

    private DistanceCheck _distanceCheck;

    private EnemyBehavior_Idle _idle;
    private EnemyBehavior_JumpTowards _jumpAttack;
    private EnemyBehavior_Patrol _patrol;

    private void Awake()
    {
        _distanceCheck = GetComponent<DistanceCheck>();
        _idle = GetComponent<EnemyBehavior_Idle>();
        _patrol = GetComponent<EnemyBehavior_Patrol>();
        _jumpAttack = GetComponent<EnemyBehavior_JumpTowards>();
    }

    private void Start()
    {
        _behavior = _idle;
    }

    private void Update()
    {
        if (_distanceCheck.IsPlayerInAttackRange())
        {
            ChangeBehavior(_jumpAttack);
        }
        else if (_distanceCheck.IsPlayerInChaseRange())
        {
            // ChangeBehavior(idleBehavior);
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
