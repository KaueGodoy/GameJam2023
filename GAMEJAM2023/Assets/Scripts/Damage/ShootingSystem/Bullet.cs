using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] public Rigidbody2D rb;

    [Header("Speed")]
    public float bulletSpeed = 10f;

    [Header("Damage")]
    public float bulletDamage = 5f;
    public float critRate = 30f;
    public float critDamage = 2f;

    [Header("Distance")]
    public float timeToDestroy = 3f;

    public bool isCritical;

    // animation
    PlayerMovement player;
    private const string SHOT_ANIMATION = "Player_Shot";

    void Start()
    {

        rb.velocity = transform.right * bulletSpeed;
        //player.ChangeAnimationState(SHOT_ANIMATION);

    }

    private void Update()
    {
        Destroy(this.gameObject, timeToDestroy);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {

            isCritical = UnityEngine.Random.Range(0, 100) < critRate;

            if (isCritical)
            {
                bulletDamage *= critDamage;
            }

            enemy.TakeDamage(bulletDamage);
            FindObjectOfType<AudioManager>().PlayOneShot("Hitmarker");
            DamagePopup.Create(transform.position, (int)bulletDamage, isCritical);
        }

        EnemyNoAggro enemyNoAggro = collision.GetComponent<EnemyNoAggro>();
        if(enemyNoAggro != null)
        {
            enemyNoAggro.TakeDamage(bulletDamage);
            DamagePopup.Create(transform.position, (int)bulletDamage, isCritical);
        }


        Destroy(gameObject);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

}
