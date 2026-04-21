using System;
using _01_Script._04_Combat;
using UnityEngine;

namespace _01_Script._02_Unit.Combat.Projectile
{
    public interface IProjectilable
    {
        public event Action<HitData> OnProjectileHit;
        void InitProjectile(Vector3 startPos, Vector3 dir, float speed);
    }
}