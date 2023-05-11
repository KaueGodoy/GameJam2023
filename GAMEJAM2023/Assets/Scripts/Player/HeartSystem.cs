using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class HeartSystem : MonoBehaviour
{
    public Image[] hearts;
    public Sprite lifeFruit;
    public int currentHealth;
    public int maxHealth = 3;

    void Start()
    {
        currentHealth = hearts.Length;
    }

    private void FixedUpdate()
    {
        UpdateHeart();
    }

    private void UpdateHeart()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i >= currentHealth)
            {
                hearts[i].enabled = false;
            }
            else
            {
                hearts[i].enabled = true;
            }
        }
    }
}
