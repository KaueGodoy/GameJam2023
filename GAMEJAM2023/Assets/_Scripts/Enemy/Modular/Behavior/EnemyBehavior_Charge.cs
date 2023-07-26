using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior_Charge : MonoBehaviour, IEnemyBehavior
{
    public float chargeTime = 1.5f; // The time it takes for the enemy to charge the attack
    public float speedForce = 10f; // The speed at which the enemy charges towards the player

    public float cooldownDefault = 0.5f; // The minimum cooldown duration
    public float cooldownMin = 0.5f; // The minimum cooldown duration
    public float cooldownMax = 1.5f; // The maximum cooldown duration
    public float stepBackCooldown = 1f; // The duration of the step back cooldown

    public bool IsCooldown = false;
    public bool isCharging = false;
    public bool hasCharged = false;
    public bool playerHit = false;

    private Vector2 force; // The force to be applied to the worm

    private Transform _player;
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rb;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void UpdateBehavior()
    {
        if (!IsCooldown && !isCharging && !hasCharged)
        {
            StartCoroutine(Charge());
        }
    }

    private IEnumerator Charge()
    {
        _spriteRenderer.color = Color.red;

        IsCooldown = true;

        isCharging = true;

        Debug.Log("Charging");

        yield return new WaitForSeconds(chargeTime);

        isCharging = false;

        _spriteRenderer.color = Color.white;

        Vector2 direction = (_player.position - transform.position).normalized;

        force = new Vector2(direction.x * speedForce, _rb.velocity.y);

        _rb.AddForce(force, ForceMode2D.Impulse);

        Debug.Log("Has charged");

        hasCharged = true;

        yield return new WaitForSeconds(cooldownDefault);

        if (playerHit)
        {
            yield return new WaitForSeconds(stepBackCooldown);
            _rb.velocity = Vector2.zero;

            Vector2 stepback = new Vector2(direction.x * -(speedForce / 2), _rb.velocity.y);
            _rb.AddForce(stepback, ForceMode2D.Impulse);
        }

        float cooldown = Random.Range(cooldownMin, cooldownMax);

        yield return new WaitForSeconds(cooldown);
        _rb.velocity = Vector2.zero;

        hasCharged = false;
        IsCooldown = false;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player hit");
            playerHit = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        playerHit = false;
    }

    public void Disable()
    {

    }

    private void OnDrawGizmosSelected()
    {
        // Draw the charging distance
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, force);
    }
}
