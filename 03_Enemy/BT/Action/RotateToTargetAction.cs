using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;

namespace _01_Script._03_Enemy.BT.Action
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "RotateToTarget", story: "[Self] rotate to [Target] in [Second]", category: "Eneny/Move", id: "1df3cf2b8632e1254278f971087aecf6")]
    public partial class RotateToTargetAction : Unity.Behavior.Action
    {
        [SerializeReference] public BlackboardVariable<Enemy> Self;
        [SerializeReference] public BlackboardVariable<Transform> Target;
        [SerializeReference] public BlackboardVariable<float> Second;

        private float _startTime;

        protected override Status OnStart()
        {
            _startTime = Time.time;
            return Status.Running;
        }

        protected override Status OnUpdate()
        {
            LookTargetSmoothly();
            if (Time.time - _startTime >= Second.Value)
                return Status.Success;
            return Status.Success;
        }

        private void LookTargetSmoothly()
        {
            float rotationSpeed = 10f;
            Vector3 direction = Target.Value.position - Self.Value.transform.position;
            direction.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
            Quaternion rotation = Quaternion.Slerp(Self.Value.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            
            Self.Value.transform.rotation = rotation;
        }

        protected override void OnEnd() { }
    }
}

