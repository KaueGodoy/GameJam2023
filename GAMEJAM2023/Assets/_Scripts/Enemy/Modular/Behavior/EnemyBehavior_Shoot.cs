using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior_Shoot : MonoBehaviour, IEnemyBehavior
{
    [Header("Projectile")]
    public GameObject projectilePrefab; // Reference to the projectile prefab
    public Vector2 projectileOffset; // Offset from the enemy's position for projectile instantiation

    [Header("Cooldown")]
    public float cooldown = 1.5f;

    public bool isCooldown;
    public bool hasShot;

    public void UpdateBehavior()
    {
        if (!isCooldown && !hasShot)
        {
            StartCoroutine(Shoot());
        }
    }

    private IEnumerator Shoot()
    {
        isCooldown = true;

        Vector3 spawnPosition = GetSpawnPosition(); // Calculate the spawn position

        GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);

        hasShot = true;

        yield return new WaitForSeconds(cooldown); // Adjust the delay as needed

        hasShot = false;
        isCooldown = false;
    }

    private Vector3 GetSpawnPosition()
    {
        Vector3 offset = new Vector3(projectileOffset.x, projectileOffset.y, 0f);
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        // If the enemy is flipped horizontally, reverse the offset
        if (spriteRenderer.flipX)
            offset.x *= -1f;

        return transform.position + offset;
    }

    public void Disable()
    {
        // Clean up any resources or references when the enemy is disabled
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(GetSpawnPosition(), 0.3f
            );
    }
}

