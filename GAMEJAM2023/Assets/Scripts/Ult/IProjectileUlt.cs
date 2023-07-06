using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectileUlt
{
    public Transform ProjectileSpawn { get; set; }
    void CastProjectile();
}
