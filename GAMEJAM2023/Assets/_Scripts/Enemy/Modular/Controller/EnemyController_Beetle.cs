using UnityEngine;

public class EnemyController_Beetle : MonoBehaviour
{
    private IEnemyBehavior _behavior;

    private DistanceCheck _distanceCheck;

    private EnemyBehavior_Idle _idle;
    private EnemyBehavior_Patrol _patrol;
    private EnemyBehavior_Chase _chase;
    private EnemyBehavior_Charge _chargedAttack;

    private void Awake()
    {
        _distanceCheck = GetComponent<DistanceCheck>();

        _idle = GetComponent<EnemyBehavior_Idle>();
        _patrol = GetComponent<EnemyBehavior_Patrol>();
        _chase = GetComponent<EnemyBehavior_Chase>();
        _chargedAttack = GetComponent<EnemyBehavior_Charge>();
    }

    private void Start()
    {
        _behavior = _idle;
    }

    private void Update()
    {
        if (_distanceCheck.IsPlayerInAttackRange())
        {
            ChangeBehavior(_chargedAttack);
        }
        else if (_distanceCheck.IsPlayerInChaseRange())
        {
            ChangeBehavior(_chase);
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

