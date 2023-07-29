using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectableObject : MonoBehaviour
{
    private CharacterStats characterStats;
    List<BaseEffect> ActiveEffects = new List<BaseEffect>();

    private void Start()
    {
        characterStats = GetComponent<Player>().characterStats;
    }
    void Update()
    {
        // tick all active effects - clean up any that are finished (in reverse order to not get null index)
        for (int index = ActiveEffects.Count - 1; index >= 0; --index)
        {
            ActiveEffects[index].TickEffect();

            // effect finished?
            if (!ActiveEffects[index].IsActive)
            {
                ActiveEffects.RemoveAt(index);
            }
        }
    }

    public void ApplyEffect(BaseEffect effectTemplate)
    {
        // create a new instance of the effect
        var newEffect = ScriptableObject.Instantiate(effectTemplate);

        // make the effect active
        newEffect.EnableEffect();
        ActiveEffects.Add(newEffect);
    }

    public float Effect_JumpHeight(float originalJumpHeight)
    {
        float workingJumpHeight = originalJumpHeight;

        for (int index = 0; index < ActiveEffects.Count; ++index)
        {
            if (!ActiveEffects[index].IsActive)
            {
                continue;
            }

            workingJumpHeight = ActiveEffects[index].Effect_JumpHeight(workingJumpHeight);
        }

        return workingJumpHeight;
    }

    public float Effect_JumpVelocity(float originalJumpVelocity)
    {
        float workingJumpVelocity = originalJumpVelocity;

        for (int index = 0; index < ActiveEffects.Count; ++index)
        {
            if (!ActiveEffects[index].IsActive)
            {
                continue;
            }

            workingJumpVelocity = ActiveEffects[index].Effect_JumpVelocity(workingJumpVelocity);
        }

        return workingJumpVelocity;
    }

    public float Effect_FallForce(float originalFallForce)
    {
        float workingFallForce = originalFallForce;

        for (int index = 0; index < ActiveEffects.Count; ++index)
        {
            if (!ActiveEffects[index].IsActive)
            {
                continue;
            }

            workingFallForce = ActiveEffects[index].Effect_FallForce(workingFallForce);
        }

        return workingFallForce;
    }

    public float Effect_GroundSpeed(float originalGroundSpeed)
    {
        float workingGroundSpeed = originalGroundSpeed;

        for (int index = 0; index < ActiveEffects.Count; ++index)
        {
            if (!ActiveEffects[index].IsActive)
            {
                continue;
            }

            workingGroundSpeed = ActiveEffects[index].Effect_GroundSpeed(workingGroundSpeed);
        }

        return workingGroundSpeed;
    }

    public float Effect_BonusGroundSpeed(float originalBonusGroundSpeed)
    {
        float workingBonusGroundSpeed = originalBonusGroundSpeed;

        for (int index = 0; index < ActiveEffects.Count; ++index)
        {
            if (!ActiveEffects[index].IsActive)
            {
                continue;
            }

            workingBonusGroundSpeed = ActiveEffects[index].Effect_BonusGroundSpeed(workingBonusGroundSpeed);
        }

        return workingBonusGroundSpeed;
        ////float bonus = 20;
        //return originalBonusGroundSpeed;
        ////+ (bonus / 100);
    }

    public float Effect_InAirSpeed(float originalInAirSpeed)
    {
        float workingInAirSpeed = originalInAirSpeed;

        for (int index = 0; index < ActiveEffects.Count; ++index)
        {
            if (!ActiveEffects[index].IsActive)
            {
                continue;
            }

            workingInAirSpeed = ActiveEffects[index].Effect_InAirSpeed(workingInAirSpeed);
        }

        return workingInAirSpeed;
    }

    public float Effect_MaxHealth(float originalMaxHealth)
    {
        float workingMaxHealth = originalMaxHealth;

        for (int index = 0; index < ActiveEffects.Count; ++index)
        {
            if (!ActiveEffects[index].IsActive)
            {
                continue;
            }

            workingMaxHealth = ActiveEffects[index].Effect_MaxHealth(workingMaxHealth);
        }

        return workingMaxHealth;
    }

    public float Effect_BonusMaxHealth(float originalBonusMaxHealth)
    {
        float workingBonusMaxHealth = originalBonusMaxHealth;

        for (int index = 0; index < ActiveEffects.Count; ++index)
        {
            if (!ActiveEffects[index].IsActive)
            {
                continue;
            }

            workingBonusMaxHealth = ActiveEffects[index].Effect_BonusMaxHealth(workingBonusMaxHealth);
        }
        //workingBonusMaxHealth += (characterStats.GetStat(BaseStat.BaseStatType.HPBonus).GetCalculatedStatValue() / 100);
        return workingBonusMaxHealth;
    }
}
