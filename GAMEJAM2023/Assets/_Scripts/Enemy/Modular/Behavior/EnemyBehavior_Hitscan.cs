using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior_Hitscan : MonoBehaviour, IEnemyBehavior
{
    [Header("Hitscan")]
    public LayerMask raycastMask; // Layer mask for the hitscan raycast
    public float raycastDistance = 10f; // Maximum distance of the hitscan ray
    public float raycastDelay = 1.5f; // Delay between hitscan checks
    public float hitEffectDuration = 0.2f; // Duration of the hit effect

    [Header("Cooldown")]
    public float cooldown = 1.5f;

    public bool isCooldown;
    public bool isShooting;
    public bool hasShot;

    private void Start()
    {

    }

    public void UpdateBehavior()
    {
        if (!isCooldown && !hasShot)
        {
            StartCoroutine(HitscanShoot());
        }
    }

    private IEnumerator HitscanShoot()
    {
        isCooldown = true;

        Vector2 shootDirection = transform.right;

        // Perform hitscan raycast
        RaycastHit2D hit = Physics2D.Raycast(transform.position, shootDirection, raycastDistance, raycastMask);

        // Calculate the hit position
        Vector2 hitPosition = hit ? hit.point : (Vector2)transform.position + shootDirection * raycastDistance;

        // Draw a line to indicate the shot direction and hit position
        Debug.DrawLine(transform.position, hitPosition, Color.red, raycastDelay);

        if (hit)
        {
            // Handle hit effect or damage here

            // Wait for hit effect duration
            yield return new WaitForSeconds(hitEffectDuration);
        }

        hasShot = true;

        yield return new WaitForSeconds(cooldown); // Adjust the delay as needed

        hasShot = false;
        isCooldown = false;
    }

    public void Disable()
    {
        // Clean up any resources or references when the enemy is disabled
    }
}
