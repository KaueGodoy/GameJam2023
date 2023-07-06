using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkill
{
    List<BaseStat> Stats { get; set; }
    float CurrentDamage { get; set; }
    void PerformAttack(float damage);
    void PerformSkillAttack();
    void PerformUltAttack();
}
