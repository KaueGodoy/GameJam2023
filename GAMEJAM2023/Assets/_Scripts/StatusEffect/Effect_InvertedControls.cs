using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Inverted Controls", fileName = "Effect_InvertedControls")]
public class Effect_InvertedControls : BaseEffect
{
    public override float Effect_GroundSpeed(float originalGroundSpeed)
    {
        return originalGroundSpeed * -1f;
    }

    public override float Effect_BonusGroundSpeed(float originalBonusGroundSpeed)
    {
        return originalBonusGroundSpeed;
    }
}
