using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Item
{
    public enum ItemType
    {
        HealthPotion,
        SpeedPotion,
        Coin,
        Key,
    }

    public ItemType itemType;
    public int amount;

    public Sprite GetSprite()
    {
        switch (itemType)
        {
            default:
            case ItemType.HealthPotion: return ItemAssets.Instance.healthPotionSprite;
            case ItemType.SpeedPotion: return ItemAssets.Instance.speedPotionSprite;
            case ItemType.Coin: return ItemAssets.Instance.coinSprite;
            case ItemType.Key: return ItemAssets.Instance.keySprite;
        }
    }

    /*
    public Color GetColor()
    {
        switch (itemType)
        {
            default:
            case ItemType.HealthPotion:     return new Color(1, 0, 0);
            case ItemType.SpeedPotion:      return new Color(1, 1, 0);
            case ItemType.Coin:             return new Color(0, 0, 0);
            case ItemType.Key:              return new Color(0, 1, 1);
        }
    }*/

    public bool IsStackable()
    {
        switch (itemType)
        {
            default:
            case ItemType.SpeedPotion:
            case ItemType.Coin:
            case ItemType.HealthPotion:
                return true;
            case ItemType.Key:
                return false;

        }
    }
}
