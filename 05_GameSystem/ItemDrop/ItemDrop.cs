using System;
using System.Collections;
using System.Linq;
using _01_Script._00_Core.EventChannel;
using MinLibrary.ObjectPool.Runtime;
using UnityEngine;

namespace _01_Script._05_GameSystem.ItemDrop
{
    public class ItemDrop : MonoBehaviour, IDropable, IPoolable
    {
        [SerializeField] private GameEventChannelSO eventChannel;
        [SerializeField] private float dropDuration = 1.0f;
        [SerializeField] private float arcHeight = 2.0f;
        
        private bool _isAutoMode;
        private PickUpItem _pickUpItem;
        private Pool _pool;

        [field:SerializeField] public PoolItemSO PoolItem { get; private set; }
        public DropItemSO DropItemSO { get; set; }
        public GameObject GameObject => gameObject;

        private void Awake()
        {
            eventChannel.AddListener<AutoCoinCollectEvent>(HandleAutoCollect);
        }
        
        private void OnDestroy()
        {
            eventChannel.RemoveListener<AutoCoinCollectEvent>(HandleAutoCollect);
        }
        
        private void HandleAutoCollect(AutoCoinCollectEvent channel) => _isAutoMode = true;

        public void DropItem(Vector3 position, Vector3 destination)
        {
            StartCoroutine(ParabolaMove(position, destination, dropDuration, arcHeight));
        }

        public void PickupItem(Action callback = null)
        {
            callback?.Invoke();
            _pool.Push(this);
        }

        private IEnumerator ParabolaMove(Vector3 start, Vector3 end, float duration, float height)
        {
            float time = 0f;
            
            while (time < duration)
            {
                float t = time / duration;
                Vector3 pos = Vector3.Lerp(start, end, t);
                pos.y += height * 4f * t * (1f - t);

                transform.position = pos;
                time += Time.deltaTime;
                yield return null;
            }

            transform.position = end;

            if (_isAutoMode)
            {
                if (_pickUpItem == null)
                    _pickUpItem = FindObjectsByType<PickUpItem>(FindObjectsSortMode.None).First();
                
                _pickUpItem.PickUp(this, end);
            }
        }

        public void SetUpPool(Pool pool)
        {
            _pool = pool;
        }
        
        public void ResetItem() { }
    }
}