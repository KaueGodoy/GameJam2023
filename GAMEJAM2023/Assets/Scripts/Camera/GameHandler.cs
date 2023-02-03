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

    private void Start()
    {
        // follow player
        cameraFollow.Setup(()=> playerTransform.position);

        /*
        // manual change
        cameraFollow.SetGetCameraFollowPositionFunc(() => enemyTransform.position);

        // UI Button
        CMDebug.ButtonUI(new Vector2(20, 20), "Enemy", () =>
        {
            cameraFollow.SetGetCameraFollowPositionFunc(() => randomObj.position);

        });
        */
    }

}

