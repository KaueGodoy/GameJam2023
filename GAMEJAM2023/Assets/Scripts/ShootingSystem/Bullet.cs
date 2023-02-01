using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;

    [Header("Speed")]
    public float bulletSpeed = 10f;

    [Header("Damage")]
    public float bulletDamage = 5f;

    void Start()
    {
        rb.velocity = transform.right * bulletSpeed; 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if(enemy != null)
        {
            enemy.TakeDamage(bulletDamage);
        }

        Destroy(gameObject);
    }


}
