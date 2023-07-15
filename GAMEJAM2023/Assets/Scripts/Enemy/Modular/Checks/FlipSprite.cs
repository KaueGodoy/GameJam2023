using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipSprite : MonoBehaviour
{
    private Transform target;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
   
    public bool isFacingRight;

    private void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            target = playerObject.transform;
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void Flip2()
    {
        if (isFacingRight && transform.position.x < 0f || !isFacingRight && transform.position.x > 0f)
        {
            isFacingRight = !isFacingRight;
            spriteRenderer.flipX = isFacingRight;
        }

    }

    public void FlipOld()
    {
        if (isFacingRight && transform.position.x < 0f || !isFacingRight && transform.position.x > 0f)
        {
            Vector3 localScale = transform.localScale;
            isFacingRight = !isFacingRight;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    public void FlipRoam()
    {
        Vector3 direction = rb.velocity;

        if (isFacingRight && direction.x < 0f || !isFacingRight && direction.x > 0f)
        {
            isFacingRight = !isFacingRight;
            spriteRenderer.flipX = isFacingRight;
        }
    }

    public void FlipBasedOnPlayerPosition()
    {
        Vector3 direction = (target.position - transform.position).normalized;

        if (isFacingRight && direction.x < 0f || !isFacingRight && direction.x > 0f)
        {
            isFacingRight = !isFacingRight;
            spriteRenderer.flipX = isFacingRight;
        }
    }
}
