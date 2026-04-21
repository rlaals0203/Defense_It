using _01_Script._00_Core.StatSystem;
using _01_Script._01_Entities;
using _01_Script._01_Entities.Stat;
using _01_Script.Entities;
using UnityEngine;

namespace _01_Script._05_GameSystem
{
    public class ChestItem : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private StatSO hpStat;
        
        private EntityStat _statCompo;
        private float _maxHp;
        private Entity _owner;
        
        public float AdditionalHp { get; set; }
        
        public void Initialize(Entity entity)
        {
            _owner = entity;
            _statCompo = entity.GetCompo<EntityStat>();
            entity.OnDeadEvent.AddListener(HandleChestDead);
        }

        private void OnDestroy()
        {
            _statCompo.RemoveModifer(hpStat, AdditionalHp);
            _owner.OnDeadEvent.AddListener(HandleChestDead);
        }

        public void Start()
        {
            _statCompo.AddModifier(hpStat, AdditionalHp);
            _maxHp = 100 + AdditionalHp;
        }
        
        private void HandleChestDead()
        {
            int value = Mathf.RoundToInt(UnityEngine.Random.Range(_maxHp * 2f, _maxHp * 3f));
            CurrencyManager.Instance.ModifyCurrency(CurrencyType.Coin,value);
            Destroy(gameObject);
        }
    }
}