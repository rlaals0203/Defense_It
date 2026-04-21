using System;
using System.Collections.Generic;
using _01_Script._01_Entities;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;

namespace _01_Script._03_Enemy.BT.Action
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "GetComponents", story: "Get Components from [Self]", category: "Enemy", id: "946a0e8cb937c9cc7a305bbe147f49e5")]
    public partial class GetComponentsAction : Unity.Behavior.Action
    {
        [SerializeReference] public BlackboardVariable<Enemy> Self;

        protected override Status OnStart()
        {
            Enemy enemy = Self.Value;

            List<BlackboardVariable> varList = enemy.BtAgent.BlackboardReference.Blackboard.Variables;

            foreach (BlackboardVariable variable in varList)
            {
                if(typeof(IEntityComponent).IsAssignableFrom(variable.Type) == false) continue;
                
                SetComponent(enemy, variable.Name, enemy.GetCompo(variable.Type));
            }
            
            return Status.Success;
        }

        private void SetComponent(Enemy enemy, string varName, IEntityComponent component)
        {
            if (enemy.BtAgent.GetVariable(varName, out BlackboardVariable variable))
            {
                variable.ObjectValue = component;
            }
        }

        private void SetVariable<T>(Enemy enemy, string varName, T component)
        {
            Debug.Assert(component != null, $"Check {varName} in {enemy.name}");
            BlackboardVariable<T> target = enemy.GetBlackboardVariable<T>(varName);
            target.Value = component;
        }
    }
}

