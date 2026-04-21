using _01_Script._04_Combat;
using _01_Script._100_Misc;
using _01_Script.Entities;
using Ami.BroAudio;
using MinLibrary.ObjectPool.Runtime;
using Unity.Behavior;
using UnityEngine;
using Action = System.Action;

namespace _01_Script._03_Enemy
{
    public class Enemy : Entity, IPoolable
    {
        [SerializeField] private SoundID deadSound;
        private EntityHealth _healthCompo;
        private Pool _pool;
        
        [field: SerializeField] public DestinationFinderSO DestinationFinder { get; set; }
        [field: SerializeField] public PoolItemSO PoolItem { get; private set; }

        public EnemySO EnemySO => EntitySO as EnemySO;
        public BehaviorGraphAgent BtAgent { get; private set; }
        public bool IsActive { get; set; }
        public GameObject GameObject => gameObject;
        
        public Action OnEnemyDead;


        protected override void Awake()
        {
            base.Awake();
            OnDeadEvent.AddListener(HandleDeadEvent);
        }

        private void OnDestroy()
        {
            OnDeadEvent.RemoveListener(HandleDeadEvent);
        }

        private void HandleDeadEvent()
        {
            BroAudio.Play(deadSound);
            OnEnemyDead?.Invoke();
            _pool.Push(this);
        }

        protected override void AddComponents()
        {
            base.AddComponents();
            BtAgent = GetComponent<BehaviorGraphAgent>();
            Debug.Assert(BtAgent != null, $"{gameObject.name} don't have BehaviorGraphAgent");
        }

        private void OnEnable()
        {
            BtAgent.Restart();
        }

        public BlackboardVariable<T> GetBlackboardVariable<T>(string key)
        {
            if (BtAgent.GetVariable(key, out BlackboardVariable<T> result))
            {
                return result;
            }

            return default;
        }

        public void SetUpPool(Pool pool)
        {
            _pool = pool;
        }

        public void ResetItem() { }
    }
}
