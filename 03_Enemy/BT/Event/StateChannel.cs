using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;

namespace _01_Script._03_Enemy.BT.Event
{
#if UNITY_EDITOR
    [CreateAssetMenu(menuName = "Behavior/Event Channels/StateChange")]
#endif
    [Serializable, GeneratePropertyBag]
    [EventChannelDescription(name: "StateChange", message: "state change to [newValue]", category: "Events", id: "29c7d4424b1601a0c10c7fb1ee12ca15")]
    public partial class StateChange : EventChannelBase
    {
        public delegate void StateChangeEventHandler(EnemyState newValue);
        public event StateChangeEventHandler Event; 

        public void SendEventMessage(EnemyState newValue)
        {
            Event?.Invoke(newValue);
        }

        public override void SendEventMessage(BlackboardVariable[] messageData)
        {
            BlackboardVariable<EnemyState> newValueBlackboardVariable = messageData[0] as BlackboardVariable<EnemyState>;
            var newValue = newValueBlackboardVariable != null ? newValueBlackboardVariable.Value : default(EnemyState);

            Event?.Invoke(newValue);
        }

        public override Delegate CreateEventHandler(BlackboardVariable[] vars, System.Action callback)
        {
            StateChangeEventHandler del = (newValue) =>
            {
                BlackboardVariable<EnemyState> var0 = vars[0] as BlackboardVariable<EnemyState>;
                if(var0 != null)
                    var0.Value = newValue;

                callback();
            };
            return del;
        }

        public override void RegisterListener(Delegate del)
        {
            Event += del as StateChangeEventHandler;
        }

        public override void UnregisterListener(Delegate del)
        {
            Event -= del as StateChangeEventHandler;
        }
    }
}