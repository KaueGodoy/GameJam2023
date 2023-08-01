using UnityEngine;
using UnityEngine.InputSystem;

public class Lore : ActionItem
{
    private bool lorePanelIsActive;
    public GameObject itemPanel;

    public override void Interact()
    {
        base.Interact();
        Debug.Log("Interacting with lore item");
    }

    public override void InteractWithItem()
    {
        if (_playerInput.Player.Interaction.triggered)
        {
            itemPanel.SetActive(!lorePanelIsActive);
            Debug.Log("did something.");
        }
    }

    public override void EndInteraction()
    {
        base.EndInteraction();
        itemPanel.SetActive(lorePanelIsActive);
    }
}
