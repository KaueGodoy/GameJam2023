using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    Rigidbody2D rb;

    // animation
    private Animator animator;
    private string currentAnimation;

    const string WASP_IDLE = "Wasp_Idle";
    const string WASP_HIT = "Wasp_Hit";
    const string WASP_ATTACK = "Wasp_Attack";

    // beetle
    const string BEETLE_WALK = "Beetle_Walk";
    const string BEETLE_WALK_AGGRO = "Beetle_Walk_Aggro";
    const string BEETLE_HIT = "Beetle_Hit";



    public GameObject player;
    private EnemyShooting enemyShooting;
    private Enemy enemy;
    private EnemyNoAggro enemyNoAggro;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        player = GameObject.FindGameObjectWithTag("Player");
        enemyShooting = GetComponent<EnemyShooting>();
        enemy = GetComponent<Enemy>();
        enemyNoAggro = GetComponent<EnemyNoAggro>();

    }


    private void FixedUpdate()
    {
        UpdateAnimationState();
    }

    public void UpdateAnimationState()
    {
        if (GameObject.FindGameObjectWithTag("Wasp"))
        {
            float distance = Vector2.Distance(transform.position, player.transform.position);

            if (enemy.animationHit)
            {
                ChangeAnimationState(WASP_HIT);

            }
            else if (!enemy.animationHit)
            {
                if (distance < enemyShooting.rangeDistance)
                {
                    ChangeAnimationState(WASP_ATTACK);
                }
                else
                {
                    ChangeAnimationState(WASP_IDLE);
                }

            }
        }

        if (GameObject.FindGameObjectWithTag("Beetle"))
        {
            if(enemyNoAggro.animationHit)
            {
                ChangeAnimationState(BEETLE_HIT);
            }
            else if (!enemyNoAggro.animationHit)
            {
                ChangeAnimationState(BEETLE_WALK);
            }

            /*
            if (!enemyNoAggro.aggro)
            {
                if (enemyNoAggro.animationHit)
                {
                    ChangeAnimationState(BEETLE_HIT);
                }
                else 
                {
                    ChangeAnimationState(BEETLE_WALK);
                }
            }
            else if (enemyNoAggro.aggro)
            {
                if (enemyNoAggro.animationHit)
                {
                    ChangeAnimationState(BEETLE_HIT);
                }
                else
                {
                    ChangeAnimationState(BEETLE_WALK_AGGRO);
                }
            }*/
           

        }

    }

    public void ChangeAnimationState(string newAnimation)
    {
        if (currentAnimation == newAnimation) return;

        animator.Play(newAnimation);
        currentAnimation = newAnimation;

    }
}
