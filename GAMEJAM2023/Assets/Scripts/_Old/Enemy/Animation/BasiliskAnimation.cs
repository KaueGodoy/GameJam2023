using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasiliskAnimation : MonoBehaviour
{
    Rigidbody2D rb;

    // animation
    private Animator animator;
    private string currentAnimation;

    const string BASILISK_WALK = "Basilisk_Walk";
    const string BASILISK_ATTACK = "Basilisk_Attack";
    const string BASILISK_HIT = "Basilisk_Hit";



    public GameObject player;
    private EnemyShooting enemyShooting;
    private Enemy enemy;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        player = GameObject.FindGameObjectWithTag("Player");
        enemyShooting = GetComponent<EnemyShooting>();
        enemy = GetComponent<Enemy>();


    }


    private void FixedUpdate()
    {
        UpdateAnimationState();
    }

    public void UpdateAnimationState()
    {

        float distance = Vector2.Distance(transform.position, player.transform.position);

        if (enemy.animationHit)
        {
            ChangeAnimationState(BASILISK_HIT);

        }
        else if (!enemy.animationHit)
        {
            if (distance < enemy.rangeDistance)
            {
                ChangeAnimationState(BASILISK_ATTACK);
            }
            else
            {
                ChangeAnimationState(BASILISK_WALK);
            }

        }
    }


    public void ChangeAnimationState(string newAnimation)
    {
        if (currentAnimation == newAnimation) return;

        animator.Play(newAnimation);
        currentAnimation = newAnimation;

    }
}
