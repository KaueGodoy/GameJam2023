using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour, ICollectible
{
    public static event HandleCoinCollected OnCoinCollected;
    public delegate void HandleCoinCollected(ItemData itemData);
    public ItemData coinData;

    public void Collect()
    { 
        Debug.Log("Random collected");
        Destroy(gameObject);
        OnCoinCollected?.Invoke(coinData);
        
    }
    
}
