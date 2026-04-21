using MinLibrary.Dependencies;
using MinLibrary.ObjectPool.Runtime;
using UnityEngine;

namespace _01_Script._100_Misc.Effects
{
    public class PoolingEffect : MonoBehaviour, IPoolable
    {
        [field:SerializeField] public PoolItemSO PoolItem { get; private set; }
        public GameObject GameObject => gameObject;
        [SerializeField] private GameObject effectObject;

        private Pool _myPool;
        private IPlayableVFX _playableVFX;
        public void SetUpPool(Pool pool)
        {
            Injector.InjectTo(this);
            _myPool = pool;
            _playableVFX = effectObject.GetComponent<IPlayableVFX>();
            Debug.Assert(_playableVFX != null, $"effect object must have IPlayableVFX component.");
        }

        public void ResetItem()
        {
            _playableVFX.StopVFX();
        }

        public void PlayVFX(Vector3 position, Quaternion rotation)
        {
            _playableVFX.PlayVFX(position, rotation);
        }

        private void OnValidate()   
        {
            if (effectObject == null) return;
            _playableVFX = effectObject.GetComponent<IPlayableVFX>();
            if (_playableVFX == null)
            {
                effectObject = null;
                Debug.LogError($"effect object must have IPlayableVFX component.");
            }
        }
    }
}