using System;
using _01_Script._00_Core.StatSystem;
using _01_Script._01_Entities;
using _01_Script._01_Entities.Stat;
using _01_Script._02_Unit.Combat.Projectile;
using _01_Script._04_Combat;
using _01_Script.Entities;
using Ami.BroAudio;
using UnityEngine;

namespace _01_Script._02_Unit
{
    public class UnitAttackComponent : MonoBehaviour, IEntityComponent, IAfterInitialize
    {
        [SerializeField] private float projectileSpeed = 25f;
        [SerializeField] private StatSO damageStat;
        [SerializeField] private StatSO coolTimeStat;
        [SerializeField] private SoundID shootSound;

        private UnitProjectile _projectileCompo;
        private DamageCalcCompo _damageCompo;
        private EntityStat _statCompo;
        private Unit _unit;

        public float UnitCoolTime { get; private set; }

        public void Initialize(Entity entity)
        {
            _unit = entity as Unit;
            _projectileCompo = _unit.GetCompo<UnitProjectile>();
            _damageCompo = _unit.GetCompo<DamageCalcCompo>();
            _statCompo = _unit.GetCompo<EntityStat>();
        }
        
        public void AfterInitialize()
        {
            _projectileCompo.OnHitEvent += HandleProjectileHit;
            UnitCoolTime = _statCompo.SubscribeStat(coolTimeStat, HandleCooltimeChange, 0f);
        }

        private void OnDestroy()
        {
            _projectileCompo.OnHitEvent -= HandleProjectileHit;
            _statCompo.UnSubscribeStat(coolTimeStat, HandleCooltimeChange);
        }

        private void HandleCooltimeChange(StatSO stat, float currentvalue, float prevvalue)
        {
            UnitCoolTime = currentvalue;
        }

        private void HandleProjectileHit(HitData hitData)
        {
            if (_unit.Target == null) return;
            
            if (_unit.Target.TryGetComponent(out IDamageable damageable))
            {
                DamageData data = _damageCompo.CalculateDamge(damageStat, _unit.UnitSO);
                data.hitPoint = hitData.hitPoint;
                data.hitNormal = hitData.hitNormal;
                damageable.ApplyDamage(data, hitData.hitNormal * -1, _unit);
            }
        }

        public void AttackToEnemy(Vector3 start, Vector3 dir)
        {
            _projectileCompo.FireProjectile(start, dir, projectileSpeed);

            if (shootSound.IsValid())
                BroAudio.Play(shootSound);
        }
    }
}
