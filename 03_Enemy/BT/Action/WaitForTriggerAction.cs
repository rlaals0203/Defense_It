using System;
using _01_Script.Entities;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "WaitForTrigger", story: "Wait for [Trigger] end", category: "Enemy/Animation", id: "29cdb5a13f89c916907e6adb10ce129b")]
public partial class WaitForTriggerAction : Action
{
    [SerializeReference] public BlackboardVariable<EntityAnimatorTrigger> Trigger;

    private bool _isTriggered;

    protected override Status OnStart()
    {
        _isTriggered = false;
        Trigger.Value.OnAnimationEndTrigger += HandleAnimationEnd;
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return _isTriggered ? Status.Success: Status.Running;
    }

    protected override void OnEnd()
    {
        Trigger.Value.OnAnimationEndTrigger -= HandleAnimationEnd;
    }

    private void HandleAnimationEnd() => _isTriggered = true;
}

