using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    private static DontDestroy instance;

    private void Awake()
    {
        // Check if an instance of ItemManager already exists.
        if (instance == null)
        {
            // If not, set this instance as the active instance and make it persistent.
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // If another instance already exists, destroy this GameObject.
            Destroy(gameObject);
        }
    }
}
