using _01_Script._01_Entities.Stat;
using _01_Script._03_Enemy;
using _01_Script._100_Misc.Destination;
using _01_Script.Entities;
using MinLibrary.Dependencies;
using MinLibrary.ObjectPool.Runtime;
using UnityEngine;
using UnityEngine.AI;

namespace _01_Script._02_Unit
{
    public class PoolingEntity : MonoBehaviour, IPoolable
    {
        [Inject] DestinationFinder _destinationFinder;
        
        [field:SerializeField] public PoolItemSO PoolItem { get; private set; }
        public Entity Entity { get; private set; }
        public GameObject GameObject => gameObject;
        
        public void SetUpPool(Pool pool)
        {
            Entity = GetComponent<Entity>();
        }

        public void ResetItem()
        {
            if (Entity is Unit unit)
            {
                unit.Outline.enabled = false;
                unit.GetCompo<UnitUpgrade>().ResetUpgrade();
                unit.SetLevel(0);
            }
        }
    }
}