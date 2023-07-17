using CodeMonkey;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public CameraFollow cameraFollow;
    public Transform playerTransform;
    public Transform enemyTransform;
    public Transform randomObj;
    public bool bossRange = false;

    private void Awake()
    {
        cameraFollow.Setup(() => playerTransform.position);

    }

    private void Start()
    {

        /*
        if (bossRange)
        {
            // manual change
            cameraFollow.SetGetCameraFollowPositionFunc(() => randomObj.position);
        }
        else
        {
            // follow player
            cameraFollow.Setup(() => playerTransform.position);

        }

        */

        /*
        // UI Button
        CMDebug.ButtonUI(new Vector2(20, 20), "Enemy", () =>
        {
            cameraFollow.SetGetCameraFollowPositionFunc(() => randomObj.position);

        });
        */
    }

    private void Update()
    {

        if (bossRange)
        {
            // manual change
            cameraFollow.SetGetCameraFollowPositionFunc(() => randomObj.position);
        }
        else
        {
            // follow player
            cameraFollow.Setup(() => playerTransform.position);

        }
    }

}

