using System.Linq;
using _01_Script._00_Core.EventChannel;
using _01_Script._00_Core.StatSystem;
using _01_Script._01_Entities;
using _01_Script._01_Entities.Stat;
using _01_Script.Entities;
using Ami.BroAudio;
using MinLibrary.Dependencies;
using MinLibrary.ObjectPool.Runtime;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _01_Script._04_Combat
{
    public class EntityHealth : MonoBehaviour, IEntityComponent, IAfterInitialize, IDamageable
    {
        [SerializeField] private GameEventChannelSO eventChannel;
        [SerializeField] private StatSO hpStat;
        [SerializeField] private PoolItemSO damageTextPool;
        [SerializeField] private SoundID deadSound;
        
        [Inject] private PoolManagerMono _poolManager;
        private Entity _entity;
        private EntityStat _entityStat;
        
        public float CurrentHealth { get; private set; }
        public float MaxHealth { get; private set; }
        
        public void Initialize(Entity entity)
        {
            _entity = entity;
            _entityStat = entity.GetCompo<EntityStat>();
            
            if (_poolManager == null)
            {
                _poolManager = FindObjectsByType<PoolManagerMono>(FindObjectsSortMode.None).FirstOrDefault();
            }
        }
        
        public void AfterInitialize()
        {
            CurrentHealth = MaxHealth = _entityStat.SubscribeStat(hpStat, HandleMaxHpChange, hpStat.Value);
        }

        private void HandleMaxHpChange(StatSO stat, float currentvalue, float prevvalue)
        {
            float changed = currentvalue - prevvalue;
            MaxHealth = currentvalue;

            if (changed > 0)
            {
                CurrentHealth = Mathf.Clamp(CurrentHealth + changed, 0, MaxHealth);
            }
            else
            {
                CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);
            }
        }

        public void ApplyDamage(DamageData damageData, Vector3 direction, Entity dealer)
        {
            CurrentHealth = Mathf.Clamp(CurrentHealth - damageData.damage, 0f, MaxHealth);

            if (CurrentHealth <= 0)
            {
                _entity.OnDeadEvent?.Invoke();
                CurrentHealth = MaxHealth;
            }
            
            EmitDamageText(damageData);
            _entity.OnHitEvent?.Invoke();
        }

        private void EmitDamageText(DamageData damageData)
        {
            PoolingDamageText text = _poolManager.Pop<PoolingDamageText>(damageTextPool);
            Vector3 position = _entity.transform.position;
            position.y = 1f;
            position += Random.insideUnitSphere * 0.3f;
            
            text.SetDamageText(damageData.damage.ToString());
            text.transform.position = position;

            if (damageData.isCritical)
            {
                text.SetDamageText($"{damageData.damage.ToString()}!");
                text.CriticalText();
            }
        }

        private void OnDestroy()
        { 
            _entityStat.UnSubscribeStat(hpStat, HandleMaxHpChange);
        }
    }
}