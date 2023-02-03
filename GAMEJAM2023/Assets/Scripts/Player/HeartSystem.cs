using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HeartSystem : MonoBehaviour
{

    public GameObject[] hearts;
    private int curLife;
    private bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        curLife = hearts.Length;
    }
   
    // Update is called once per frame
    void Update()
    {
        if(isDead)
        {
            Debug.Log("Morto");
            SceneManager.LoadScene(1);
        }
    }

    public void TakeDamage(int d)
    {
        if(curLife >= 1)
        {
            curLife -= d;
            Destroy(hearts[curLife].gameObject);

            if(curLife < 1)
            {
                isDead = true;
            }
        }
    }
}
