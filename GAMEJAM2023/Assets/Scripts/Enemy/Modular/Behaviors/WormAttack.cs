using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormAttack : MonoBehaviour, IEnemyBehavior
{
    public float jumpForce = 5f; // The force applied to the worm when jumping
    public float cooldown = 2f; // The cooldown between attacks
    public float stunDuration = 2f; // The duration of the stun after jumping

    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer component
    private Transform player; // Reference to the player's transform
    private Rigidbody2D rb; // Reference to the worm's Rigidbody2D component

    private bool isJumping = false; // Flag to check if the worm is currently jumping
    private bool isCooldown = false; // Flag to check if the attack is on cooldown
    private bool isStunned = false; // Flag to check if the worm is currently stunned
    private bool hasJumped = false; // Flag to check if the worm has jumped

    private Vector2 force; // The force to be applied to the worm

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();

        // Get the reference to the SpriteRenderer component
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Disable()
    {
        // Disable any behavior-specific functionality here
    }

    public void UpdateBehavior()
    {
        if (!isJumping && !isCooldown && !isStunned)
        {
            StartCoroutine(AttackCooldown());
        }
    }

    private IEnumerator AttackCooldown()
    {
        // Set the cooldown flag to true
        isCooldown = true;

        // Calculate the direction towards the player
        Vector2 direction = (player.position - transform.position).normalized;

        // Set the jumping flag to true
        isJumping = true;

        // Calculate the force to be applied to the worm
        force = new Vector2(direction.x, direction.y + 1f) * jumpForce;

        // Apply the jump force to the worm
        rb.AddForce(force, ForceMode2D.Impulse);

        // Set the hasJumped flag to true
        hasJumped = true;

        // Reset the jumping flag
        isJumping = false;

        // Set the stun flag to true
        isStunned = true;

        // Wait for the stun duration
        yield return new WaitForSeconds(stunDuration);

        // Set the stun flag to false
        isStunned = false;

        // Reset the hasJumped flag to false
        hasJumped = false;

        // Flip the sprite back to normal
        spriteRenderer.flipY = false;

        // Wait for the cooldown duration
        yield return new WaitForSeconds(cooldown);

        // Set the cooldown flag to false
        isCooldown = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        CheckGrounded checkGrounded = GetComponent<CheckGrounded>();

        if (hasJumped && checkGrounded.IsGrounded())
        {
            // Flip the sprite on the y-axis
            spriteRenderer.flipY = true;
        }


        // Check if the worm has landed on the ground
        if (checkGrounded.IsGrounded())
        {
            // Reset the jumping flag
            isJumping = false;
            Debug.Log("On ground");
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player takes damage");
        }
    }


    private void OnDrawGizmosSelected()
    {
        // Draw a debug ray to visualize the force in the Unity editor
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, force);
    }
}
