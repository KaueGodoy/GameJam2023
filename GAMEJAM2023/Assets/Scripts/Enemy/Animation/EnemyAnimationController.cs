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

        

    }

    public void ChangeAnimationState(string newAnimation)
    {
        if (currentAnimation == newAnimation) return;

        animator.Play(newAnimation);
        currentAnimation = newAnimation;

    }
}
