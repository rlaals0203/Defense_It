using _01_Script._00_Core.EventChannel;
using _01_Script._02_Unit;
using _01_Script._03_Enemy;
using _01_Script._04_Combat;
using MinLibrary.Dependencies;
using MinLibrary.ObjectPool.Runtime;
using UnityEngine;

namespace _01_Script._05_GameSystem
{
    public class Tower : MonoBehaviour
    {
        public float health = 500f;

        [SerializeField] private GameEventChannelSO eventChannel;
        [Inject] private PoolManagerMono _poolManager; 

        private float _time;
        private int _healthRegen;
        private PoolingEntity _poolingEntity;
        private EntityHealth _healthCompo;
        
        private readonly TowerAttackEvent _towerAttackEvent = GameSystemEventChannel.TowerAttackEvent;
        private readonly GameOverEvent _gameOverEvent = GameSystemEventChannel.GameOverEvent;

        private void Awake()
        {
            eventChannel.AddListener<TowerHealthEvent>(HandleTowerHealthEvent);
            _time = Time.time;
        }

        private void Update()
        {
            if (Time.time - _time >= 1)
            {
                _time = Time.time;
                health += _healthRegen;
            }
        }

        private void HandleTowerHealthEvent(TowerHealthEvent obj)
        {
            _healthRegen++;
        }

        public void ApplyDamage(float damage)
        {
            health -= damage;
            eventChannel.RaiseEvent(_towerAttackEvent.Initializer(health));

            if (health <= 0)
            {
                health = 0;
                eventChannel.RaiseEvent(_gameOverEvent);
                Time.timeScale = 1f;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Enemy enemy))
            {
                _healthCompo = enemy.GetCompo<EntityHealth>();
                _poolingEntity = enemy.GetComponent<PoolingEntity>();
                ApplyDamage(_healthCompo.CurrentHealth);
                
                if (_poolManager == null)
                    Injector.InjectTo(this);
                
                _poolManager.Push(_poolingEntity);
            }
        }
    }
}