using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private HealthSystem healthSystem;

    public void Setup(HealthSystem healthSystem)
    {
        this.healthSystem = healthSystem;
        healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;
        UpdateHealthBar();
    }
    private void HealthSystem_OnHealthChanged(object sender, System.EventArgs e)
    {
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        //if (healthSystem != null)
        //{
        //    transform.Find("Bar").localScale = new Vector3(healthSystem.GetHealthPercent(), 1);
        //}

        Transform barTransform = transform.Find("Bar");

        if (barTransform != null)
        {
            barTransform.localScale = new Vector3(healthSystem.GetHealthPercent(), 1);
        }
        else
        {
            Debug.LogWarning("Bar transform not found.");
        }

    }

    private void Update()
    {
        //transform.Find("Bar").localScale = new Vector3(healthSystem.GetHealthPercent(), 1);
    }
}
