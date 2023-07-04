using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lore : ActionItem
{
    public override void Interact()
    {
        base.Interact();

        Debug.Log("Interacting with lore item");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Interact();
    }


}
