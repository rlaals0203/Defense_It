using System;
using _01_Script._00_Core.StatSystem;
using _01_Script._01_Entities;
using _01_Script._01_Entities.Stat;
using _01_Script.Entities;
using UnityEngine;

namespace _01_Script._02_Unit
{
    public class BuffUnit : Entity
    {
        private UnitRangeGizmo _ranageGizmo;
        private EntityStat _statCompo;
        
        public int CurrentLevel { get; private set; } = 1;
        public bool IsSelected
        {
            get => this;
            set
            {
                OnSelectedEvent?.Invoke(value);
            }
        }

        public event Action<bool> OnSelectedEvent;
        
        protected override void Awake()
        {
            base.Awake();
            _ranageGizmo = GetCompo<UnitRangeGizmo>();
            _statCompo = GetCompo<EntityStat>();
        }
        
        public void ShowRangeGizmo(bool value, float radius = 0f) =>_ranageGizmo.ChangeGizmoStatus(value, radius);
        public float GetUnitStatValue(StatSO stat) => _statCompo.GetStat(stat).Value;
        public void ModifyStat(StatSO stat, object key, float value) => _statCompo.AddModifier(stat, value);
    }
}