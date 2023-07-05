using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionItem : Interactable
{
    public override void Interact()
    {
        Debug.Log("Interacting with base ActionItem class");
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }

}
