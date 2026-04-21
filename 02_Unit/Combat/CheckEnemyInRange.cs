using System;
using System.Collections.Generic;
using System.Linq;
using _01_Script._00_Core.StatSystem;
using _01_Script._01_Entities;
using _01_Script._01_Entities.Stat;
using _01_Script._02_Unit.Combat;
using _01_Script._03_Enemy;
using _01_Script._04_Combat;
using _01_Script._05_GameSystem.Chest;
using _01_Script._100_Misc;
using _01_Script.Entities;
using UnityEngine;

namespace _01_Script._02_Unit
{
    public class CheckEnemyInRange : MonoBehaviour, IEntityComponent, IAfterInitialize
    {
        [SerializeField] protected LayerMask whatIsTarget;
        [SerializeField] private DestinationFinderSO destinationFinder;
        [SerializeField] private StatSO rangeStat;
        
        private float _range;
        private float _time;
        private Collider[] _targets = new Collider[32];
        private EntityStat _statCompo;
        private Unit _unit;
        
        private readonly List<Entity> _buffer = new(32);
        private readonly float _frequency = 0.1f;
        
        public event Action<Entity> OnEnemyFind;
        
        public void Initialize(Entity entity)
        {
            _unit = entity as Unit;
            _statCompo = entity.GetCompo<EntityStat>();
        }
        
        public void AfterInitialize()
        {
            _range = _statCompo.SubscribeStat(rangeStat, HandleRangeChange, rangeStat.Value);
        }

        private void OnDestroy()
        {
            _statCompo.UnSubscribeStat(rangeStat, HandleRangeChange);
        }

        private void Update()
        {
            if (Time.time - _time >= _frequency)
            {
                FindEnemyInRange(_unit.AttackMethod);
                _time = Time.time;
            }
        }
        
        private void HandleRangeChange(StatSO stat, float currentvalue, float prevvalue)
        {
            _range = currentvalue;
        }
        
        private void CacheEntities(int count)
        {
            _buffer.Clear();
            for (int i = 0; i < count; i++)
            {
                if (_targets[i].TryGetComponent(out Entity entity))
                {
                    _buffer.Add(entity);
                }
            }
        }

        public void FindEnemyInRange(UnitAttackMethod attackMethod)
        {
            int count = Physics.OverlapSphereNonAlloc(_unit.transform.position, _range / 2f, _targets, whatIsTarget);
            Entity target = null;

            if (count > 0)
            {
                CacheEntities(count);
                target = attackMethod switch {
                    UnitAttackMethod.First => FindFirst(),
                    UnitAttackMethod.Last => FindLast(),
                    UnitAttackMethod.Strongest => FindStrongest(),
                    _ => null
                };
            }

            OnEnemyFind?.Invoke(target);
        }

        private Entity FindFirst()
        {
            return _buffer
                .OrderBy(e => e is Chest ? float.MinValue : GetRemainDistance(e))
                .FirstOrDefault();
        }

        private Entity FindLast()
        {
            return _buffer
                .OrderBy(e => e is Chest ? float.MinValue : GetRemainDistance(e))
                .LastOrDefault();
        }

        private Entity FindStrongest()
        {
            return _buffer
                .OrderByDescending(e => e is Chest ? float.MinValue : GetRemainHealth(e))
                .FirstOrDefault();
        }

        private float GetRemainHealth(Entity entity)
        {
            var healthCompo = entity.GetCompo<EntityHealth>();
            return healthCompo != null ? healthCompo.CurrentHealth : 0;
        }

        private float GetRemainDistance(Entity entity)
        {
            var movement = entity.GetCompo<NavMovement>();
            return movement != null ? movement.RemainDistance : float.MaxValue;
        }
    }
}