using _01_Script.Entities;
using UnityEngine;

namespace _01_Script._02_Unit.UnitStates
{
    public class UnitAttackState : UnitState
    {
        private UnitAttackComponent _attackCompo;
        public UnitAttackState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _attackCompo = Unit.GetCompo<UnitAttackComponent>();
        }

        public override void Enter()
        {
            base.Enter();

            Debug.Log(Unit.Target);
            if(Unit.Target == null)
                Unit.ChangeState("IDLE");

            Attack();
            Unit.ChangeState("IDLE");
        }

        private void Attack()
        {
            Vector3 pos = Unit.FirePos.position;
            Vector3 targetPos = Unit.Target.transform.position;
            targetPos.y += 1; 
            Vector3 dir = (targetPos - pos).normalized;
            
            _attackCompo.AttackToEnemy(pos, dir);
        }
    }
}
