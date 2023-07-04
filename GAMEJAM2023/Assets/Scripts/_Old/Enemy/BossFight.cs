using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFight : MonoBehaviour
{
    GameHandler gameHandler;
    Enemy enemy;
    public Player player;

    // Start is called before the first frame update
    void Start()
    {
        gameHandler = GetComponent<GameHandler>();
        enemy = GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance < enemy.rangeDistance)
        {
            gameHandler.bossRange = true;
        }
    }
}
