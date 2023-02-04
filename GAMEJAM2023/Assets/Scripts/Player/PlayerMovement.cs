using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance { get; private set; }

    // components
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;

    [Header("Health")]
    public float currentHealth = 0f;
    public float maxHealth = 3f;

    [Header("Movement")]
    public float moveSpeed = 5f;
    private float moveX = 0f;

    [Header("Jump")]
    [Range(0f, 10f)]
    public float jumpForce = 4f;
    private bool doubleJump;
    private bool jumpRequest = false;

    [Header("Dash")]
    public float dashSpeed = 5f;
    public float dashingTime = 0.2f;
    public float dashingCooldown = 1f;

    private bool canDash = true;
    private bool isDashing;
    private bool dashRequest = false;

    [SerializeField] TrailRenderer tr;

    [Header("Shooting")]
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float shootDelay = 0.3f;
    private bool shootRequest = false;
    private bool isShooting = false;

    [Header("Knockback")]
    public float knockbackForce = 2f;
    public float knockbackCounter = 0f;
    public float knockbackTotalTime = 1f;
    public bool knockbackFromRight;

    [Header("Inventory")]
    [SerializeField] private UI_Inventory uiInventory;
    private Inventory inventory;

    [Header("Ground")]
    [SerializeField] private LayerMask jumpableGround;

    // facing direction
    private bool isFacingRight = true;

    // pause
    private bool gameIsPaused = false;

    // animation
    private Animator animator;
    private string currentAnimation;

    const string RUN_ANIMATION = "Player_Run";
    const string IDLE_ANIMATION = "Player_Idle";
    const string SHOOT_ANIMATION = "Player_Shoot";
    const string JUMP_ANIMATION = "Player_Jump";
    const string HIT_ANIMATION = "Player_Hit";


    private void Awake()
    {
        Instance = this;
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();

    }


    private void Start()
    {
        inventory = new Inventory(UseItem);
        uiInventory.SetPlayer(this);
        uiInventory.SetInventory(inventory);
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;

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
                Debug.Log("Healed");
                inventory.RemoveItem(new Item { itemType = Item.ItemType.HealthPotion, amount = 1 });
                break;
            case Item.ItemType.SpeedPotion:
                // speed boost
                moveSpeed *= 2;
                Debug.Log("Speed boost");
                inventory.RemoveItem(new Item { itemType = Item.ItemType.SpeedPotion, amount = 1 });
                break;
            case Item.ItemType.Coin:
                // damage buff
                //

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
        if (isDashing) return;

        ProcessInput();


    }

    private void FixedUpdate()
    {
        if (isDashing) return;

        Move();
        Flip();
        Jump();
        DashTrigger();
        Shoot();
        UpdateAnimationState();

    }

    void ProcessInput()
    {
        // horizontal movement
        moveX = Input.GetAxisRaw("Horizontal");



        // jump
        if (Input.GetButtonDown("Jump") && !gameIsPaused)
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
        if (Input.GetButtonDown("Dash") && canDash && !gameIsPaused)
        {
            dashRequest = true;
        }

        // shooting
        if (Input.GetButtonDown("Fire1") && !gameIsPaused)
        {
            shootRequest = true;

        }

        // pause
        if (PauseMenu.GameIsPaused)
        {
            gameIsPaused = true;
        }
        else
            gameIsPaused = false;
    }

    public void ChangeAnimationState(string newAnimation)
    {
        if (currentAnimation == newAnimation) return;

        animator.Play(newAnimation);
        currentAnimation = newAnimation;

    }

    private void UpdateAnimationState()
    {

        if (IsGrounded() && !shootRequest)
        {
            if (moveX > 0 || moveX < 0)
            {
                ChangeAnimationState(RUN_ANIMATION);
            }
            else
            {
                ChangeAnimationState(IDLE_ANIMATION);
            }
        }

        if (rb.velocity.y > .1f && !IsGrounded())
        {
            ChangeAnimationState(JUMP_ANIMATION);
        }

        /*if(knockbackFromRight || !knockbackFromRight)
        {
            ChangeAnimationState(HIT_ANIMATION);
        }*/


    }

    public void PlayerTakeDamage(float damageAmount)
    {
        currentHealth -= Mathf.FloorToInt(damageAmount);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        SceneManager.LoadScene(1);
    }

    void Move()
    {
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

    }

    void Jump()
    {
        if (jumpRequest)
        {

            rb.velocity = Vector2.up * jumpForce;
            jumpRequest = false;
        }
    }
    void Shoot()
    {
        /*if (shootRequest)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            shootRequest = false;
        }*/


        if (shootRequest)
        {
            shootRequest = false;

            if (!isShooting)
            {
                isShooting = true;

                ChangeAnimationState(SHOOT_ANIMATION);
                Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

                Invoke("ShootComplete", shootDelay);
            }
        }
    }


    void ShootComplete()
    {
        isShooting = false;
    }

    void DashTrigger()
    {
        if (dashRequest)
        {
            StartCoroutine(Dash());
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

    private void Flip()
    {
        if (isFacingRight && moveX < 0f || !isFacingRight && moveX > 0f)
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

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(boxCollider.bounds.min, boxCollider.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }
}
