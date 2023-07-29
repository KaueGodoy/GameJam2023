using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Change Movement Speed", fileName = "Effect_ChangeMovementSpeed")]
public class Effect_ChangeMovementSpeed : BaseEffect
{
    public float MovementSpeedBonus;

    public override float Effect_BonusGroundSpeed(float originalBonusGroundSpeed)
    {
        return originalBonusGroundSpeed + (MovementSpeedBonus / 100);
    }
}
