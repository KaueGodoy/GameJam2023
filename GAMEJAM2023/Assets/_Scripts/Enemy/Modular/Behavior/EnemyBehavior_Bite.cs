using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior_Bite : MonoBehaviour, IEnemyBehavior
{
    private Transform player; // Reference to the player's transform

    public float chargeTime = 1.5f; // The time it takes for the enemy to charge the attack
    public float speedForce = 10f; // The speed at which the enemy charges towards the player

    public float cooldownDefault = 0.5f; // The minimum cooldown duration
    public float cooldownMin = 0.5f; // The minimum cooldown duration
    public float cooldownMax = 1.5f; // The maximum cooldown duration
    public float stepBackCooldown = 1f; // The duration of the step back cooldown

    public bool isCooldown = false;
    public bool isCharging = false;
    public bool hasCharged = false;
    public bool playerHit = false;

    private Vector2 force; // The force to be applied to the worm

    private Rigidbody2D rb;

    public QTEUI QTEUI;
    public float QTEAmount = 10;

    private void Start()
    {
        // Find the player object and store its transform
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
    }

    public void UpdateBehavior()
    {
        if (!isCooldown && !isCharging && !hasCharged)
        {
            StartCoroutine(Charge());
        }
    }

    private IEnumerator Charge()
    {
        isCooldown = true;

        isCharging = true;

        Debug.Log("Charging");

        yield return new WaitForSeconds(chargeTime);

        isCharging = false;

        Vector2 direction = (player.position - transform.position).normalized;

        force = new Vector2(direction.x * speedForce, rb.velocity.y);

        rb.AddForce(force, ForceMode2D.Impulse);

        Debug.Log("Has charged");

        hasCharged = true;

        yield return new WaitForSeconds(cooldownDefault);

        if (playerHit)
        {
            // CC
            //QTESystem.TriggerQTE(playerHit);
        }


        float cooldown = Random.Range(cooldownMin, cooldownMax);

        yield return new WaitForSeconds(cooldown);
        rb.velocity = Vector2.zero;

        hasCharged = false;
        isCooldown = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player hit");
            playerHit = true;
            QTEUI.InstantiateQTEUI(QTEAmount);
            //collision.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        playerHit = false;
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
