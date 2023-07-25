using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryUI : MonoBehaviour
{
    public RectTransform inventoryPanel;

    [Header("Content")]
    public RectTransform WeaponScrollViewContent;
    public RectTransform SkillScrollViewContent;
    public RectTransform UltScrollViewContent;
    public RectTransform ConsumableScrollViewContent;

    [Header("Sections")]
    public RectTransform sectionPanelWeapon;
    public RectTransform sectionPanelSkill;
    public RectTransform sectionPanelUlt;
    public RectTransform sectionPanelConsumable;

    InventoryUIItem ItemContainer { get; set; }
    bool MenuIsActive { get; set; }
    Item CurrentSelectedItem { get; set; }

    private PlayerControls playerControls;

    private void Awake()
    {
        playerControls = new PlayerControls();

        ItemContainer = Resources.Load<InventoryUIItem>("UI/Item_Container");
        UIEventHandler.OnItemAddedToInventory += ItemAdded;
        inventoryPanel.gameObject.SetActive(false);

        sectionPanelWeapon.gameObject.SetActive(true);
        sectionPanelSkill.gameObject.SetActive(false);
        sectionPanelUlt.gameObject.SetActive(false);
        sectionPanelConsumable.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void Start()
    {
        //ItemContainer = Resources.Load<InventoryUIItem>("UI/Item_Container");
        //UIEventHandler.OnItemAddedToInventory += ItemAdded;
        //inventoryPanel.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (playerControls.UI.Inventory.triggered)
        {

            TriggerMenu();

        }
    }

    public void TriggerMenu()
    {
        MenuIsActive = !MenuIsActive;
        inventoryPanel.gameObject.SetActive(MenuIsActive);
    }

    public void EnableSectionWeapon()
    {
        sectionPanelWeapon.gameObject.SetActive(true);
        sectionPanelSkill.gameObject.SetActive(false);
        sectionPanelUlt.gameObject.SetActive(false);
        sectionPanelConsumable.gameObject.SetActive(false);
    }


    public void EnableSectionSkill()
    {
        sectionPanelWeapon.gameObject.SetActive(false);
        sectionPanelSkill.gameObject.SetActive(true);
        sectionPanelUlt.gameObject.SetActive(false);
        sectionPanelConsumable.gameObject.SetActive(false);
    }

    public void EnableSectionUlt()
    {
        sectionPanelWeapon.gameObject.SetActive(false);
        sectionPanelSkill.gameObject.SetActive(false);
        sectionPanelUlt.gameObject.SetActive(true);
        sectionPanelConsumable.gameObject.SetActive(false);
    }

    public void EnableSectionConsumablel()
    {
        sectionPanelWeapon.gameObject.SetActive(false);
        sectionPanelSkill.gameObject.SetActive(false);
        sectionPanelUlt.gameObject.SetActive(false);
        sectionPanelConsumable.gameObject.SetActive(true);
    }

    public void ItemAdded(Item item)
    {
        InventoryUIItem emptyItem = Instantiate(ItemContainer);
        emptyItem.SetItem(item);

        if (item.ItemType == Item.ItemTypes.Weapon)
        {
            emptyItem.transform.SetParent(WeaponScrollViewContent);
        }
        else if (item.ItemType == Item.ItemTypes.Skill)
        {
            emptyItem.transform.SetParent(SkillScrollViewContent);
        }
        else if (item.ItemType == Item.ItemTypes.Ult)
        {
            emptyItem.transform.SetParent(UltScrollViewContent);
        }
        else if (item.ItemType == Item.ItemTypes.Consumable)
        {
            emptyItem.transform.SetParent(ConsumableScrollViewContent);
        }
        else if (item.ItemType == Item.ItemTypes.Quest)
        {
            emptyItem.transform.SetParent(ConsumableScrollViewContent);
        }

    }
}
