using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Change Max Health", fileName = "Effect_ChangeMaxHealth")]
public class Effect_ChangeMaxHealth : BaseEffect
{
    public float MaxHealthIncrease;
    public float MaxHealthBonusIncrease;

    public override float Effect_MaxHealth(float originalMaxHealth)
    {
        return base.Effect_MaxHealth(originalMaxHealth);
    }

    public override float Effect_BonusMaxHealth(float originalBonusMaxHealth)
    {
        return base.Effect_BonusMaxHealth(originalBonusMaxHealth) + (MaxHealthBonusIncrease / 100);
    }
}
