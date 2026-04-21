using _01_Script.Entities;
using UnityEngine;

namespace _01_Script._04_Combat
{
    public interface IDamageable
    {
        public void ApplyDamage(DamageData damageData, Vector3 direction, Entity dealer = null);
    }
}