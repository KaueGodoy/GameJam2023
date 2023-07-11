using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worm : Beetle
{
    public override void Start()
    {
        base.Start();
        ID = 1;
        Debug.Log("Worm ID: " + ID);
    }




}
