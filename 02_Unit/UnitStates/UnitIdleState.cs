using _01_Script._03_Enemy;
using _01_Script.Entities;
using UnityEngine;

namespace _01_Script._02_Unit.UnitStates
{
    public class UnitIdleState : UnitState
    {
        private CheckEnemyInRange _enemyCheckCompo;
        private UnitAttackComponent _unitAttackCompo;
        
        private float _startTime;

        public UnitIdleState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _enemyCheckCompo = Unit.GetCompo<CheckEnemyInRange>();
            _unitAttackCompo = Unit.GetCompo<UnitAttackComponent>();
        }

        public override void Enter()
        {
            base.Enter();

            _enemyCheckCompo.OnEnemyFind += HandleEnemyFind;
            _startTime = Time.time;
        }

        public override void Exit()
        {
            base.Exit();
            _enemyCheckCompo.OnEnemyFind -= HandleEnemyFind;
        }

        public override void Update()
        {
            base.Update();
            
            if (Time.time - _startTime > _unitAttackCompo.UnitCoolTime && Unit.Target is not null)
            {
                Unit.ChangeState("ATTACK");
            }
        }
        
        private void HandleEnemyFind(Entity entity)
        {
            Unit.Target = entity;
        }
    }
}
