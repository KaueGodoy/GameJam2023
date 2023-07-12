using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionItem : Interactable
{
    public GameObject interactPanel;
    private bool panelIsActive;

    public override void Interact()
    {
        Debug.Log("Interacting with base ActionItem class /n panel active");
        interactPanel.SetActive(!panelIsActive);
    }

    public override void EndInteraction()
    {
        Debug.Log("Interacting with base ActionItem class /n panel disabled");
        interactPanel.SetActive(panelIsActive);
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }

    public override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
    }



}
