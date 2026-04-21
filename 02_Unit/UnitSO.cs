using _01_Script._01_Entities;
using MinLibrary.ObjectPool.Runtime;
using UnityEngine;

namespace _01_Script._02_Unit
{
    [CreateAssetMenu(fileName = "UnitSO", menuName = "SO/Unit", order = 0)]
    public class UnitSO : EntitySO
    {
        [Header("OtherSetting")]
        public int rating = 0;
        public int shopPrice;
        public float attackDamage;
        public float attackCooltime;
        public float range;
        public float criticalDamage;
        public bool canMerge;
        public bool isEquipped;
        public bool isOwned;

        
        [Header("EffectSetting")]
        public PoolItemSO upgradeUnitPool;
        public PoolItemSO projectilePool;
        public PoolItemSO flashPool;
        public PoolItemSO hitEffectPool;
    }
}