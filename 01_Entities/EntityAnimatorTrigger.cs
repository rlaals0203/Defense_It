using System;
using _01_Script._01_Entities;
using UnityEngine;

namespace _01_Script.Entities
{
    public class EntityAnimatorTrigger : MonoBehaviour, IEntityComponent
    {
        private Entity _entity;
        
        public Action<bool> OnManualRotationTrigger;
        public Action OnAnimationEndTrigger;
        public Action OnAttackVFXTrigger;

        public void Initialize(Entity entity)
        {
            _entity = entity;
        }
        
        private void AnimationEnd() => OnAnimationEndTrigger?.Invoke();
        private void PlayAttackVFX() => OnAttackVFXTrigger?.Invoke();
        private void StopManualRotation() => OnManualRotationTrigger?.Invoke(false);
    }
}