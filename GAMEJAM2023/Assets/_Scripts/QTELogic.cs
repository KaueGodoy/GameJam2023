using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QTELogic : MonoBehaviour
{
    public float currentPressAmount;
    public float requiredPressAmount;
    public float target;
    public static bool stuck;

    private PlayerControls playerControls;

    private void Awake()
    {
        playerControls = new PlayerControls();
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
        if (playerControls.Player.Interaction.triggered)
        {
            UpdateProgress();
        }
    }

    public void Initialize()
    {
        currentPressAmount = 0;
        stuck = true;
        QTEUIContainer qteUIContainer = GetComponent<QTEUIContainer>();
        requiredPressAmount =  qteUIContainer.requiredPressAmount;
        target = currentPressAmount / requiredPressAmount;
        // Additional initialization tasks can be performed here
    }

    public void UpdateProgress()
    {
        currentPressAmount++;
        target = currentPressAmount / requiredPressAmount;
        // Perform QTE progress updates and checks here

        if (currentPressAmount >= requiredPressAmount)
        {
            target = currentPressAmount / requiredPressAmount;

            currentPressAmount = 0;
            Debug.Log("Released");
            stuck = false;

            // Destroy the UI element
           Destroy(gameObject, 0.1f);
        }

    }
}
