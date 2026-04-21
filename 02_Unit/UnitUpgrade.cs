using System;
using System.Collections.Generic;
using _01_Script._00_Core.EventChannel;
using _01_Script._01_Entities;
using _01_Script._01_Entities.Stat;
using _01_Script.Entities;
using Ami.BroAudio;
using MinLibrary.ObjectPool.Runtime;
using UnityEngine;
using UnityEngine.Events;

namespace _01_Script._02_Unit
{
    public class UnitUpgrade : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private GameEventChannelSO eventChannel;
        [SerializeField] private UpgradeStat[] upgradeStats;
        [SerializeField] private SoundID[] upgradeSounds;
        [SerializeField] private EffectFeedback effectFeedback;
        [SerializeField] private PoolItemSO upgradeEffect;

        private Unit _unit;
        private EntityStat _statCompo;
        private Dictionary<int, List<UpgradeStat>> _upgradeStatDic = new();

        public UnityEvent UpgradeEvent;

        public void Initialize(Entity entity)
        {
            _unit = entity as Unit;
            _statCompo = _unit.GetCompo<EntityStat>();
            UpgradeEvent.AddListener(PlayEffectFeedback);
        }

        private void Awake()
        {
            foreach (var stat in upgradeStats)
            {
                int key = stat.requireLevel;
                if (!_upgradeStatDic.ContainsKey(key))
                {
                    _upgradeStatDic[key] = new List<UpgradeStat>();
                }
                _upgradeStatDic[key].Add(stat);
            }
    
            eventChannel.AddListener<UnitUpgradeEvent>(HandleUnitUpgrade);
        }
        
        private void OnDestroy()
        {
            UpgradeEvent.RemoveListener(PlayEffectFeedback);
            eventChannel.RemoveListener<UnitUpgradeEvent>(HandleUnitUpgrade);
        }


        private void HandleUnitUpgrade(UnitUpgradeEvent channel)
        {
            if (channel.selectedUnit != _unit) return;

            int nextLevel = channel.selectedUnit.CurrentLevel + 1;
            if (_upgradeStatDic.TryGetValue(nextLevel, out var upgradeList))
            {
                foreach (var upgradeStat in upgradeList)
                {
                    upgradeStat.Upgrade(_statCompo);
                }
                
                UpgradeEvent?.Invoke();
                BroAudio.Play(upgradeSounds[channel.selectedUnit.CurrentLevel - 1]);
            }
        }
        
        private void PlayEffectFeedback() => effectFeedback.CreateFeedback();
        public void ResetUpgrade() => _statCompo.ClearAllStatModifier();
    }
}