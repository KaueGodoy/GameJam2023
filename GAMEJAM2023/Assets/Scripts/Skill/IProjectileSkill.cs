using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectileSkill
{
    public Transform ProjectileSpawn { get; set; }
    void CastProjectile();
}
