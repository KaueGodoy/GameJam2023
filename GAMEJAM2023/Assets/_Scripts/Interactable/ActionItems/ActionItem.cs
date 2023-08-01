using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionItem : Interactable
{
    public GameObject interactPanel;
    private bool panelIsActive;

    protected PlayerControls _playerInput;
    private bool _canInteract;

    private void Awake()
    {
        _playerInput = new PlayerControls();
    }

    private void OnEnable()
    {
        _playerInput.Enable();
    }

    private void OnDisable()
    {
        _playerInput.Disable();
    }

    public virtual void Update()
    {
        if (!_canInteract) return;
        InteractWithItem();
    }

    public override void Interact()
    {
        Debug.Log("Initiating interaction with base ActionItem class");
      
        interactPanel.SetActive(!panelIsActive);
        _canInteract = true;
    }

    public virtual void InteractWithItem()
    {
        if (_playerInput.Player.Interaction.triggered)
        {
            Debug.Log("Interacted with base ActionItem class");
        }
    }

    public override void EndInteraction()
    {
        Debug.Log("Ending interaction with base ActionItem class");
        interactPanel.SetActive(panelIsActive);
        _canInteract = false;
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
