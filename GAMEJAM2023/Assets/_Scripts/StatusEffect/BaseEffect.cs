using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEffect : ScriptableObject
{
    [SerializeField] protected float Duration = 0f;

    public bool IsActive => DurationRemaining > 0f;

    float DurationRemaining = 0f;

    public void EnableEffect()
    {
        DurationRemaining = Duration;
    }

    public void TickEffect()
    {
        if (DurationRemaining > 0f)
        {
            DurationRemaining -= Time.deltaTime;
        }
    }

    public virtual float Effect_JumpHeight(float originalJumpHeight)
    {
        return originalJumpHeight;
    }

    public virtual float Effect_JumpHeightBonus(float originalJumpHeightBonus)
    {
        return originalJumpHeightBonus;
    }

    public virtual float Effect_JumpVelocity(float originalJumpVelocity)
    {
        return originalJumpVelocity;
    }

    public virtual float Effect_FallForce(float originalFallForce)
    {
        return originalFallForce;
    }

    public virtual float Effect_GroundSpeed(float originalGroundSpeed)
    {
        return originalGroundSpeed;
    }

    public virtual float Effect_BonusGroundSpeed(float originalBonusGroundSpeed)
    {
        //float bonus = 20;
        return originalBonusGroundSpeed;
        //+ (bonus / 100);
    }

    public virtual float Effect_InAirSpeed(float originalInAirSpeed)
    {
        return originalInAirSpeed;
    }

    public virtual float Effect_MaxHealth(float originalMaxHealth)
    {
        return originalMaxHealth;
    }

    public virtual float Effect_BonusMaxHealth(float originalBonusMaxHealth)
    {
        return originalBonusMaxHealth;
    }
}
