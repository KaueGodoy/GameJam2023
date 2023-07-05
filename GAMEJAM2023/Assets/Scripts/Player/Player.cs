using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    public CharacterStats characterStats;

    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    private PlayerControls playerInput;

    [SerializeField]
    private InputActionReference playerInputAction;

    #region BaseStats

    [Header("Base Stats")]
    public float baseHealth = 22000f;
    public float baseAttack = 10;
    public float baseAttackPercent = 0;
    public float baseAttackFlat = 0;
    public float baseDamageBonus = 0;
    public float baseCritRate = 5;
    public float baseCritDamage = 50;
    public float baseDefense = 15;
    public float baseAttackSpeed = 5;

    #endregion

    private void Awake()
    {
        Instance = this;

        characterStats = new CharacterStats(baseHealth, baseAttack, baseAttackPercent, baseAttackFlat, baseDamageBonus, baseCritRate, baseCritDamage, baseDefense, baseAttackSpeed);
        Debug.Log("Player init");

        playerInput = new PlayerControls();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        bullet = GetComponent<Bullet>();

        animator = GetComponent<Animator>();


        maxHealth = baseHealth;
        currentHealth = maxHealth;
        //currentHealth = baseHealth;
        isAlive = true;

        UpdateUI();
        //UIEventHandler.HealthChanged(this.currentHealth, this.maxHealth);

        downButton.SetActive(false);
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
                ExitPlatform();
                UpdateUI();
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

        // exit platform
        if (playerInput.Player.Down.triggered)
        {
            exitPlatformTrigger = true;
        }

        // damage test DELETE
        if (Input.GetKeyDown(KeyCode.U))
        {
            TakeDamage(damageAmount);
        }

        // heal test DELETE
        if (Input.GetKeyDown(KeyCode.I))
        {
            Heal(healAmount);
        }

    }

    #region timers

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

    #endregion

    #region health

    [Header("Health")]
    public float currentHealth = 0;
    public float maxHealth = 3;

    [Header("Damage and Heal")]
    public float damageAmount = 1f;
    public float healAmount = 1f;

    public float deathExtraDelay = 0.3f;

    private float deathAnimationTime = 0.8f;
    private bool isAlive;

    private float hitTimer = 0.0f;
    private float hitCooldown = 0.5f;
    private bool isHit = false;

    public void TakeDamage(float damageAmount)
    {
        FindObjectOfType<AudioManager>().PlayOneShot("Hit");
        currentHealth -= Mathf.FloorToInt(damageAmount);
        isHit = true;

        //UIEventHandler.HealthChanged(this.currentHealth, this.maxHealth);

        if (currentHealth <= 0)
        {
            UpdateUI();
            Die();

            Invoke("RestartLevel", deathAnimationTime);
        }
    }

    private void Heal(float healAmount)
    {
        FindObjectOfType<AudioManager>().PlayOneShot("Hit");
        currentHealth += Mathf.FloorToInt(healAmount);

        if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
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
    }

    public void HealPlayer(int healAmount)
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += healAmount;
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

    #endregion

    #region jump

    [Header("Jump")]
    [Range(0f, 10f)]
    public float jumpForce = 4f;
    public float fallMultiplier = 2.5f;
    public float tapJumpMultiplier = 1.8f;
    public float holdJumpMultiplier = 1f;


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
    {
        if (rb.velocity.y < 0f)
        {
            rb.gravityScale = fallMultiplier;
        }

        playerInputAction.action.canceled += context =>
        {
            if (rb != null)
            {
                if (rb.velocity.y > 0)
                {
                    rb.gravityScale = tapJumpMultiplier;
                    Debug.Log("tap jump");
                }
            }
        };

        playerInputAction.action.performed += context =>
        {
            if (rb != null)
            {
                rb.gravityScale = holdJumpMultiplier;

                Debug.Log("hold jump");
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

    #region knockback

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

    #region oneWayPlatform

    [Header("One Way Platform")]
    [SerializeField] private float collisionDisableTime = 0.5f;
    [SerializeField] private GameObject downButton;

    private GameObject currentOneWayPlatform;
    private bool exitPlatformTrigger = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWayPlatform = collision.gameObject;
            downButton.SetActive(true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWayPlatform = null;
            downButton.SetActive(false);
        }
    }

    private IEnumerator DisableCollision()
    {
        BoxCollider2D platformCollider = currentOneWayPlatform.GetComponent<BoxCollider2D>();

        Physics2D.IgnoreCollision(boxCollider, platformCollider);
        yield return new WaitForSeconds(collisionDisableTime);
        Physics2D.IgnoreCollision(boxCollider, platformCollider, false);
    }

    private void ExitPlatform()
    {
        if (exitPlatformTrigger)
        {
            exitPlatformTrigger = false;

            if (currentOneWayPlatform != null)
            {
                StartCoroutine(DisableCollision());
            }
        }
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

    #region UI

    [Header("UI")]
    [SerializeField] private PlayerHealthBar healthBar;

    public void UpdateUI()
    {
        healthBar.UpdateHealthBar(maxHealth, currentHealth);
    }

    #endregion

    public void FootStep()
    {
        FindObjectOfType<AudioManager>().PlayOneShot("Walk");
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

}
