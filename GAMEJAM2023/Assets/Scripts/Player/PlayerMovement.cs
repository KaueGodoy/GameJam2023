using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // components
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;

    [Header("Ground")]
    [SerializeField] private LayerMask jumpableGround;

    [Header("Movement")]
    [Range(0f, 10f)]
    public float moveSpeed = 5f;
    private float moveX = 0f;

    [Header("Jump")]
    [Range(0f, 10f)]
    public float jumpForce = 4f;
    private bool jumpRequest = false;

    [Header("Dash")]
    public float dashSpeed = 5f;
    public float dashingTime = 0.2f;
    public float dashingCooldown = 1f;

    private bool canDash = true;
    private bool isDashing;
    private bool dashRequest = false;

    [SerializeField] TrailRenderer tr;

    //facing direction
    private bool isFacingRight = true;

    [Header("Shooting")]
    public Transform firePoint;
    public GameObject bulletPrefab;
    private bool shootRequest = false;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();

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
    }

    void ProcessInput()
    {
        // horizontal movement
        moveX = Input.GetAxisRaw("Horizontal");

        // jump
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            jumpRequest = true;
        }

        // dash
        if (Input.GetButtonDown("Dash") && canDash)
        {
            dashRequest = true;
        }

        // shooting
        if (Input.GetButtonDown("Fire1"))
        {
           shootRequest = true;
           
        }
    }

    void Move()
    {
        rb.velocity = new Vector2(moveX * moveSpeed, rb.velocity.y);
    }

    void Jump()
    {
        if (jumpRequest)
        {
            rb.velocity = Vector2.up * jumpForce;
            jumpRequest = false;
        }
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

    void Shoot()
    {
        if (shootRequest)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            shootRequest = false;
        }
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

            firePoint.Rotate(0f, 180f, 0f);

            if (Input.GetKey(KeyCode.W))
            {
                firePoint.Rotate(firePoint.rotation.x, firePoint.rotation.y, 90f);
            }

            // flipping the player using rotation
            //transform.Rotate(0, 180f, 0);

            //flipping the sprite
            //spriteRenderer.flipX = true;
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(boxCollider.bounds.min, boxCollider.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }
}
