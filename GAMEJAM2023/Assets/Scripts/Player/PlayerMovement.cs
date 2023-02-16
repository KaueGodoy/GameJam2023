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
    public float deathExtraDelay = 0.3f;

    private float deathAnimationTime = 0.8f;
    private bool isAlive;
    private int hpPotion = 1;
    private HeartSystem heartSystem;

    private float hitTimer = 0.0f;
    private float hitCooldown = 0.5f;
    private bool isHit = false;

    [Header("Movement")]
    public float moveSpeed = 5f;
    public float maxSpeed = 15f;
    public float speedPotion = 0.4f;
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

    private float shootTimer = 0.0f;
    public float shootDelay = 0.5f;

    private bool shootAnimation = false;
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
    const string DEATH_ANIMATION = "Player_Death";



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
        heartSystem = GetComponent<HeartSystem>();
        currentHealth = maxHealth;
        isAlive = true;

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

        if (isAlive)
        {
            ProcessInput();
            Shoot();
        }

    }

    private void FixedUpdate()
    {
        if (isDashing) return;
        if (isAlive)
        {
            Move();
            Flip();
            Jump();
            DashTrigger();
        }

        UpdateTimers();
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
        else if(moveX > 0 || moveX < 0)
        {
            ChangeAnimationState(RUN_ANIMATION);
        }
        // idle
        else
        {
            ChangeAnimationState(IDLE_ANIMATION);
        }

        /*
        if (isAlive)
        {
            if (IsGrounded() && !shootRequest)
            {
                if (moveX > 0 || moveX < 0)
                {
                    ChangeAnimationState(RUN_ANIMATION);
                    //audioManager.Play("Walk");
                    //FindObjectOfType<AudioManager>().PlayOneShot("Walk");

                }
                else
                    ChangeAnimationState(IDLE_ANIMATION);

            }

            if (rb.velocity.y > .1f && !IsGrounded())
            {
                ChangeAnimationState(JUMP_ANIMATION);
            }
            /*
            if(knockbackFromRight || !knockbackFromRight)
            {
                ChangeAnimationState(HIT_ANIMATION);
            }
        }
        else if (!isAlive)
        {
            ChangeAnimationState(DEATH_ANIMATION);
        }
        */


    }

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
            FindObjectOfType<AudioManager>().PlayOneShot("Jump");
            rb.velocity = Vector2.up * jumpForce;
            jumpRequest = false;
        }
    }
    void Shoot()
    {
        /*if (shootRequest)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            shootAnimation = true;
            shootRequest = false;
        }*/

    
        if (shootRequest)
        {
            shootRequest = false;
            shootAnimation = true;

            if (!isShooting)
            {
                isShooting = true;

                //ChangeAnimationState(SHOOT_ANIMATION);
                FindObjectOfType<AudioManager>().PlayOneShot("Shoot");

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
}
