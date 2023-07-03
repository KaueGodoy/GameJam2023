using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeetleAnimation : MonoBehaviour
{
    Rigidbody2D rb;

    // animation
    private Animator animator;
    private string currentAnimation;

    // beetle
    const string BEETLE_WALK = "Beetle_Walk";
    const string BEETLE_WALK_AGGRO = "Beetle_Walk_Aggro";
    const string BEETLE_HIT = "Beetle_Hit";

    public GameObject player;
    private EnemyNoAggro enemyNoAggro;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        player = GameObject.FindGameObjectWithTag("Player");
        enemyNoAggro = GetComponent<EnemyNoAggro>();

    }


    private void FixedUpdate()
    {
        UpdateAnimationState();
    }

    public void UpdateAnimationState()
    {

        float distance = Vector2.Distance(transform.position, player.transform.position);

        if (enemyNoAggro.animationHit)
        {
            ChangeAnimationState(BEETLE_HIT);

        }
        else if (!enemyNoAggro.animationHit)
        {
            if (distance < enemyNoAggro.rangeDistance)
            {
                ChangeAnimationState(BEETLE_WALK_AGGRO);
            }
            else
            {
                ChangeAnimationState(BEETLE_WALK);
            }
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

    public void ChangeAnimationState(string newAnimation)
    {
        if (currentAnimation == newAnimation) return;

        animator.Play(newAnimation);
        currentAnimation = newAnimation;

    }
}
