using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    [Header("Objects")]
    public GameObject pfBullet;
    public Transform bulletPos;
    private GameObject player;

    [Header("Shooting")]
    public float shootTimer;
    public float shootCooldown = 2f;

    [Header("Range")]
    public float rangeDistance = 10f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);
        Debug.Log(distance);

        if (distance < rangeDistance)
        {
            shootTimer += Time.deltaTime;

            if (shootTimer > shootCooldown)
            {
                shootTimer = 0;
                Shoot();

            }

        }

    }

    private void Shoot()
    {
        Instantiate(pfBullet, bulletPos.position, Quaternion.identity);
    }
}
