using Cinemachine.Utility;
using UnityEngine;

public class EnemyBehavior_Patrol : MonoBehaviour, IEnemyBehavior
{
    private Rigidbody2D _rb;

    private FlipSprite _flipSprite;

    private Vector2 _originalPosition;
    private Vector2 _destination;

    public float MoveSpeed = 3f;
    public float PatrolDistance = 5f;

    public bool isMovingRight;


    public float baseSpeed = 3f;
    public float baseSpeedBonus = 0f;
    protected EffectableObject Effectable;

    public float CurrentSpeed
    {
        get
        {
            float currentSpeed = Effectable.Effect_GroundSpeed(baseSpeed * (1 + Effectable.Effect_BonusGroundSpeed(baseSpeedBonus)));

            return Mathf.Max(currentSpeed, 0f);
        }
    }

    private void Awake()
    {
        Effectable = GetComponent<EffectableObject>();

        _rb = GetComponent<Rigidbody2D>();
        _flipSprite = GetComponent<FlipSprite>();
    }

    private void Start()
    {
        _originalPosition = _rb.position;
        _destination = _originalPosition + new Vector2(PatrolDistance, 0f);
    }

    public void UpdateBehavior()
    {
        _destination = _originalPosition + new Vector2(PatrolDistance, 0f);

        if (isMovingRight)
        {
            _rb.velocity = new Vector2(CurrentSpeed, _rb.velocity.y);
            if (_rb.position.x >= _destination.x)
            {
                isMovingRight = false;
            }
        }
        else
        {
            _rb.velocity = new Vector2(-CurrentSpeed, _rb.velocity.y);
            if (_rb.position.x <= _originalPosition.x)
            {
                isMovingRight = true;
            }
        }

        _flipSprite.FlipBasedOnMovingDirection();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(PatrolDistance, 0, 0));
    }

    public void Disable()
    {
        _rb.velocity = Vector2.zero;
    }

}
