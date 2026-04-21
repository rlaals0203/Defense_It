using MinLibrary.ObjectPool.Runtime;
using UnityEngine;

namespace _01_Script._01_Entities
{
    [CreateAssetMenu(fileName = "EntitySO", menuName = "SO/Entity", order = 0)]
    public class EntitySO : ScriptableObject
    {
        [Header("Setting")]
        public string entityName;
        public Sprite icon;
        [TextArea] public string description;

        [Header("CombatSetting")]
        public float damageMultiplier = 1f;
        public float damageIncrease = 0;
        
        [Header("OtherSetting")]
        public int price;
        public int[] upgradePrice;
        public PoolItemSO entityPool;
        public GameObject unitGizmo;
    }
}