using _01_Script._00_Core.ETC;
using _01_Script._100_Misc.Effects;
using MinLibrary.Dependencies;
using MinLibrary.ObjectPool.Runtime;
using Unity.Mathematics;
using UnityEngine;

namespace _01_Script._02_Unit
{
    public class EffectFeedback : MonoBehaviour
    {
        [Inject] private PoolManagerMono _poolManager;
        [SerializeField] private PoolItemSO upgradeEffect;
        [SerializeField] private Transform playerPosition;
        
        public async void CreateFeedback(Vector3 pos = default)
        {
            if (_poolManager == null)
            {
                Injector.InjectTo(this);
            }
            
            if(pos == default)
                pos = playerPosition.position;
            
            PoolingEffect effect = _poolManager.Pop<PoolingEffect>(upgradeEffect);
            effect.PlayVFX(pos, quaternion.identity);

            await Awaitable.WaitForSecondsAsync(2f);
            _poolManager.Push(effect);
        }

        public void StopFeedback() { }
    }
}