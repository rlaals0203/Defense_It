using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;

namespace _01_Script._03_Enemy.BT.Action
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "EnemyMovement", story: "[self] move to destination", category: "Action", id: "469723aa174ae0fdea70b15c3f958e0f")]
    public partial class EnemyMovementAction : Unity.Behavior.Action
    {
        [SerializeReference] public BlackboardVariable<Enemy> Self;

        private NavMovement _movement;

        protected override Status OnStart()
        {
            Initialize();
            _movement.SetDestination(Self.Value.DestinationFinder.Destination.position);
            
            return Status.Running;
        }

        private void Initialize()
        {
            if (_movement == null)
                _movement = Self.Value.GetCompo<NavMovement>();
        }
    }
}

