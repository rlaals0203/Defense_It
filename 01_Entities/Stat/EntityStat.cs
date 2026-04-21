using System.Collections.Generic;
using System.Linq;
using _01_Script._00_Core.StatSystem;
using _01_Script.Entities;
using UnityEngine;

namespace _01_Script._01_Entities.Stat
{
    public class EntityStat : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private StatOverride[] statOverrides;
        private Dictionary<string, StatSO> _stats;
        
        public Entity Owner { get; private set; }
        public void Initialize(Entity entity)
        {
            Owner = entity;
            _stats = statOverrides.ToDictionary(so => so.StatName, so => so.CreateStat());
        }

        public StatSO GetStat(StatSO stat)
        {
            Debug.Assert(stat != null, "Finding stat cannot be null");
            return _stats.GetValueOrDefault(stat.statName);
        }

        public bool TryGetStat(StatSO stat, out StatSO outStat)
        {
            Debug.Assert(stat != null, "Finding stat cannot be null");

            outStat = _stats.GetValueOrDefault(stat.statName);
            return outStat != null;
        }

        public void SetBaseValue(StatSO stat, float value)
            => GetStat(stat).BaseValue = value;

        public float GetBaseValue(StatSO stat)
            => GetStat(stat).BaseValue;

        public void IncreaseBaseValue(StatSO stat, float value)
            => GetStat(stat).BaseValue += value;

        public void AddModifier(StatSO stat, float value)
            => GetStat(stat).AddModifier(value);

        public void RemoveModifer(StatSO stat, object key)
            => GetStat(stat).RemoveModifier(key);

        public void ClearAllStatModifier()
            => _stats.Values.ToList().ForEach(s => s.ClearModifier());

        public float SubscribeStat(StatSO stat, StatSO.ValueChangedHanlder handler, float defaultValue)
        {
            StatSO target = GetStat(stat);
            if (target == null) return defaultValue;
            target.OnValueChanged += handler;
            return target.Value;
        }

        public void UnSubscribeStat(StatSO stat, StatSO.ValueChangedHanlder handler)
        {
            StatSO target = GetStat(stat);
            if (target == null) return;
            target.OnValueChanged -= handler;
        }
    }
}