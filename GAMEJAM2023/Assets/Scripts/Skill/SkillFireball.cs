using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillFireball : MonoBehaviour, ISkill, IProjectileSkill
{
    private Animator animator;
    public List<BaseStat> Stats { get; set; }
    public Transform ProjectileSpawn { get; set; }
    public CharacterStats CharacterStats { get; set; }
    public float CurrentDamage { get; set; }

    // projectile reference
    SkillFireballProjectile SkillFireballProjectile { get; set; }

    private void Awake()
    {
        SkillFireballProjectile = Resources.Load<SkillFireballProjectile>("Projectiles/skillFireballProjectile");
        animator = GetComponent<Animator>();
    }

    public void PerformAttack(float damage)
    {
        CurrentDamage = damage;
        animator.SetTrigger("Base_Attack");
        Debug.Log(this.name + " Skill used!");
    }

    public void PerformSkillAttack()
    {

    }

    public void PerformUltAttack()
    {

    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Enemy"))
    //    {
    //        collision.GetComponent<IEnemy>().TakeDamage(CurrentDamage);
    //        DamagePopup.Create(transform.position, (int)CurrentDamage, isCritical);
    //        Debug.Log("Hit: " + collision.name);
    //    }
    //}

    public void CastProjectile()
    {
        SkillFireballProjectile SkillFireballProjectileInstance = (SkillFireballProjectile)Instantiate(SkillFireballProjectile, ProjectileSpawn.position, ProjectileSpawn.rotation);
        SkillFireballProjectileInstance.Direction = ProjectileSpawn.right;
    }
}
