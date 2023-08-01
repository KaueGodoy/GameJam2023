using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    public CharacterStats characterStats;

    private Rigidbody2D _rb;
    private BoxCollider2D _boxCollider;
    private PlayerControls _playerInput;

    protected EffectableObject Effectable;

    [SerializeField]
    private InputActionReference playerInputAction;

    #region BaseStats

    [Header("Base Stats")]
    public float baseHealth = 22000f;
    public float baseHealthBonus = 0f;
    public float baseAttack = 10;
    public float baseAttackPercent = 0;
    public float baseAttackFlat = 0;
    public float baseDamageBonus = 0;
    public float baseCritRate = 5;
    public float baseCritDamage = 50;
    public float baseDefense = 15;
    public float baseAttackSpeed = 5;
    public float baseMoveSpeed = 6f;
    public float baseMoveSpeedBonus = 0f;
    public float baseJumpHeight = 5f;
    public float baseJumpHeightBonus = 0;

    #endregion

    private void Awake()
    {
        Instance = this;

        characterStats = new CharacterStats(baseHealth, baseHealthBonus, baseAttack, baseAttackPercent, baseAttackFlat,
                                            baseDamageBonus, baseCritRate, baseCritDamage, baseDefense,
                                            baseAttackSpeed, baseMoveSpeed, baseMoveSpeedBonus, baseJumpHeight, baseJumpHeightBonus);
        Debug.Log("Player init");

        _playerInput = new PlayerControls();

        _rb = GetComponent<Rigidbody2D>();
        _boxCollider = GetComponent<BoxCollider2D>();
        _animator = GetComponent<Animator>();

        Effectable = GetComponent<EffectableObject>();
    }

    private void Start()
    {
        bullet = GetComponent<Bullet>();
        currentHealth = MaxHealth;
        isAlive = true;

        UpdateHealthBar();
        downButton.SetActive(false);
    }

    private void OnEnable()
    {
        _playerInput.Enable();
        playerInputAction.action.Enable();
    }

    private void OnDisable()
    {
        _playerInput.Disable();
        playerInputAction.action.Disable();
    }

    void Update()
    {
        if (PauseMenu.GameIsPaused) return;

        if (isDashing) return;

        if (isAlive)
        {
            ProcessInput();
            Shoot();
        }
    }

    private void FixedUpdate()
    {
        if (PauseMenu.GameIsPaused) return;

        if (isDashing) return;

        if (isAlive)
        {
            Move();
            Flip();
            Jump();
            BetterJump();
            DashTrigger();
            ExitPlatform();
            UpdateHealthBar();
        }

        UpdateTimers();
        UpdateAnimationState();
    }

    void ProcessInput()
    {
        // jump
        if (_playerInput.Player.Jump.triggered)
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
        if (_playerInput.Player.Dash.triggered && canDash)
        {
            dashRequest = true;
        }

        // shooting
        //if (playerInput.Player.Attack.triggered)
        //{
        //    shootRequest = true;
        //}

        // exit platform
        if (_playerInput.Player.Down.triggered)
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
    public float currentHealth;

    public float MaxHealth
    {
        get
        {
            baseHealth = characterStats.GetStat(BaseStat.BaseStatType.HP).GetCalculatedStatValue();
            baseHealthBonus = characterStats.GetStat(BaseStat.BaseStatType.HPBonus).GetCalculatedStatValue() / 100;

            float maxHealth = Effectable.Effect_MaxHealth(baseHealth * (1 + Effectable.Effect_BonusMaxHealth(baseHealthBonus)));

            return Mathf.Max(maxHealth, 0f);
        }
    }

    public float MaxHealthNoIntermediateVariables
    {
        get
        {
            return
                Effectable.Effect_MaxHealth(
                 (baseHealth = characterStats.GetStat(BaseStat.BaseStatType.HP).GetCalculatedStatValue())
                  * (1 + Effectable.Effect_BonusMaxHealth(
                 (characterStats.GetStat(BaseStat.BaseStatType.HPBonus).GetCalculatedStatValue() / 100)))
                );
        }
    }

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

        UpdateHealthBar();
    }

    private void Heal(float healAmount)
    {
        FindObjectOfType<AudioManager>().PlayOneShot("Hit");
        currentHealth += Mathf.FloorToInt(healAmount);

        UpdateHealthBar();
    }

    [Header("HealthBar")]
    [SerializeField] private PlayerHealthBar healthBar;

    public void UpdateHealthBar()
    {
        if (currentHealth > 0.1f && currentHealth >= MaxHealth)
        {
            currentHealth = MaxHealth;
        }

        if (currentHealth <= 0f)
        {
            currentHealth = 0;
            Die();
            Invoke("RestartLevel", deathAnimationTime);
        }

        healthBar.UpdateHealthBar(Mathf.FloorToInt(MaxHealth), Mathf.FloorToInt(currentHealth));

        //UIEventHandler.HealthChanged(currentHealth, MaxHealth);

        // add status > (equip item (that has the stats)) > change stats in UI
        // final max HP = (base)MaxHP + HP% + HP flat
        // cant show the buffs from effects this way

        //characterStats.AddStatBonus(itemToEquip.Stats);
        //UIEventHandler.ItemEquipped(itemToEquip);
        //UIEventHandler.StatsChanged();
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
        _rb.bodyType = RigidbodyType2D.Static;
    }

    #endregion

    #region move

    [Header("Movement")]
    public float moveSpeed;
    public float maxSpeed = 15f;
    public float speedPotion = 0.4f;

    public float CurrentSpeed
    {
        get
        {
            return
                Effectable.Effect_GroundSpeed(
                    baseMoveSpeed * (1 + Effectable.Effect_BonusGroundSpeed(
                    (characterStats.GetStat(BaseStat.BaseStatType.MoveSpeedBonus).GetCalculatedStatValue() / 100)))
                );
        }
    }

    private Vector2 moveH;
    private Vector2 direction;

    private void Move()
    {
        moveH = _playerInput.Player.Move.ReadValue<Vector2>();

        moveSpeed = baseMoveSpeed;

        //Debug.Log(CurrentSpeed);

        direction = new Vector2(moveH.x * CurrentSpeed, _rb.velocity.y);

        if (direction != Vector2.zero)
        {
            _rb.velocity = direction;
        }
        else
        {
            _rb.velocity = Vector3.zero;
        }

    }

    #endregion

    #region jump

    [Header("Jump")]
    public float fallMultiplier = 2.5f;
    public float tapJumpMultiplier = 1.8f;
    public float holdJumpMultiplier = 1f;

    private float jumpForce;

    private bool doubleJump;
    private bool jumpRequest = false;

    public float JumpForce
    {
        get
        {
            baseJumpHeight = characterStats.GetStat(BaseStat.BaseStatType.JumpHeight).GetCalculatedStatValue();
            baseJumpHeightBonus = characterStats.GetStat(BaseStat.BaseStatType.JumpHeightBonus).GetCalculatedStatValue() / 100;

            float jumpForce = Effectable.Effect_JumpHeight(baseJumpHeight * (1 + Effectable.Effect_JumpHeightBonus(baseJumpHeightBonus)));

            return Mathf.Max(jumpForce, 0f);
        }
    }

    void Jump()
    {
        if (jumpRequest)
        {
            FindObjectOfType<AudioManager>().PlayOneShot("Jump");

            Debug.Log(JumpForce);

            _rb.velocity = Vector2.up * JumpForce;
            jumpRequest = false;
        }
    }

    private void BetterJump()
    {
        if (_rb.velocity.y < 0f)
        {
            _rb.gravityScale = fallMultiplier;
        }

        playerInputAction.action.canceled += context =>
        {
            if (_rb != null)
            {
                if (_rb.velocity.y > 0)
                {
                    _rb.gravityScale = tapJumpMultiplier;
                    //Debug.Log("tap jump");
                }
            }
        };

        playerInputAction.action.performed += context =>
        {
            if (_rb != null)
            {
                _rb.gravityScale = holdJumpMultiplier;

                //Debug.Log("hold jump");
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

        float originalGravity = _rb.gravityScale;
        _rb.gravityScale = 0f;

        _rb.velocity = new Vector2(transform.localScale.x * dashSpeed, 0f);
        tr.emitting = true;

        yield return new WaitForSeconds(dashingTime);

        tr.emitting = false;
        _rb.gravityScale = originalGravity;
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
    public Transform spawnPoint;
    public Transform skillSpawnPoint;
    public Transform ultSpawnPoint;
    public GameObject bulletPrefab;
    public Bullet bullet;

    private float shootTimer = 0.0f;
    public float shootDelay = 0.5f;

    private bool shootAnimation = false;
    private bool shootRequest = false;
    private bool isShooting = false;

    public int damageBuff = 2;

    //private int animationHash;
    //public float animationSpeedMultiplier = 1f;
    //private string Player_Shoot = "Player_Shoot";

    void Shoot()
    {
        if (shootRequest)
        {
            shootRequest = false;

            shootAnimation = true;
            //animator.speed = 5f;

            //animationHash = Animator.StringToHash(Player_Shoot);
            //animator.SetFloat(animationHash, animationSpeedMultiplier);
            //Debug.Log("current speed: " + animator.speed);

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

        Physics2D.IgnoreCollision(_boxCollider, platformCollider);
        yield return new WaitForSeconds(collisionDisableTime);
        Physics2D.IgnoreCollision(_boxCollider, platformCollider, false);
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

    private Animator _animator;
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
        else if (_rb.velocity.y > .1f && !IsGrounded())
        {
            ChangeAnimationState(JUMP_ANIMATION);
        }
        // falling
        else if (_rb.velocity.y < .1f && !IsGrounded())
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

        _animator.Play(newAnimation);
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

            spawnPoint.Rotate(firePoint.rotation.x, 180f, firePoint.rotation.z);
            skillSpawnPoint.Rotate(firePoint.rotation.x, 180f, firePoint.rotation.z);
            ultSpawnPoint.Rotate(firePoint.rotation.x, 180f, firePoint.rotation.z);


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

        RaycastHit2D raycastHit = Physics2D.BoxCast(_boxCollider.bounds.center, _boxCollider.bounds.size - new Vector3(0.1f, 0f, 0f), 0f, Vector2.down, extraHeightText, jumpableGround);

        // draw gizmos

        //Color rayColor;

        //if(raycastHit.collider != null)
        //{
        //    rayColor = Color.green;
        //}
        //else
        //{
        //    rayColor = Color.red;
        //}
        //Debug.DrawRay(boxCollider.bounds.center + new Vector3(boxCollider.bounds.extents.x, 0), Vector2.down * (boxCollider.bounds.extents.y + extraHeightText), rayColor);
        //Debug.DrawRay(boxCollider.bounds.center - new Vector3(boxCollider.bounds.extents.x, 0), Vector2.down * (boxCollider.bounds.extents.y + extraHeightText), rayColor);
        //Debug.DrawRay(boxCollider.bounds.center - new Vector3(boxCollider.bounds.extents.x, boxCollider.bounds.extents.y + extraHeightText), Vector2.right * (boxCollider.bounds.extents.x * 2f), rayColor);
        //Debug.Log(raycastHit.collider);

        return raycastHit.collider != null;
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
