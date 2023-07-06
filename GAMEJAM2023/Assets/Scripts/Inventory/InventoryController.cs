using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    public static InventoryController Instance { get; set; }

    public PlayerWeaponController playerWeaponController;
    public PlayerSkillController playerSkillController;
    public PlayerUltController playerUltController;

    public ConsumableController consumableController;
    public InventoryUIDetails inventoryDetailsPanel;

    public List<Item> playerItems = new List<Item>();

    private void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        playerWeaponController = GetComponent<PlayerWeaponController>();
        playerSkillController = GetComponent<PlayerSkillController>();
        playerUltController = GetComponent<PlayerUltController>();
        consumableController = GetComponent<ConsumableController>();

        GiveItem("sword");
        GiveItem("staff");
        //GiveItem("potion_log");
        //GiveItem("potion_hp");
        //GiveItem("coin");
        GiveItem("skillTest");
        GiveItem("ultTest");
    }

    public void GiveItem(string itemSlug)
    {
        Item item = ItemDatabase.Instance.GetItem(itemSlug);
        playerItems.Add(item);
        Debug.Log(playerItems.Count + " items in inventory. Added: " + itemSlug);
        UIEventHandler.ItemAddedToInventory(item);
    }
    public void GiveItem(Item item)
    {
        playerItems.Add(item);
        Debug.Log(playerItems.Count + " items in inventory. Added: " + item.ItemName);
        UIEventHandler.ItemAddedToInventory(item);
    }

    public void SetItemDetails(Item item, Button selectedButton)
    {
        inventoryDetailsPanel.SetItem(item, selectedButton);
    }

    public void EquipItem(Item itemToEquip)
    {
        playerWeaponController.EquipWeapon(itemToEquip);
    }

    public void EquipSkill(Item itemToEquip)
    {
        playerSkillController.EquipSkill(itemToEquip);
    }

    public void EquipUlt(Item itemToEquip)
    {
        playerUltController.EquipUlt(itemToEquip);
    }

    public void ConsumeItem(Item itemToConsume)
    {
        consumableController.ConsumeItem(itemToConsume);
    }

}
