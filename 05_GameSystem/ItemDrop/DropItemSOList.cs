using System.Collections.Generic;
using UnityEngine;

namespace _01_Script._05_GameSystem.ItemDrop
{
    [CreateAssetMenu(fileName = "DropItemSOList", menuName = "SO/DropItemSOList", order = 0)]
    public class DropItemSOList : ScriptableObject
    {
        public List<DropItemSO> dropItems = new List<DropItemSO>();
    }
}