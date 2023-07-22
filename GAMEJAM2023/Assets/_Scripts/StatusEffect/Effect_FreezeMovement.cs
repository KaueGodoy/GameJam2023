using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Freeze Movement", fileName = "Effect_FreezeMovement")]
public class Effect_FreezeMovement : BaseEffect
{
    public override float Effect_GroundSpeed(float originalGroundSpeed)
    {
        return originalGroundSpeed * 0;
    }

    public override float Effect_BonusGroundSpeed(float originalBonusGroundSpeed)
    {
        return originalBonusGroundSpeed;
    }
}
