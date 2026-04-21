using System;
using _01_Script._00_Core.EventChannel;
using _01_Script.Core.ETC;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _01_Script._06_UI
{
    public class SettingUI : MonoBehaviour
    {
        [SerializeField] private PlayerInputSO playerInput;
        [SerializeField] private Image background;
        [SerializeField] private GameObject gameOverUI;

        private RectTransform _rect;
        private bool _isActive;

        private void Awake()
        {
            _rect = gameOverUI.GetComponent<RectTransform>();
            playerInput.OnSettingEvent += SettingActive;
            background.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            playerInput.OnSettingEvent -= SettingActive;
        }

        public void SettingActive()
        {
            _isActive = !_isActive;

            if (_isActive)
            {
                background.gameObject.SetActive(true);
                background.DOFade(0.7f, 0.5f);
                _rect.DOAnchorPosY(0f, 0.5f);
            }
            else
            {
                background.DOFade(0f, 0.5f).OnComplete(() =>
                {
                    background.gameObject.SetActive(false);
                });
                _rect.DOAnchorPosY(1000f, 0.5f);
            }
        }
    }
}