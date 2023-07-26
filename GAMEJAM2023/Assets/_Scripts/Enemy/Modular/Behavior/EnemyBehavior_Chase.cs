using UnityEngine;

public class EnemyBehavior_Chase : MonoBehaviour, IEnemyBehavior
{
    private Transform _playerTransform;
    private Rigidbody2D _rb;
    private FlipSprite _flipSprite;

    public float MoveSpeed = 3f;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _flipSprite = GetComponent<FlipSprite>();
    }

    private void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            _playerTransform = playerObject.transform;
        }
    }

    public void UpdateBehavior()
    {
        Vector2 direction = (_playerTransform.position - transform.position).normalized;
        _rb.velocity = direction * MoveSpeed;
        _flipSprite.FlipBasedOnMovingDirection();
    }

    public void Disable()
    {
        _rb.velocity = Vector2.zero;
    }

}
