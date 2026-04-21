using UnityEngine;
using System;
using _01_Script._04_Combat;

namespace _01_Script._02_Unit.Combat.Projectile
{
    public class Projectile : MonoBehaviour, IProjectilable
    {
        [SerializeField] private LayerMask whatIsEnemy;
        private Rigidbody _rigidCompo;
        private Vector3 _direction;
        private float _speed;

        public event Action<HitData> OnProjectileHit;

        private void Awake()
        {
            _rigidCompo = GetComponent<Rigidbody>();
        }
        
        public void InitProjectile(Vector3 startPos, Vector3 dir, float speed)
        {
            _direction = dir;
            _speed = speed;
            
            transform.position = startPos;
            transform.rotation = Quaternion.LookRotation(dir);
        }

        private void FixedUpdate()
        {
            MoveProjectile();
        }

        private void MoveProjectile()
        {
            _rigidCompo.linearVelocity = _direction * _speed;
        }

        private void OnCollisionEnter(Collision other)
        {
            HitData data = new HitData();
            data.hitPoint = other.contacts[0].point;
            data.hitNormal = other.contacts[0].normal * -1f;

            if (other.collider.TryGetComponent(out IDamageable damageable))
            {
                OnProjectileHit?.Invoke(data);
                return;
            }
            
            OnProjectileHit?.Invoke(data);
        }
        
        public void ClearSubscribers() => OnProjectileHit = null;
    }
}
