using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float bounce = 2f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "HitStomp")
        {
            PlayerBounce();
        }
    }

    private void PlayerBounce()
    {
        rb.velocity = new Vector2(rb.velocity.x, bounce);
    }
}
