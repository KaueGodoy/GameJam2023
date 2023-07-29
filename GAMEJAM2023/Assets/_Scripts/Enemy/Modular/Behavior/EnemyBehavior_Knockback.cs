using System.Collections;
using UnityEngine;

public class EnemyBehavior_Knockback : MonoBehaviour
{
    private Transform _player;
    private Rigidbody2D _rb;

    public float knockbackForce = 12f;
    public float KnockbackTime = 0.18f;
    public float KnockbackCooldown = 0.75f;
    public float KnockbackCooldownTime = 0f;
    public bool IsKnockback;

    public AnimationCurve knockbackCurve;

    public Vector2 force;

    private Coroutine _knockbackCoroutine;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _rb = _player.transform.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (IsKnockback)
        {
            CallKnockback();
        }
    }

    public void CallKnockback()
    {
        _knockbackCoroutine = StartCoroutine(KnockbackCoroutine());
    }

    public IEnumerator KnockbackCoroutine()
    {
        if (_rb != null)
        {
            _rb.velocity = Vector3.zero;

            Vector2 direction = (_player.position - transform.position).normalized;

            Bounds playerBounds = _player.transform.GetComponent<Collider2D>().bounds;
            Bounds enemyBounds = GetComponent<Collider2D>().bounds;

            bool playerAboveEnemy = playerBounds.min.y > enemyBounds.max.y;

            float upwardKnockbackForce = playerAboveEnemy ? 0.5f : 0f;
            float elapsedTime = 0f;

            while (elapsedTime < KnockbackTime)
            {
                elapsedTime += Time.fixedDeltaTime;

                Vector2 directionalforce = new Vector2(direction.x * knockbackForce, upwardKnockbackForce);

                force = directionalforce * knockbackCurve.Evaluate(elapsedTime);

                _rb.AddForce(force);

                yield return new WaitForFixedUpdate();
            }

            Debug.Log("Knockback applied");
        }

        IsKnockback = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && Time.time > KnockbackCooldownTime)
        {
            IsKnockback = true;
            KnockbackCooldownTime = Time.time + KnockbackCooldown;
        }
    }

}