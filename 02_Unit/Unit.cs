using System;
using _01_Script._00_Core.StatSystem;
using _01_Script._01_Entities;
using _01_Script._01_Entities.Stat;
using _01_Script._02_Unit.Combat;
using _01_Script._03_Enemy;
using _01_Script.Entities;
using Blade.FSM;
using UnityEngine;

namespace _01_Script._02_Unit
{
    public class Unit : Entity
    {
        [SerializeField] private StateDataSO[] states;
        
        [field: SerializeField] public Transform FirePos { get; private set; }
        public Entity Target { get; set; }
        public UnitSO UnitSO => EntitySO as UnitSO;
        
        public int CurrentLevel { get; private set; } = 0;
        public UnitAttackMethod AttackMethod { get; private set; }
        public bool IsSelected 
        {
            get => this;
            set => OnSelectedEvent?.Invoke(value); 
        }
        
        private UnitRangeGizmo _rangeGizmo;
        private EntityStat _statCompo;
        private EntityStateMachine _stateMachine;
        
        public event Action<bool> OnSelectedEvent;

        protected override void Awake()
        {
            base.Awake();
            _stateMachine = new EntityStateMachine(this, states);
            _rangeGizmo = GetCompo<UnitRangeGizmo>();
            _statCompo = GetCompo<EntityStat>();
            
            AttackMethod = UnitAttackMethod.First;
        }

        private void Start()
        {
            _stateMachine.ChangeState("IDLE");
        }
        
        private void Update()
        {
            _stateMachine.UpdateStateMachine();
        }
        
        public void ChangeState(string newStateName) => _stateMachine.ChangeState(newStateName);
        public void ShowRangeGizmo(bool value, float radius = 0f) => _rangeGizmo.ChangeGizmoStatus(value, radius);
        public float GetUnitStatValue(StatSO stat) => _statCompo.GetStat(stat).Value;
        public void SetAttackMethod(UnitAttackMethod method) => AttackMethod = method;
        
        public void SetLevel(int level) => CurrentLevel = level;
        public bool CanMerge() => CurrentLevel == 4 && UnitSO.canMerge;

        public void ResetUnit()
        {
            SetLevel(0);
            transform.position = Vector3.zero;
        }
    }
}
