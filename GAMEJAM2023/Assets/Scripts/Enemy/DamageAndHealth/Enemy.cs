using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] public float currentHealth;
    [SerializeField] public float maxHealth = 3f;

    [Header("Damage")]
    [SerializeField] public float damageAmount = 1f;

    [Header("Range")]
    public float rangeDistance = 5f;

    public PlayerMovement player;

    public bool animationHit;
    public bool isAlive;

    void Start()
    {
        currentHealth = maxHealth;
        animationHit = false;
        isAlive = true;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // HP update
            var healthComponent = collision.GetComponent<PlayerMovement>();
            if (healthComponent != null)
            {
                healthComponent.PlayerTakeDamage(damageAmount);
            }

            
            // knockback
            player.knockbackCounter = player.knockbackTotalTime;

            if (collision.transform.position.x <= transform.position.x)
            {
                player.knockbackFromRight = true;
            }
            if (collision.transform.position.x > transform.position.x)
            {
                player.knockbackFromRight = false;
            }

        }
    }

    public void TakeDamage(float damage)
    {
        animationHit = true;
        currentHealth -= damage;

        Invoke("HitComplete", 0.3f);
        
        if (currentHealth <= 0)
        {
            isAlive = false;
            Die();
        }
    }

    public void HitComplete()
    {
        animationHit = false;
    }

    void Die()
    {
        //GetComponent<LootBag>().InstantiateLoot(transform.position);
        Destroy(gameObject);
    }


}
