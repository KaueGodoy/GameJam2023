using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Trap : MonoBehaviour
{

    [Header("Damage")]
    [SerializeField] public float damageAmount = 1f;

    public PlayerMovement player;

    public bool animationHit;

    void Start()
    { 
        animationHit = false;
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


    public void HitComplete()
    {
        animationHit = false;
    }



}
