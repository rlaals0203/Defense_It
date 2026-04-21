using _01_Script._00_Core.StatSystem;
using _01_Script._01_Entities;
using _01_Script._01_Entities.Stat;
using _01_Script.Entities;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _01_Script._04_Combat
{
    public class DamageCalcCompo : MonoBehaviour, IEntityComponent, IAfterInitialize
    {
        [SerializeField] private StatSO criticalStat, criticalDamageStat;

        private EntityStat _statCompo;
        private float _critical, _criticalDamage;
        public void Initialize(Entity entity)
        {
            _statCompo = entity.GetCompo<EntityStat>();
        }

        public void AfterInitialize()
        {
            _critical = criticalStat.BaseValue;
            
            if (criticalDamageStat is null)
                _critical = 0;
            else
            {
                _criticalDamage = _statCompo.SubscribeStat(criticalDamageStat, HandleCriticalDamageChange, 0f);
            }
        }
        
        private void OnDestroy()
        {
            if (criticalStat is not null)
                _statCompo.UnSubscribeStat(criticalStat, HandleCriticalChange);
            
            if (criticalStat is not null)
                _statCompo.UnSubscribeStat(criticalDamageStat, HandleCriticalDamageChange);
        }

        private void HandleCriticalDamageChange(StatSO stat, float currentvalue, float prevvalue)
            => _criticalDamage = currentvalue;

        private void HandleCriticalChange(StatSO stat, float currentvalue, float prevvalue)
            => _critical = currentvalue;

        public DamageData CalculateDamge(StatSO majorStat, EntitySO entitySO, float multiplier = 1f)
        {
            DamageData data = new DamageData();
            data.damage = _statCompo.GetStat(majorStat).Value * entitySO.damageMultiplier + 
                          entitySO.damageIncrease * multiplier;
            
            if (Random.value < _critical)
            {
                data.damage *= _criticalDamage;
                data.isCritical = true;
            }
            else
            {
                data.isCritical = false;
            }

            return data;
        }
    }
}