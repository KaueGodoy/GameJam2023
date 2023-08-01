using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy
{
    int ID { get; set; }
    float CurrentHealth { get; set; }
    float MaxHealth { get; set; }
    void TakeDamage(float damage);
    void PerformAttack();
    void Die();
    bool IsDead { get; set; }
}
