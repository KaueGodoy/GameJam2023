using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile_Wasp : MonoBehaviour
{
    private Player _player;
    private Rigidbody2D _rb;

    [Header("Speed")]
    public float bulletForce = 5f;

    [Header("Damage")]
    public float damage = 1500f;

    [Header("Timer")]
    public float bulletTimer = 0f;
    public float bulletMaxTimer = 5f;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    void Start()
    {
        Vector3 direction = _player.transform.position - transform.position;
        direction.Normalize();

        _rb.velocity = direction * bulletForce;

        float bulletRotation = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, bulletRotation + 90f);
    }

    void Update()
    {
        bulletTimer += Time.deltaTime;

        if (bulletTimer > bulletMaxTimer)
        {
            Destroy(gameObject);
        }
    }

    public void PerformAttack()
    {
        _player.TakeDamage(damage);
        DamagePopup.Create(_player.transform.position + Vector3.right + Vector3.up, (int)damage);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PerformAttack();
            Destroy(gameObject);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
