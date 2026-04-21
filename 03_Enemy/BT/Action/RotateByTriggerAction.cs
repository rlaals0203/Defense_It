using System;
using _01_Script.Entities;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;

namespace _01_Script._03_Enemy.BT.Action
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "RotateByTrigger", story: "[Movement] rotate to [Target] by [Trigger]", category: "Enemy/Move", id: "ff68d46db2ec21dc503c646990e22168")]
    public partial class RotateByTriggerAction : Unity.Behavior.Action
    {
        [SerializeReference] public BlackboardVariable<NavMovement> Movement;
        [SerializeReference] public BlackboardVariable<Transform> Target;
        [SerializeReference] public BlackboardVariable<EntityAnimatorTrigger> Trigger;

        private bool _isRotate;
    
        protected override Status OnStart()
        {
            _isRotate = false;
            Trigger.Value.OnManualRotationTrigger += HandleManualRotation;
            return Status.Running;
        }

        protected override Status OnUpdate()
        {
            if(_isRotate)
                Movement.Value.LookAtTarget(Target.Value.position);
            return Status.Running;
        }

        protected override void OnEnd()
        {
            Trigger.Value.OnManualRotationTrigger -= HandleManualRotation;
        }
    
        private void HandleManualRotation(bool isRotate) => _isRotate = isRotate;
    }
}

