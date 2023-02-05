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
    [SerializeField] public float damageAmount = 0f;

    [Header("Range")]
    public float rangeDistance = 5f;

    public PlayerMovement player;

    public bool animationHit;
    public bool aggro;


    void Start()
    {
        currentHealth = maxHealth;
        animationHit = false;
        aggro = false;
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
