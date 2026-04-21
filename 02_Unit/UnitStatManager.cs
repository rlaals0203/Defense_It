using System.Collections.Generic;
using _01_Script._00_Core.EventChannel;
using _01_Script._00_Core.StatSystem;
using _01_Script._01_Entities;
using _01_Script._01_Entities.Stat;
using _01_Script.Entities;
using UnityEngine;

namespace _01_Script._02_Unit
{
    public class UnitStatManager : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private GameEventChannelSO eventChannel;
        private EntityStat _statCompo;
        private Dictionary<string, StatSO> _modifyStats;

        public void Initialize(Entity entity)
        {
            _statCompo = entity.GetCompo<EntityStat>();
            _modifyStats = new Dictionary<string, StatSO>();
        }

        public void ModifyUnitStat(StatSO stat, object key, float value)
        {
            if (_modifyStats.ContainsKey(key.ToString())) return;
            
            _modifyStats.TryAdd(key.ToString(), stat);
            _statCompo.AddModifier(stat, value);
        }
        
        public void RemoveUnitStat(StatSO stat, object key) => _statCompo.RemoveModifer(stat, key);
    }
}