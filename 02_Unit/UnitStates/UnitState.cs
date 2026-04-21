using _01_Script._01_Entities.FSM;
using _01_Script.Entities;
using UnityEngine;

namespace _01_Script._02_Unit.UnitStates
{
    public abstract class UnitState : EntityState
    {
        protected Unit Unit;

        public UnitState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            Unit = entity as Unit;
        }
    }
}