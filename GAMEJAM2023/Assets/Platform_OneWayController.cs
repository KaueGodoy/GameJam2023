using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform_OneWayController : MonoBehaviour
{
    private Transform _player;
    private Platform_OneWayBelowRaycast _platform_OneWayBelow;
    private Platform_OneWayAboveRaycast _platform_OneWayAbove;

    [SerializeField] private float platformActivationRadius = 5f;
    private float platformActivationRadiusSquared;

    [SerializeField] private int updateFrequency = 3;
    private int frameCounter = 0;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        TryGetComponent(out _platform_OneWayBelow);
        TryGetComponent(out _platform_OneWayAbove);

        platformActivationRadiusSquared = platformActivationRadius * platformActivationRadius;
    }

    private void Update()
    {
        frameCounter++;

        if (frameCounter >= updateFrequency)
        {
            float squaredDistance = (_player.position - transform.position).sqrMagnitude;

            bool playerInsideRadius = squaredDistance <= platformActivationRadiusSquared;

            // Enable or disable the platform components based on player proximity
            if (_platform_OneWayBelow != null)
                _platform_OneWayBelow.enabled = playerInsideRadius;

            if (_platform_OneWayAbove != null)
                _platform_OneWayAbove.enabled = playerInsideRadius;

            frameCounter = 0;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, platformActivationRadius);
    }
}
