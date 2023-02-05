using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingWall : MonoBehaviour
{
    Rigidbody2D rb;
    BoxCollider2D boxCollider2D;
    public float distance;
    bool isFalling = false;
    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = 15;
            isFalling = true;
        }
        
    }
}
