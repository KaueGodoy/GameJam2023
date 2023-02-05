using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    [Header("Objects")]
    public GameObject pfBullet;
    public Transform  bulletPos;

    [Header("Shooting")]
    public float timer;
    public float shootCooldown = 2f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if(timer > shootCooldown)
        {
            timer = 0;
            Shoot();
        }
    }

    private void Shoot()
    {
        Instantiate(pfBullet, bulletPos.position, Quaternion.identity);
    }
}
