using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plat_WaypointFollower : MonoBehaviour
{
    [Header("Waypoints")]
    [SerializeField] private GameObject[] waypoints;
    private int currentWaypointIndex = 0;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;

    private void FixedUpdate()
    {
        if (Vector3.Distance(waypoints[currentWaypointIndex].transform.position, transform.position) < .1f)
        {
            currentWaypointIndex++;

            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0;

            }  

        }
        transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWaypointIndex].transform.position, Time.deltaTime * moveSpeed);
    }


}

