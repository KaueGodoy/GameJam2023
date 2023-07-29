using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Insta Kill", fileName = "Effect_InstaKill")]
public class Effect_InstaKill : BaseEffect
{
    public override float Effect_MaxHealth(float originalMaxHealth)
    {
        return base.Effect_MaxHealth(originalMaxHealth) * 0;
    }

}
