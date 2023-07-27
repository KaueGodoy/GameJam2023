using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class EnemyBehavior_JumpTowards : MonoBehaviour, IEnemyBehavior
{
    private Player _player;
    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;

    public float damage = 300f;

    public float jumpForce = 5f; // The force applied to the worm when jumping
    public float cooldown = 2f; // The cooldown between attacks
    public float stunDuration = 2f; // The duration of the stun after jumping

    private bool isJumping = false; // Flag to check if the worm is currently jumping
    private bool isCooldown = false; // Flag to check if the attack is on cooldown
    private bool isStunned = false; // Flag to check if the worm is currently stunned
    private bool hasJumped = false; // Flag to check if the worm has jumped

    private Vector2 force; // The force to be applied to the worm

    private Coroutine _jumpTowardsCoroutine;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void UpdateBehavior()
    {
        if (!isCooldown && !isJumping && !isStunned)
        {
            CallJumpTowards();
        }
    }

    private void CallJumpTowards()
    {
        _jumpTowardsCoroutine = StartCoroutine(JumpTowards());
    }

    private IEnumerator JumpTowards()
    {
        isCooldown = true;
        _rb.gravityScale = 4f;

        Vector2 direction = (_player.transform.position - transform.position).normalized;

        isJumping = true;

        force = new Vector2(direction.x, direction.y + 1f) * jumpForce;

        _rb.AddForce(force, ForceMode2D.Impulse);

        hasJumped = true;
        isJumping = false;
        isStunned = true;

        yield return new WaitForSeconds(stunDuration);

        isStunned = false;
        hasJumped = false;

        _spriteRenderer.flipY = false;

        yield return new WaitForSeconds(cooldown);

        isCooldown = false;
        _rb.gravityScale = 1f;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        CheckGrounded checkGrounded = GetComponent<CheckGrounded>();

        if (hasJumped && checkGrounded.IsGrounded())
        {
            // Flip the sprite on the y-axis
            _spriteRenderer.flipY = true;
        }

        if (checkGrounded.IsGrounded())
        {
            isJumping = false;
            Debug.Log("On ground");
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            PerformAttack();
        }
    }

    public void PerformAttack()
    {
        _player.TakeDamage(damage);
        DamagePopup.Create(_player.transform.position + Vector3.right + Vector3.up, (int)damage);
    }

    public void Disable()
    {

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, force);
    }
}
