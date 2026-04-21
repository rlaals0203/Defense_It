using System;
using _01_Script._01_Entities;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;

namespace _01_Script._03_Enemy.BT.Action
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "ChangeClip", story: "[MainAnimator] change [oldClip] to [newClip]",
        category: "Action/Enemy", id: "fd585b8c8a6f54ffcc02f0de8b61dadc")]
    public partial class ChangeClipAction : Unity.Behavior.Action
    {
        [SerializeReference] public BlackboardVariable<EntityAnimator> MainAnimator;
        [SerializeReference] public BlackboardVariable<string> OldClip;
        [SerializeReference] public BlackboardVariable<string> NewClip;

        protected override Status OnStart()
        {
            int oldHash = Animator.StringToHash(OldClip.Value);
            int newHash = Animator.StringToHash(NewClip.Value);
            MainAnimator.Value.SetParam(oldHash, false);
            MainAnimator.Value.SetParam(newHash, true);

            OldClip.Value = NewClip.Value;
            return Status.Success;
        }

        protected override void OnEnd() { }
    }
}

