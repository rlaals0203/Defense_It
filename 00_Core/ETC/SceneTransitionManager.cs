using System;
using _01_Script._00_Core.EventChannel;
using _01_Script.Entities;
using UnityEngine;

namespace _01_Script._00_Core.ETC
{
    [DefaultExecutionOrder(-20)]
    public class SceneTransitionManager : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO uiChannel;

        private void Start()
        {
            FadeEvent fadeEvt = UIGameEventChannel.FadeEvent;
            fadeEvt.isFadeIn = true;
            fadeEvt.fadeTime = 0.5f;
            
            uiChannel.RaiseEvent(fadeEvt);
        }
    }
}