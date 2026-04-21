using _01_Script._04_Combat;
using _01_Script.Entities;
using UnityEngine;

namespace _01_Script._02_Unit.Combat
{
    public class OverlapDamageCaster : DamageCaster
    {
        public enum OverlapCastType
        {
            Sphere, Box
        }
        [SerializeField] protected OverlapCastType overlapCastType;
        [SerializeField] private Vector3 damageBoxSize;
        [SerializeField] private float damageRadius;

        private Collider[] _hitResults;

        public override void InitCaster(Entity owner)
        {
            base.InitCaster(owner);
            _hitResults = new Collider[maxHitCount];
        }

        public override bool CastDamage(DamageData damage, Vector3 castPosition, Vector2 knockBack)
        {
            int cnt = overlapCastType switch
            {
                OverlapCastType.Sphere => Physics.OverlapSphereNonAlloc(castPosition, damageRadius, _hitResults, whatIsTarget),
                OverlapCastType.Box => Physics.OverlapBoxNonAlloc(castPosition, damageBoxSize, 
                    _hitResults, Quaternion.identity, whatIsTarget),
                _ => 0
            };
            
            for (int i = 0; i < cnt; i++)
            {
                Vector2 direction = (_hitResults[i].transform.position - _owner.transform.position).normalized;
                knockBack.x *= Mathf.Sign(direction.x);
                
                if (_hitResults[i].TryGetComponent(out IDamageable damageable))
                {
                    damageable.ApplyDamage(damage, direction, _owner);
                }
            }
            
            return cnt > 0;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(0.7f, 0.7f, 0, 1f);
            switch(overlapCastType)
            {
                case OverlapCastType.Sphere:
                    Gizmos.DrawWireSphere(transform.position, damageRadius);
                    break;
                case OverlapCastType.Box:
                    Gizmos.DrawWireCube(transform.position, damageBoxSize);
                    break;
            };
        }
#endif
    }
}