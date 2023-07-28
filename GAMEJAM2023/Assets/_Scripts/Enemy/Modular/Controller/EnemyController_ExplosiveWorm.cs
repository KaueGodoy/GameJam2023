using UnityEngine;

public class EnemyController_ExplosiveWorm : MonoBehaviour
{
    private IEnemyBehavior _behavior;

    private DistanceCheck _distanceCheck;

    private EnemyBehavior_Idle _idle;
    private EnemyBehavior_Explode _explodeAttack;

    private void Awake()
    {
        _distanceCheck = GetComponent<DistanceCheck>();

        _idle = GetComponent<EnemyBehavior_Idle>();
        _explodeAttack = GetComponent<EnemyBehavior_Explode>();
    }

    private void Start()
    {
        _behavior = _idle;
    }

    private void Update()
    {
        if (_distanceCheck.IsPlayerInAttackRange())
        {
            ChangeBehavior(_explodeAttack);
        }
        else if (_distanceCheck.IsPlayerInChaseRange())
        {
            // ChangeBehavior(idleBehavior);
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
