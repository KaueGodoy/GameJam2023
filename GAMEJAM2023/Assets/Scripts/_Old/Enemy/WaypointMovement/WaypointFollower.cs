using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointFollower : MonoBehaviour
{
    [Header("Waypoints")]
    [SerializeField] private GameObject[] waypoints;
    private int currentWaypointIndex = 0;

    [Header("Movement")]
    [SerializeField] private float speed = 2f;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.flipX = false;
    }

    private void Update()
    {
        ProcessCalc();
    }

    private void FixedUpdate()
    {
        MoveEnemy();
    }

    private void ProcessCalc()
    {
        if (Vector3.Distance(waypoints[currentWaypointIndex].transform.position, transform.position) < .1f)
        {
            currentWaypointIndex++;
            //transform.Rotate(transform.position.x, 180f, transform.position.z);

            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0;
                spriteRenderer.flipX = false;
            }
            else
            {
                spriteRenderer.flipX = true;
            }

        }
    }

    private void MoveEnemy()
    {
        transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWaypointIndex].transform.position, Time.deltaTime * speed);

    }
}

