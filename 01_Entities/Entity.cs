using System;
using System.Collections.Generic;
using System.Linq;
using _01_Script._01_Entities;
using UnityEngine;
using UnityEngine.Events;

namespace _01_Script.Entities
{
    public abstract class Entity : MonoBehaviour
    {
        protected Dictionary<Type, IEntityComponent> _components;
        [field:SerializeField] protected EntitySO EntitySO { get; private set; }
        public Outline Outline { get; private set; }
        
        public UnityEvent OnHitEvent;
        public UnityEvent OnDeadEvent;

        protected virtual void Awake()
        {
            _components = new Dictionary<Type, IEntityComponent>();
            Outline = GetComponent<Outline>();
            
            AddComponents();
            InitializeComponents();
            AfterInitialize();
        }

        protected virtual void AddComponents()
        {
            GetComponentsInChildren<IEntityComponent>().ToList()
                .ForEach(component => _components.Add(component.GetType(), component));
        }

        protected virtual void InitializeComponents()
        {
            _components.Values.ToList().ForEach(component => component.Initialize(this));
        }

        protected virtual void AfterInitialize()
        {
            _components.Values.OfType<IAfterInitialize>().ToList()
                .ForEach(compo => compo.AfterInitialize());
        }

        public T GetCompo<T>() where T : IEntityComponent
            => (T)_components.GetValueOrDefault(typeof(T));
        
        public IEntityComponent GetCompo(Type type)
            => _components.GetValueOrDefault(type);

        public void SetOutline(bool isActive, Color color = default)
        {
            Outline.enabled = isActive;
            Outline.OutlineColor = color;
        }
    }
}