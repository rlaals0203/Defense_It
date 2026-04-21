using System.Linq;
using _01_Script._00_Core.ETC;
using MinLibrary.Dependencies;
using MinLibrary.ObjectPool.Runtime;
using UnityEngine;

namespace _01_Script._05_GameSystem.ItemDrop
{
    public class ItemDropFeedback : Feedback
    {
        [Inject] private PoolManagerMono poolManager;
        
        [SerializeField] private float dropRange;
        [SerializeField] private DropItemSOList itemList;
        [SerializeField] private LayerMask whatisUnit;
        [SerializeField] private LayerMask whatIsConveyer;

        private void Start()
        {
            if (poolManager == null)
                Injector.InjectTo(this);
        }

        public override void CreateFeedback()
        {
            InitParabola();
        }

        public override void StopFeedback() { }

        private void InitParabola()
        {
            float random = Random.value;
            
            Vector3 center = transform.position;
            Vector3 offset = Random.insideUnitSphere * dropRange;
            Vector3 pos = center + offset;

            if (CheckEmpty(pos, whatisUnit) || CheckEmpty(pos, whatisUnit)) {
                InitParabola();
                return;
            }
            
            if (random > 0.3f) return;
            pos.y = 0.5f;

            DropItemSO itemSO = SelectItem();
            ItemDrop itemDrop = poolManager.Pop<ItemDrop>(itemSO.poolItem);
            itemDrop.DropItemSO = itemSO;
            itemDrop.DropItem(center, pos);
        }
        
        private bool IsPicked(float rarity)
        {
            return Random.Range(1, (int)rarity) == 1;
        }
        
        private DropItemSO SelectItem()
        {
            foreach (DropItemSO item in itemList.dropItems)
            {
                if (IsPicked(item.rarity / 1f))
                {
                    return item;
                }
            }

            return itemList.dropItems.Last();
        }
        
        private bool CheckEmpty(Vector3 position, LayerMask whatisTarget)
        {
            position.y = 10f;
            return Physics.Raycast(position, -Vector3.up,Mathf.Infinity, whatisTarget);
        }
    }
}