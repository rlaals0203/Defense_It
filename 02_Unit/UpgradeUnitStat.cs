using System;
using _01_Script._00_Core.StatSystem;
using _01_Script._01_Entities.Stat;
using UnityEngine;

namespace _01_Script._02_Unit
{
    [Serializable]
    public class UpgradeStat
    {
        public int requireLevel;
        
        [SerializeField] private StatSO targetStat;
        [SerializeField] private float value;

        public void Upgrade(EntityStat statCompo)
        {
            statCompo.AddModifier(targetStat, value);
        }
        
        public void ResetUpgrade(EntityStat statCompo)
        {
            statCompo.ClearAllStatModifier();
        }
    }
}