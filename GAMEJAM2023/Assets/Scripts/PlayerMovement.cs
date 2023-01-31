using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // components
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;

    // grounded
    [SerializeField] private LayerMask jumpableGround;

    // movement
    private float moveX = 0f;
    public float moveSpeed = 5f;

    // jump
    public float jumpForce = 4f;

  
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();

    }

    void Update()
    {
        ProcessInput();
    }

    private void FixedUpdate()
    {
        Move();
    }

    void ProcessInput()
    {
        // horizontal movement
        moveX = Input.GetAxisRaw("Horizontal");

        // sprite flip
        if(moveX > 0f)
        {
            spriteRenderer.flipX = false;
        }
        else if(moveX < 0f)
        {
            spriteRenderer.flipX = true;
        }


        // jump
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = Vector2.up * jumpForce;
        }
    }

    void Move()
    {
        rb.velocity = new Vector2(moveX * moveSpeed, rb.velocity.y);
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(boxCollider.bounds.min, boxCollider.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }
}
