using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    private bool levelCompleted = false;
    public float delayTime = 1f;
    public GameObject pfOperDoor;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && !levelCompleted)
        {
            levelCompleted = true;
            OpenDoor();
            Invoke("CompleteLevel", delayTime);
        }
    }

    private void CompleteLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void OpenDoor()
    {
        gameObject.SetActive(false);
        //Instantiate(pfOperDoor, transform.position, transform.rotation);
        
    }
}
