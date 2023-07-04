using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop_Coin : Interactable
{
    private InventoryController inventoryController;

    private void Start()
    {
        // Find the player object with the "Player" tag and get its InventoryController component
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        inventoryController = player.GetComponent<InventoryController>();
    }

    public override void Interact()
    {
        Debug.Log("Interacting with lore icon");

        // pop up UI (press F to pick)
        // press F 
        // add to inventory
        if (inventoryController != null)
        {
            inventoryController.GiveItem("coin");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Interact();
        // play sfx
        // destroy game obj
    }
}
