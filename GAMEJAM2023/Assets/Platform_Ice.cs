using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform_Ice : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.rigidbody.freezeRotation = false;
        }
      
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.rigidbody.freezeRotation = true;
            collision.rigidbody.rotation = 0f;
        }
    }
}
