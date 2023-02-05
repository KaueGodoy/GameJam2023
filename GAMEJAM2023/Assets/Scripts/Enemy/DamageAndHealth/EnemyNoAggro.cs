using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyNoAggro : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] public float currentHealth;
    [SerializeField] public float maxHealth = 3f;

    [Header("Damage")]
    [SerializeField] public float damageAmount = 1f;

    public PlayerMovement player;

    public bool animationHit;
    public bool aggro;


    void Start()
    {
        currentHealth = maxHealth;
        animationHit = false;
        aggro = false;
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

            // UI update
            var heartComponent = collision.GetComponent<HeartSystem>();
            if (heartComponent != null)
            {
                heartComponent.curLife -= Mathf.FloorToInt(damageAmount);

                
            }
            
           

        }
    }

    public void TakeDamage(float damage)
    {
        animationHit = true;
        aggro = true;
        currentHealth -= damage;

        Invoke("HitComplete", 0.3f);
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void HitComplete()
    {
        animationHit = false;
    }

    void Die()
    {
        Destroy(gameObject);
    }


}
