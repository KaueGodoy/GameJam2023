using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Low Gravity", fileName = "Effect_LowGravity")]
public class Effect_LowGravity : BaseEffect
{
    [SerializeField] float JumpHeightModifier = 20f;
    [SerializeField] float JumpVelocityModifier = 2f;
    [SerializeField] float FallForceModifier = 0.25f;

    public override float Effect_JumpHeight(float originalJumpHeight)
    {
        return originalJumpHeight;
    }

    public override float Effect_JumpHeightBonus(float originalJumpHeightBonus)
    {
        return originalJumpHeightBonus + (JumpHeightModifier / 100);
    }

    public override float Effect_JumpVelocity(float originalJumpVelocity)
    {
        return originalJumpVelocity * JumpVelocityModifier;
    }

    public override float Effect_FallForce(float originalFallForce)
    {
        return originalFallForce * FallForceModifier;
    }
}
