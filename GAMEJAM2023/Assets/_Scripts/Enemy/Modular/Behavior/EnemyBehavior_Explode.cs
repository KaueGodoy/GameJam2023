using System.Collections;
using UnityEngine;

public class EnemyBehavior_Explode : MonoBehaviour, IEnemyBehavior
{
    private Player _player;
    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;

    [SerializeField] private GameObject _explosionEffect;
    [Space]
    [SerializeField] private float _explosionRadius = 5f;
    [SerializeField] private int _explosionDamage = 20;
    [Space]
    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private float _positionThreshold = 0.1f;
    [SerializeField] private float _waitTime = 0.4f;

    private Vector3 _targetPosition;
    private Vector2 force;

    private Coroutine _moveTowardsCoroutine;

    [Space]
    public bool IsCooldown;
    public bool hasCharged;

    public float attackCooldown = 2f;
    public bool canAttack = true;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void UpdateBehavior()
    {
        if (!IsCooldown)
        {
            CallMoveTowards();
        }
    }

    private IEnumerator MoveTowardsCoroutine()
    {
        _spriteRenderer.color = Color.red;
        IsCooldown = true;

        Vector2 direction = (_player.transform.position - transform.position).normalized;
        direction.y = 0f;

        force = new Vector2(direction.x * _moveSpeed, direction.y);

        _rb.AddForce(force, ForceMode2D.Impulse);
        hasCharged = true;

        yield return new WaitForSeconds(_waitTime);

        if (_rb.velocity.magnitude <= _positionThreshold)
        {
            Explode();
        }

        hasCharged = false;
    }

    private void CallMoveTowards()
    {
        _moveTowardsCoroutine = StartCoroutine(MoveTowardsCoroutine());
    }

    public void PerformAttack()
    {
        _player.TakeDamage(_explosionDamage);
        DamagePopup.Create(_player.transform.position + Vector3.right + Vector3.up, (int)_explosionDamage);
    }

    private void Explode()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _explosionRadius);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                PerformAttack();
                Debug.Log("Player damaged by explosion");
            }
        }

        GameObject pfExplosionEffect = Instantiate(_explosionEffect, transform.position, transform.rotation);
        Destroy(pfExplosionEffect, 0.2f);

        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Explode();
        }
    }

    public void Disable()
    {

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _explosionRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, force);
    }

    private IEnumerator MoveTowardsCoroutineOld()
    {
        if (_targetPosition == Vector3.zero)
        {
            _targetPosition = _player.transform.position;
        }

        float distanceToTarget = Vector3.Distance(transform.position, _targetPosition);

        float step = _moveSpeed * Time.deltaTime;
        Vector3 newPosition = Vector3.MoveTowards(transform.position, _targetPosition, step);
        //newPosition.y = transform.position.y; 
        //newPosition.z = transform.position.z; 
        transform.position = newPosition;

        //Vector3 directionToPlayer = _targetPosition - transform.position;
        //directionToPlayer.Normalize();
        //directionToPlayer.y = 0f;
        //_rb.velocity = directionToPlayer * movementSpeed;

        if (distanceToTarget <= _positionThreshold)
        {
            Debug.Log("Enemy Attacked");

            Explode();

            canAttack = false;
            Invoke(nameof(ResetAttack), attackCooldown);
        }

        yield return new WaitForFixedUpdate();
    }

    private void ResetAttack()
    {
        canAttack = true;
        _targetPosition = Vector3.zero;
    }

}