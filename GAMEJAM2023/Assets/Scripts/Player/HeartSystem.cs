using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class HeartSystem : MonoBehaviour
{

    public Image[] hearts;
    public Sprite lifeFruit;
    public int curLife;
    public int maxLife = 3;
    private bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        curLife = hearts.Length;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i >= curLife)
            {
                hearts[i].enabled = false;
            }
            else
            {
                hearts[i].enabled = true;
            }
        }

        if (isDead)
        {
            Debug.Log("Morto");
            SceneManager.LoadScene(1);
        }
    }

    public void TakeDamage(int d)
    {
        if (d > curLife)
        {
            d = curLife;
        }

        if (curLife >= 1)
        {
            curLife -= d;

            if (curLife < 1)
            {
                isDead = true;
            }
        }
    }

    public void Heal(int h)
    {
        if (h > maxLife - curLife)
        {
            h = maxLife - curLife;
        }

        if (curLife < maxLife)
        {
            curLife += h;
        }
    }
}
