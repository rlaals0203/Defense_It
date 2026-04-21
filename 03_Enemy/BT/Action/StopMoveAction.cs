using System;
using _01_Script._03_Enemy;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace Blade.Enemies.BT.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "StopMove", story: "Set [Movement] isStop to [newValue]", category: "Enemy/Move", id: "590c64b68ff5ce9424799727b5b75437")]
    public partial class StopMoveAction : Action
    {
        [SerializeReference] public BlackboardVariable<NavMovement> Movement;
        [SerializeReference] public BlackboardVariable<bool> NewValue;

        protected override Status OnStart()
        {
            Movement.Value.SetStop(NewValue.Value);
            return Status.Success;
        }
    }
}

