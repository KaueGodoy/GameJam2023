using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop_Coin : Interactable
{
    public override void Interact()
    {
        Debug.Log("Interacting with coin");

        // pop up UI (press F to pick)
        // press F 
        // add to inventory
        InventoryController.Instance.GiveItem("coin");
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        // play sfx
        // destroy game obj
    }
}
