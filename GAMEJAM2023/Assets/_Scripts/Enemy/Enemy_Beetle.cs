using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Beetle : MonoBehaviour, IEnemy
{
    [Header("Health")]
    public float currentHealth;
    public float maxHealth = 200;
    private readonly float healthThreshold = 0.0f;

    private Rigidbody2D _rb;
    private BoxCollider2D _boxCollider;

    HealthSystem healthSystem;
    Transform healthBarTransform;

    public float baseSpeed = 3f;
    public float baseSpeedBonus = 0f;
    protected EffectableObject Effectable;

    public float CurrentSpeed
    {
        get
        {
            float currentSpeed = Effectable.Effect_GroundSpeed(baseSpeed * (1 + Effectable.Effect_BonusGroundSpeed(baseSpeedBonus)));

            return Mathf.Max(currentSpeed, 0f);
        }
    }


    public float CurrentHealth { get; set; }
    public float MaxHealth { get; set; }
    public int ID { get; set; }
    public bool IsDead { get; set; }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _boxCollider = GetComponent<BoxCollider2D>();

        Effectable = GetComponent<EffectableObject>();

        pfDeathEffect = Resources.Load<GameObject>("Prefabs/pfDeathAnimationEffect");
    }

    private void Start()
    {
        currentHealth = maxHealth;

        ID = 3;

        DropTable = new DropTable();
        DropTable.loot = new List<LootDrop>
        {
            new LootDrop("coin", 100),
        };
    }

    private void Update()
    {
        UpdateTimers();
    }

    [Header("Timers")]
    private float hitCooldown = 0.3f;
    [HideInInspector] public bool isHit = false;

    private float hitTimer = 0.0f;

    private void UpdateTimers()
    {
        // hit
        if (isHit)
        {
            hitTimer += Time.deltaTime;
        }

        if (hitTimer > hitCooldown)
        {
            isHit = false;
            hitTimer = 0f;
        }
    }

    public void PerformAttack()
    {

    }

    public void TakeDamage(float damage)
    {
        if (currentHealth == maxHealth)
        {
            InstantiateHealthBar();
        }

        isHit = true;

        currentHealth -= damage;
        healthSystem.Damage(damage);

        //Debug.Log("Health: " + healthSystem.GetHealthPercent());
        //Debug.Log("Health: " + healthSystem.GetCurrentHealth());

        if (currentHealth <= 0)
        {
            DisableHealthBar();

            _rb.bodyType = RigidbodyType2D.Static;
            _boxCollider.enabled = false;

            Invoke("Die", deathAnimationTime);
            Invoke("DeathEffect", deathAnimationTime);
        }
    }

    [Header("Death")]
    [SerializeField] private float deathAnimationTime = 0.55f;
    private GameObject pfDeathEffect;

    private void DeathEffect()
    {
        if (pfDeathEffect != null)
        {
            this.gameObject.SetActive(false);
            GameObject effect = Instantiate(pfDeathEffect, transform.position, Quaternion.identity);
            Destroy(effect, 0.3f);
        }
    }

    public void Die()
    {
        DropLoot();
        CombatEvents.EnemyDied(this);
        Destroy(gameObject);
    }

    [Header("HP Bar")]
    public Transform pfHealthBar;
    public Vector3 offset = new Vector3(0, 1f);

    private void InstantiateHealthBar()
    {
        healthSystem = new HealthSystem(maxHealth);

        healthBarTransform = Instantiate(pfHealthBar, transform.position + offset, Quaternion.identity, transform);
        //healthBarTransform.gameObject.SetActive(false);

        HealthBar healthBar = healthBarTransform.GetComponent<HealthBar>();
        healthBar.Setup(healthSystem);

        Debug.Log("Health: " + healthSystem.GetHealthPercent());
        Debug.Log("Health: " + healthSystem.GetCurrentHealth());
    }

    private void DisableHealthBar()
    {
        healthSystem.Die();
        healthBarTransform.gameObject.SetActive(false);
        //Destroy(healthBarTransform.gameObject);
    }

    [Header("Drop")]
    public PickupItem pickupItem;
    public DropTable DropTable { get; set; }


    void DropLoot()
    {
        Item item = DropTable.GetDrop();
        if (item != null)
        {
            PickupItem instance = Instantiate(pickupItem, transform.position, Quaternion.identity);
            instance.ItemDrop = item;
        }
    }

    public bool IsAlive()
    {
        return currentHealth > healthThreshold;
    }

    public bool isDead()
    {
        return currentHealth <= healthThreshold;
    }

    public bool IsDamaged()
    {
        return currentHealth < maxHealth;
    }

    public bool isFullHealth()
    {
        return currentHealth == maxHealth;
    }
}
