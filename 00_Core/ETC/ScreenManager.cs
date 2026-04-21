using System;
using _01_Script._00_Core.EventChannel;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _01_Script._00_Core.ETC
{
    public class ScreenManager : MonoBehaviour
    {
        [SerializeField] private Image fadeImage;
        [SerializeField] private GameEventChannelSO uiChannel;
        private readonly int _circleSizeHash = Shader.PropertyToID("_CircleSize");

        private void Awake()
        {
            fadeImage.material = new Material(fadeImage.material);
            uiChannel.AddListener<FadeEvent>(HandleFadeScreen);
        }

        private void HandleFadeScreen(FadeEvent channel)
        {
            float fadeValue = channel.isFadeIn ? 2f : 0;
            float startValue = channel.isFadeIn ? 0f : 2f;
            
            fadeImage.material.SetFloat(_circleSizeHash, startValue);
            
            fadeImage.material.DOFloat(fadeValue, _circleSizeHash, channel.fadeTime).OnComplete(() =>
            {
                uiChannel.RaiseEvent(UIGameEventChannel.FadeCompleteEvent);
            });

        }
    }
}