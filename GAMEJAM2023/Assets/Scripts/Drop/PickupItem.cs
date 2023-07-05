using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : Interactable
{
    public Item ItemDrop { get; set; }

    // override and inherits from interactable

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }

    public override void Interact()
    {
        InventoryController.Instance.GiveItem(ItemDrop);
        Debug.Log("Interacting with item object: " + ItemDrop.ItemName);
        Destroy(gameObject);
    }
}
