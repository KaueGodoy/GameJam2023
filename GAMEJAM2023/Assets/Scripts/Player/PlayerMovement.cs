using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance { get; private set; }

    // components
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;

    [SerializeField]
    private InputActionReference playerInputAction;

    [Header("Inventory")]
    [SerializeField] private UI_Inventory uiInventory;
    private Inventory inventory;


    private PlayerInput playerInput;


    private void Awake()
    {
        Instance = this;
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        bullet = GetComponent<Bullet>();

        playerInput = new PlayerInput();


    }


    private void Start()
    {
        inventory = new Inventory(UseItem);
        uiInventory.SetPlayer(this);
        uiInventory.SetInventory(inventory);
        animator = GetComponent<Animator>();
        heartSystem = GetComponent<HeartSystem>();
        currentHealth = maxHealth;
        isAlive = true;

    }

    private void OnEnable()
    {
        playerInput.Enable();
        playerInputAction.action.Enable();

    }

    private void OnDisable()
    {
        playerInput.Disable();
        playerInputAction.action.Disable();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ItemWorld itemWorld = collision.GetComponent<ItemWorld>();
        if (itemWorld != null)
        {
            inventory.AddItem(itemWorld.GetItem());
            itemWorld.DestroySelf();

        }
    }

    private void UseItem(Item item)
    {
        switch (item.itemType)
        {
            case Item.ItemType.HealthPotion:
                //heal hp
                HealPlayer(hpPotion);
                Debug.Log("Healed");
                inventory.RemoveItem(new Item { itemType = Item.ItemType.HealthPotion, amount = 1 });
                break;
            case Item.ItemType.SpeedPotion:
                // speed boost
                SpeedBoost(speedPotion);
                Debug.Log("Speed boost");
                inventory.RemoveItem(new Item { itemType = Item.ItemType.SpeedPotion, amount = 1 });
                break;
            case Item.ItemType.Coin:
                // damage buff
                //DamageBuff(damageBuff);
                //bullet.bulletDamage *= 2;

                Debug.Log("Money spent");
                inventory.RemoveItem(new Item { itemType = Item.ItemType.Coin, amount = 1 });
                break;
            case Item.ItemType.Key:
                // speed boost
                Debug.Log("Used key");
                inventory.RemoveItem(item);
                break;

        }
    }

    void Update()
    {
        if (!PauseMenu.GameIsPaused)
        {
            if (isDashing) return;

            if (isAlive)
            {
                ProcessInput();
                Shoot();
            }
        }
    }

    private void FixedUpdate()
    {
        if (!PauseMenu.GameIsPaused)
        {
            if (isDashing) return;

            if (isAlive)
            {
                Move();
                Flip();
                Jump();
                BetterJump();
                DashTrigger();
            }

            UpdateTimers();
            UpdateAnimationState();
        }
    }

    void ProcessInput()
    {
        // jump
        if (playerInput.Player.Jump.triggered)
        {
            if (IsGrounded())
            {
                jumpRequest = true;
                doubleJump = true;
            }
            else if (doubleJump)
            {
                jumpRequest = true;
                doubleJump = false;
            }

        }

        // dash
        if (playerInput.Player.Dash.triggered && canDash)
        {
            dashRequest = true;
        }

        // shooting
        if (playerInput.Player.Attack.triggered)
        {
            shootRequest = true;
        }
    }



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

        // shooting
        if (shootAnimation)
        {
            shootTimer += Time.deltaTime;

        }
        if (shootTimer > shootDelay)
        {
            shootAnimation = false;
            shootTimer = 0f;
        }
    }


    public void FootStep()
    {
        FindObjectOfType<AudioManager>().PlayOneShot("Walk");

    }

    #region health

    [Header("Health")]
    public float currentHealth = 0f;
    public float maxHealth = 3f;
    public float deathExtraDelay = 0.3f;

    private float deathAnimationTime = 0.8f;
    private bool isAlive;
    private int hpPotion = 1;
    private HeartSystem heartSystem;

    private float hitTimer = 0.0f;
    private float hitCooldown = 0.5f;
    private bool isHit = false;

    public void PlayerTakeDamage(float damageAmount)
    {
        FindObjectOfType<AudioManager>().PlayOneShot("Hit");
        currentHealth -= Mathf.FloorToInt(damageAmount);
        isHit = true;

        if (currentHealth <= 0)
        {
            Die();

            Invoke("RestartLevel", deathAnimationTime);
        }
    }

    #endregion

    #region level

    void RestartLevel()
    {
        isAlive = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Die()
    {
        FindObjectOfType<AudioManager>().Play("Hit");
        isAlive = false;
        rb.bodyType = RigidbodyType2D.Static;
    }

    #endregion


    #region buff

    public void DamageBuff(int damageBuff)
    {
        float newDamage;
        newDamage = bullet.bulletDamage + damageBuff;

        //bullet.GetComponent<Bullet>().bulletDamage = newDamage;

    }

    public void HealPlayer(int healAmount)
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += healAmount;
            heartSystem.curLife += healAmount;
            FindObjectOfType<AudioManager>().PlayOneShot("Heal");

        }
    }

    public void SpeedBoost(float speedAmount)
    {
        if (moveSpeed < maxSpeed)
        {
            moveSpeed += speedAmount;
            FindObjectOfType<AudioManager>().PlayOneShot("SpeedBoost");

        }
    }

    #endregion

    void MoveOld()
    {/*
        if (knockbackCounter <= 0)
        {
            rb.velocity = new Vector2(moveX * moveSpeed, rb.velocity.y);
        }
        else
        {
            if (knockbackFromRight)
            {
                rb.velocity = new Vector2(-knockbackForce, knockbackForce);
            }
            if (!knockbackFromRight)
            {
                rb.velocity = new Vector2(knockbackForce, knockbackForce);
            }

            knockbackCounter -= Time.deltaTime;
        }
        */
    }

    #region move

    [Header("Movement")]
    public float moveSpeed = 5f;
    public float maxSpeed = 15f;
    public float speedPotion = 0.4f;

    private Vector2 moveH;
    private Vector2 direction;

    private void Move()
    {
        moveH = playerInput.Player.Move.ReadValue<Vector2>();

        direction = new Vector2(moveH.x * moveSpeed, rb.velocity.y);

        if (direction != Vector2.zero)
        {
            rb.velocity = direction;
        }
        else
        {
            rb.velocity = Vector3.zero;
        }

    }

    #endregion

    #region jump

    [Header("Jump")]
    [Range(0f, 10f)]
    public float jumpForce = 4f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 1f;

    private bool doubleJump;
    private bool jumpRequest = false;

    void Jump()
    {
        if (jumpRequest)
        {
            FindObjectOfType<AudioManager>().PlayOneShot("Jump");

            rb.velocity = Vector2.up * jumpForce;
            jumpRequest = false;
        }
    }

    private void BetterJump()
    {/*
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !playerInput.Player.Jump.triggered)
        {
            rb.velocity += Vector2.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }*/

        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }

        playerInputAction.action.started += context =>
        {
            if (rb.velocity.y > 0 && context.interaction is HoldInteraction)
            {
                rb.gravityScale = lowJumpMultiplier;
            }
            else if (context.interaction is PressInteraction)
            {
                rb.gravityScale = 1f;
            }
        };

    }

    #endregion

    #region dash

    [Header("Dash")]
    public float dashSpeed = 5f;
    public float dashingTime = 0.2f;
    public float dashingCooldown = 1f;

    private bool canDash = true;
    private bool isDashing;
    private bool dashRequest = false;

    [SerializeField] TrailRenderer tr;

    void DashTrigger()
    {
        if (dashRequest)
        {
            StartCoroutine(Dash());
            FindObjectOfType<AudioManager>().PlayOneShot("Dash");
            dashRequest = false;
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;

        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        rb.velocity = new Vector2(transform.localScale.x * dashSpeed, 0f);
        tr.emitting = true;

        yield return new WaitForSeconds(dashingTime);

        tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;

        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;

    }

    #endregion

    #region knockbak

    [Header("Knockback")]
    public float knockbackForce = 2f;
    public float knockbackCounter = 0f;
    public float knockbackTotalTime = 1f;
    public bool knockbackFromRight;

    #endregion

    #region shoot

    [Header("Shooting")]
    public Transform firePoint;
    public GameObject bulletPrefab;
    public Bullet bullet;

    private float shootTimer = 0.0f;
    public float shootDelay = 0.5f;

    private bool shootAnimation = false;
    private bool shootRequest = false;
    private bool isShooting = false;

    public int damageBuff = 2;

    void Shoot()
    {
        if (shootRequest)
        {
            shootRequest = false;
            shootAnimation = true;

            if (!isShooting)
            {
                isShooting = true;

                Invoke("InstantiateBullet", shootDelay - 0.1f);
                Invoke("ShootComplete", shootDelay);
            }
        }
    }

    private void InstantiateBullet()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        FindObjectOfType<AudioManager>().PlayOneShot("Shoot");
    }

    void ShootComplete()
    {
        isShooting = false;
    }

    #endregion

    #region animation

    private Animator animator;
    private string currentAnimation;

    const string RUN_ANIMATION = "Player_Run";
    const string IDLE_ANIMATION = "Player_Idle";
    const string SHOOT_ANIMATION = "Player_Shoot";
    const string JUMP_ANIMATION = "Player_Jump";
    const string HIT_ANIMATION = "Player_Hit";
    const string DEATH_ANIMATION = "Player_Death";

    private void UpdateAnimationState()
    {
        // dead
        if (!isAlive)
        {
            ChangeAnimationState(DEATH_ANIMATION);
        }
        // hit
        else if (isHit)
        {
            ChangeAnimationState(HIT_ANIMATION);
        }
        // shooting
        else if (shootAnimation)
        {
            ChangeAnimationState(SHOOT_ANIMATION);
        }
        // jump
        else if (rb.velocity.y > .1f && !IsGrounded())
        {
            ChangeAnimationState(JUMP_ANIMATION);
        }
        // falling
        else if (rb.velocity.y < .1f && !IsGrounded())
        {
            ChangeAnimationState(JUMP_ANIMATION);
        }
        // move
        else if (moveH.x > 0 || moveH.x < 0)
        {
            ChangeAnimationState(RUN_ANIMATION);
        }
        // idle
        else
        {
            ChangeAnimationState(IDLE_ANIMATION);
        }
    }

    public void ChangeAnimationState(string newAnimation)
    {
        if (currentAnimation == newAnimation) return;

        animator.Play(newAnimation);
        currentAnimation = newAnimation;
    }

    #endregion 

    #region flip

    private bool isFacingRight = true;

    private void Flip()
    {
        if (isFacingRight && moveH.x < 0f || !isFacingRight && moveH.x > 0f)
        {
            // flipping the player using scale
            Vector3 localScale = transform.localScale;
            isFacingRight = !isFacingRight;
            localScale.x *= -1f;
            transform.localScale = localScale;
            firePoint.Rotate(firePoint.rotation.x, 180f, firePoint.rotation.z);

            // flippping using eulerAngles
            //firePoint.eulerAngles = new Vector3(0f, 180f, 0f);

            // flipping the player using rotation
            //transform.Rotate(0, 180f, 0);

            //flipping the sprite
            //spriteRenderer.flipX = true;
        }
    }

    #endregion

    #region ground

    [Header("Ground")]
    [SerializeField] private LayerMask jumpableGround;

    private bool IsGrounded()
    {
        //return Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size - new Vector3(0.1f, 0f, 0f), 0f, Vector2.down, .1f, jumpableGround);

        float extraHeightText = .1f;

        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size - new Vector3(0.1f, 0f, 0f), 0f, Vector2.down, extraHeightText, jumpableGround);

        // draw gizmos
        /*
        Color rayColor;
        
        if(raycastHit.collider != null)
        {
            rayColor = Color.green;
        }
        else
        {
            rayColor = Color.red;
        }
        Debug.DrawRay(boxCollider.bounds.center + new Vector3(boxCollider.bounds.extents.x, 0), Vector2.down * (boxCollider.bounds.extents.y + extraHeightText), rayColor);
        Debug.DrawRay(boxCollider.bounds.center - new Vector3(boxCollider.bounds.extents.x, 0), Vector2.down * (boxCollider.bounds.extents.y + extraHeightText), rayColor);
        Debug.DrawRay(boxCollider.bounds.center - new Vector3(boxCollider.bounds.extents.x, boxCollider.bounds.extents.y + extraHeightText), Vector2.right * (boxCollider.bounds.extents.x * 2f), rayColor);
        Debug.Log(raycastHit.collider);
        */

        return raycastHit.collider != null;

    }

    #endregion

    public Vector3 GetPosition()
    {
        return transform.position;
    }

}
