using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public virtual void Interact()
    {
        Debug.Log("Interacting with base class");
    }

    public virtual void EndInteraction()
    {
        Debug.Log("Interaction ended");
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();

        if (player != null)
        {
            if (collision.CompareTag("Player"))
            {
                Interact();
            }
        }
    }

    public virtual void OnTriggerExit2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();

        if (player != null)
        {
            if (collision.CompareTag("Player"))
            {
                EndInteraction();
            }
        }
    }




}
