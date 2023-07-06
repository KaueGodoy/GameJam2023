using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTest : MonoBehaviour, ISkill
{
    private Animator animator;
    public List<BaseStat> Stats { get; set; }
    public CharacterStats CharacterStats { get; set; }
    public float CurrentDamage { get; set; }
    public bool isCritical;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void PerformAttack(float damage)
    {
        CurrentDamage = damage;

        //animator.SetTrigger("Base_Attack");
        Debug.Log(this.name + " Skill used!");
    }

    public void PerformSkillAttack()
    {
      
    }

    public void PerformUltAttack()
    {
      
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<IEnemy>().TakeDamage(CurrentDamage);
            DamagePopup.Create(transform.position, (int)CurrentDamage, isCritical);
            Debug.Log("Hit: " + collision.name);
        }
    }
}
