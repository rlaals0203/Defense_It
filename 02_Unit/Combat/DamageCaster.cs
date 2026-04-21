using _01_Script._04_Combat;
using _01_Script.Entities;
using UnityEngine;

namespace _01_Script._02_Unit.Combat
{
    public abstract class DamageCaster : MonoBehaviour
    {
        [SerializeField] protected int maxHitCount = 1;
        [SerializeField] protected LayerMask whatIsTarget;
        
        protected Entity _owner;

        public virtual void InitCaster(Entity owner)
        {
            _owner = owner;
        }

        public abstract bool CastDamage(DamageData damage, Vector3 castPosition,  Vector2 knockBack);
        
        public bool CanCounter { get; set; }
        public Transform TargetTrm => _owner.transform;
    }
}