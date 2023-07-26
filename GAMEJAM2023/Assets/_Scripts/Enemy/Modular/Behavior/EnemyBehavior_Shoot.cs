using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior_Shoot : MonoBehaviour, IEnemyBehavior
{
    private SpriteRenderer _spriteRenderer;

    [Header("Projectile")]
    public GameObject projectilePrefab; // Reference to the projectile prefab
    public Vector3 projectileOffset; // Offset from the enemy's position for projectile instantiation

    [Header("Cooldown")]
    public float cooldown = 1.5f;

    public bool isCooldown;
    public bool hasShot;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

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

        _spriteRenderer.color = Color.red;

        Vector3 spawnPosition = GetSpawnPosition(); // Calculate the spawn position

        var projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);

        hasShot = true;

        yield return new WaitForSeconds(cooldown); // Adjust the delay as needed
        
        _spriteRenderer.color = Color.white;

        isCooldown = false;
        hasShot = false;
    }

    private Vector3 GetSpawnPosition()
    {
        Vector3 offset = new Vector3(projectileOffset.x, projectileOffset.y, 0f);

        if (_spriteRenderer.flipX) offset.x *= -1f;

        return transform.position + offset;
    }

    public void Disable()
    {

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + projectileOffset, 0.3f);
    }
}

