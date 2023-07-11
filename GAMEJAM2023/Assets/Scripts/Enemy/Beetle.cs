using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Beetle : MonoBehaviour, IEnemy
{
    public float CurrentHealth { get; set; }
    public float MaxHealth { get; set; }
    public int ID { get; set; }
    public bool IsDead { get; set; }

    [Header("Health")]
    public float currentHealth;
    public float maxHealth = 200;
    //private readonly float healthThreshold = 0.0f;

    private HealthSystem healthSystem;
    private Transform healthBarTransform;

    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;

    private GameObject pfDeathEffect;


    public virtual void Start()
    {
        Setup();

        SetHealth();
        SetID(0);
        CreateDrop();


        //ID = 0; // first enemy (could use as a stat on character stats)



    }

    // things that will never change

    public void Setup()
    {
        GetComponents();
        LoadResources();
    }


    public virtual void GetComponents()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    public virtual void SetHealth()
    {
        currentHealth = maxHealth;
    }

    public virtual void SetID(int id)
    {
        this.ID = id;
    }

    public virtual void CreateDrop()
    {
        DropTable = new DropTable();
        DropTable.loot = new List<LootDrop>
        {
            new LootDrop("coin", 100),
        };
    }

    public virtual void LoadResources()
    {
        pfDeathEffect = Resources.Load<GameObject>("Prefabs/pfDeathAnimationEffect");
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

            rb.bodyType = RigidbodyType2D.Static;
            boxCollider.enabled = false;

            Invoke("Die", deathAnimationTime);
            Invoke("DeathEffect", deathAnimationTime);
        }
    }

    [Header("Death")]
    [SerializeField] private float deathAnimationTime = 0.55f;

    private void DeathEffect()
    {
        if (pfDeathEffect != null)
        {
            this.gameObject.SetActive(false);
            GameObject pfDeathEffectInstance = Instantiate(pfDeathEffect, transform.position, Quaternion.identity);

            //GameObject effect = Instantiate(pfDeathEffect, transform.position, Quaternion.identity);
            Destroy(pfDeathEffectInstance, 0.2f);
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


}
