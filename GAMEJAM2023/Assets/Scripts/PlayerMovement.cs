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
  
    //facing direction
    private bool isFacingRight = true;


    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();

    }

    void Update()
    {
        if (isDashing)
        {
            return;
        }

        ProcessInput();
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }

        Move();
        Jump();
        Dash();
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
            StartCoroutine(Dash());
        }

        Flip();
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

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;

        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        rb.velocity = new Vector2(transform.localScale.x * dashSpeed, 0f);

        yield return new WaitForSeconds(dashingTime);

        rb.gravityScale = originalGravity;
        isDashing = false;

        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;

    }

    private void Flip()
    {
        if(isFacingRight && moveX < 0f || !isFacingRight && moveX > 0f)
        {
            Vector3 localScale = transform.localScale;
            isFacingRight = !isFacingRight;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(boxCollider.bounds.min, boxCollider.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }
}
