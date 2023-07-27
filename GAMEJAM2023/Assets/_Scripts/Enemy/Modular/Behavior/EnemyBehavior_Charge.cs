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

    public float damage = 1200f;

    private Vector2 force; // The force to be applied to the worm

    private Player _player;
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rb;

    private Coroutine _chargeAttackCoroutine;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
    }

    public void UpdateBehavior()
    {
        if (!IsCooldown && !isCharging && !hasCharged)
        {
            CallChargeAttack();
        }
    }

    public void CallChargeAttack()
    {
        _chargeAttackCoroutine = StartCoroutine(ChargeAttackCoroutine());
    }

    private IEnumerator ChargeAttackCoroutine()
    {
        _spriteRenderer.color = Color.red;
        IsCooldown = true;
        isCharging = true;
        Debug.Log("Charging");

        yield return new WaitForSeconds(chargeTime);

        isCharging = false;
        _spriteRenderer.color = Color.white;

        Vector2 direction = (_player.transform.position - transform.position).normalized;

        force = new Vector2(direction.x * speedForce, 0f);

        _rb.AddForce(force, ForceMode2D.Impulse);

        Debug.Log("Has charged");

        hasCharged = true;

        yield return new WaitForSeconds(cooldownDefault);


        if (playerHit)
        {
            yield return new WaitForSeconds(stepBackCooldown);
            _rb.velocity = Vector2.zero;

            Vector2 stepback = new Vector2(direction.x * -(speedForce / 2), 0f);
            _rb.AddForce(stepback, ForceMode2D.Impulse);

            playerHit = false;
        }
        else
        {
            _spriteRenderer.color = Color.yellow;
            _rb.velocity = Vector2.zero;
            yield return new WaitForSeconds(cooldownDefault);
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
            playerHit = true;
            PerformAttack();
        }
    }

    public void PerformAttack()
    {
        _player.TakeDamage(damage);
        DamagePopup.Create(_player.transform.position + Vector3.right + Vector3.up, (int)damage);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {

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
