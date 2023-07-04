using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private GameObject player;
    private Rigidbody2D rb;

    [Header("Speed")]
    public float bulletForce = 2f;

    [Header("Damage")]
    public float bulletDamage = 1f;

    [Header("Timer")]
    public float bulletTimer = 0f;
    public float bulletMaxTimer = 5f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");

        // calculates the player direction
        Vector3 direction = player.transform.position - transform.position;
        rb.velocity = new Vector2(direction.x, direction.y).normalized * bulletForce;

        // rotates the bullet
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // HP update
            var healthComponent = collision.GetComponent<Player>();
            if (healthComponent != null)
            {
                healthComponent.PlayerTakeDamage(bulletDamage);
            }

       

            Destroy(gameObject);

        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
