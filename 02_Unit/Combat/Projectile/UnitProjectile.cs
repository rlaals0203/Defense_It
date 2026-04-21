using System;
using System.Linq;
using _01_Script._00_Core.EventChannel;
using _01_Script._01_Entities;
using _01_Script._100_Misc.Effects;
using _01_Script.Entities;
using MinLibrary.Dependencies;
using MinLibrary.ObjectPool.Runtime;
using UnityEngine;

namespace _01_Script._02_Unit.Combat.Projectile
{
    public class UnitProjectile : MonoBehaviour, IEntityComponent
    {
        [Inject] private PoolManagerMono _poolManager;
        [SerializeField] private GameEventChannelSO eventChannel;

        public event Action<HitData> OnHitEvent; 

        private Unit _unit;
        private bool _isActve = true;

        public void Initialize(Entity entity)
        {
            _unit = entity as Unit;

            if (_poolManager == null)
            {
                _poolManager = FindObjectsByType<PoolManagerMono>(FindObjectsSortMode.None).FirstOrDefault();
            }
            
            eventChannel.AddListener<ChangeSFXEvent>(HandleSFXChange);
        }

        private void HandleSFXChange(ChangeSFXEvent channel)
        {
            channel.isActive = !channel.isActive;
            _isActve = channel.isActive;
        }

        public void FireProjectile(Vector3 startPos, Vector3 dir, float speed)
        {
            var poolingProjectile = _poolManager.Pop<PoolingProjectile>(_unit.UnitSO.projectilePool);
            var projectile = poolingProjectile.GetProjectile();
            projectile.OnProjectileHit += hitData => HandleProjectileHit(hitData, poolingProjectile, projectile);
            projectile.InitProjectile(startPos, dir, speed);

            if (_isActve)
            {
                PlayVFX(_unit.UnitSO.flashPool, startPos, Quaternion.identity);
            }
        }
        
        private void  HandleProjectileHit(HitData data, PoolingProjectile poolingProjectile, Projectile projectile)
        {
            if (_isActve)
            {
                PlayVFX(_unit.UnitSO.hitEffectPool, data.hitPoint, Quaternion.LookRotation(data.hitNormal));
            }
            
            projectile.ClearSubscribers();
            _poolManager.Push(poolingProjectile);
            OnHitEvent?.Invoke(data);
        }

        private async void PlayVFX(PoolItemSO effectPool, Vector3 hitPoint, Quaternion rotation)
        {
            PoolingEffect effect = _poolManager.Pop<PoolingEffect>(effectPool);
            effect.PlayVFX(hitPoint, rotation);
            
            await Awaitable.WaitForSecondsAsync(1f);
            _poolManager.Push(effect);
        }

        private void OnDestroy()
        {
            eventChannel.RemoveListener<ChangeSFXEvent>(HandleSFXChange);
        }
    }
}