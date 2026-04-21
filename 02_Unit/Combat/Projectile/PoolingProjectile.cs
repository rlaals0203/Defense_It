using _01_Script._100_Misc.Effects;
using MinLibrary.Dependencies;
using MinLibrary.ObjectPool.Runtime;
using UnityEngine;

namespace _01_Script._02_Unit.Combat.Projectile
{
    public class PoolingProjectile : MonoBehaviour, IPoolable
    {
        [field:SerializeField] public PoolItemSO PoolItem { get; private set; }
        public GameObject GameObject => gameObject;

        public Projectile projectile;

        private Pool _pool;
        
        public void SetUpPool(Pool pool)
        {
            _pool = pool;
        }

        public void ResetItem()
        {
        }

        public Projectile GetProjectile() => projectile;
    }
}