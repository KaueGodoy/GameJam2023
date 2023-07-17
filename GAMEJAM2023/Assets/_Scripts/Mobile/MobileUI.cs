using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileUI : MonoBehaviour
{
    private PlayerControls playerControls;

    public RectTransform mobileUI;

    bool MenuIsActive { get; set; }

    private void Awake()
    {
        playerControls = new PlayerControls();

        mobileUI.gameObject.SetActive(true);
        MenuIsActive = true;
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void Update()
    {
        if (playerControls.UI.Mobile.triggered)
        {
            MenuIsActive = !MenuIsActive;
            mobileUI.gameObject.SetActive(MenuIsActive);
        }
    }

}
