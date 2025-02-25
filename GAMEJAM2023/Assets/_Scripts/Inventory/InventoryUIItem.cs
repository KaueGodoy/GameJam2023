using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUIItem : MonoBehaviour
{
    public Item item;
    public TextMeshProUGUI itemText;
    public Image itemIcon;

    private void Start()
    {
        transform.localScale = Vector3.one;
    }

    public void SetItem(Item item)
    {
        this.item = item;
        SetupItemValues();
    }

    void SetupItemValues()
    {
        itemText.text = item.ItemName;
        itemIcon.sprite = Resources.Load<Sprite>("UI/Icons/Items/" + item.ObjectSlug);
    }

    public void OnSelectItemButton()
    {
        InventoryController.Instance.SetItemDetails(item, GetComponent<Button>());
    }
}
