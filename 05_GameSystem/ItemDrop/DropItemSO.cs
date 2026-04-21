using MinLibrary.ObjectPool.Runtime;
using UnityEngine;

namespace _01_Script._05_GameSystem.ItemDrop
{
    [CreateAssetMenu(fileName = "ItemSO", menuName = "SO/Item", order = 0)]
    public class DropItemSO : ScriptableObject
    {
        public int rarity;
        public int maxCoin;
        public int minCoin;
        public PoolItemSO poolItem;
    }
}