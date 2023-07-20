using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPPotion : MonoBehaviour, IConsumable
{
    public List<BaseStat> Stats { get; set; }
    public void Consume()
    {
        Debug.Log("HP potion consumed. Stats NOT changed");
        Destroy(gameObject);
    }

    public void Consume(CharacterStats stats)
    {
        Debug.Log("HP potion consumed. Stats changed");

        Destroy(gameObject);
    }



}
