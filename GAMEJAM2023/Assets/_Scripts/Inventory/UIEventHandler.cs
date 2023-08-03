using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEventHandler : MonoBehaviour
{
    public delegate void ItemEventHandler(Item item);
    public static event ItemEventHandler OnItemAddedToInventory;
    public static event ItemEventHandler OnItemEquipped;
    public static event ItemEventHandler OnSkillEquipped;
    public static event ItemEventHandler OnUltEquipped;

    public delegate void PlayerHealthEventHandler(float currentHealth, float maxHealth);
    public static event PlayerHealthEventHandler OnPlayerHealthChanged;

    public delegate void StatsEventHandler();
    public static event StatsEventHandler OnStatsChanged;   

    public static void ItemAddedToInventory(Item item)
    {
        OnItemAddedToInventory?.Invoke(item);
    }

    public static void ItemEquipped(Item item)
    {
        OnItemEquipped?.Invoke(item);
    }

    public static void SkillEquipped(Item item)
    {
        OnSkillEquipped?.Invoke(item);
    }

    public static void UltEquipped(Item item)
    {
        OnUltEquipped?.Invoke(item);
    }

    public static void HealthChanged(float currentHealth, float maxHealth)
    {
        OnPlayerHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public static void StatsChanged()
    {
        OnStatsChanged?.Invoke();
    }
}
