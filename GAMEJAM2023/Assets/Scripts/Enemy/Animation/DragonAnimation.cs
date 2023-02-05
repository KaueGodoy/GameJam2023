using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonAnimation : MonoBehaviour
{
    Rigidbody2D rb;

    // animation
    private Animator animator;
    private string currentAnimation;

    const string DRAGON_IDLE = "DragonBoss_Idle";
    const string DRAGON_ATTACK = "DragonBoss_Attack";
    const string DRAGON_RISE = "DragonBoss_Rise";

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

        if (enemy.isAlive)
        {
            float distance = Vector2.Distance(transform.position, player.transform.position);

            if (enemy.animationHit)
            {
                ChangeAnimationState(DRAGON_IDLE);

            }
            else if (!enemy.animationHit)
            {
                if (distance < enemyShooting.rangeDistance)
                {
                    ChangeAnimationState(DRAGON_ATTACK);
                }
                else
                {
                    ChangeAnimationState(DRAGON_IDLE);
                }

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
