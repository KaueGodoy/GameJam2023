using System.Collections.Generic;
using UnityEngine;

public class Chest_GiveItem : ActionItem
{
    [Header("Drop")]
    public PickupItem pickupItem;
    public DropTable DropTable { get; set; }

    private void Start()
    {
        DropTable = new DropTable();
        DropTable.loot = new List<LootDrop>
        {
            new LootDrop("coin", 100),
        };
    }

    private void DropLoot()
    {
        Item item = DropTable.GetDrop();
        if (item != null)
        {
            //PickupItem instance = Instantiate(pickupItem, transform.position, Quaternion.identity);
            //instance.ItemDrop = item;
            InventoryController.Instance.GiveItem(item);
        }

    }

    public override void Interact()
    {
        base.Interact();
        Debug.Log("Interacting with chest item");
    }

    public override void InteractWithItem()
    {
        if (_playerInput.Player.Interaction.triggered)
        {
            DropLoot();
            Debug.Log("Chest item");
        }
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }

    public override void EndInteraction()
    {
        base.EndInteraction();
    }
}
