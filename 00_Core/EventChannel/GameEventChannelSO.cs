using System;
using System.Collections.Generic;
using UnityEngine;

namespace _01_Script._00_Core.EventChannel
{
    public abstract class GameEvent { }

    [CreateAssetMenu(fileName = "GameEventChannelSO", menuName = "SO/Event/GameEventChannel")]
    public class GameEventChannelSO : ScriptableObject
    {
        public Dictionary<Type, Action<GameEvent>> _events = new Dictionary<Type, Action<GameEvent>>();
        public Dictionary<Delegate, Action<GameEvent>> _lokcUpTalbe = new Dictionary<Delegate, Action<GameEvent>>();

        public void AddListener<T>(Action<T> hender) where T : GameEvent
        {
            if (_lokcUpTalbe.ContainsKey((hender)) == true) return;

            Action<GameEvent> castHander = evt => hender.Invoke(evt as T);
            _lokcUpTalbe[hender] = castHander;

            Type eventType = typeof(T);

            if (_events.ContainsKey(eventType))
                _events[eventType] += castHander;
            else
                _events[eventType] = castHander;
        }

        public void RemoveListener<T>(Action<T> hender) where T : GameEvent
        {
            Type evtType = typeof(T);
            if (_lokcUpTalbe.TryGetValue(hender, out Action<GameEvent> casthender))
            {
                if (_events.TryGetValue(evtType, out Action<GameEvent> internalHandler))
                {
                    internalHandler -= casthender;
                    if (internalHandler == null)
                        _events.Remove(evtType);
                    else
                    {
                        _events[evtType] = internalHandler;
                    }
                    _lokcUpTalbe.Remove(hender);
                }
            }
        }

        public void RaiseEvent(GameEvent evt)
        {
            if (_events.TryGetValue(evt.GetType(), out Action<GameEvent> casthender))
            {
                casthender.Invoke(evt);
            }
        }

        public void Clear()
        {
            _events.Clear();
            _lokcUpTalbe.Clear();
        }
    }
}