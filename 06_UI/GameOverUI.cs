using System;
using _01_Script._00_Core.EventChannel;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _01_Script._06_UI
{
    public class GameOverUI : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO eventChannel;
        [SerializeField] private Image background;
        [SerializeField] private GameObject gameOverUI;

        private RectTransform _rect;
        private float _currentWave;

        private void Awake()
        {
            eventChannel.AddListener<GameOverEvent>(HandleGameOver);
            eventChannel.AddListener<WaveChangeEvent>(HandleWaveChange);
            _rect = gameOverUI.GetComponent<RectTransform>();
            background.gameObject.SetActive(false);
        }

        private void HandleWaveChange(WaveChangeEvent channel)
        {
            _currentWave = channel.currentWave;
        }

        private void OnDestroy()
        {
            eventChannel.RemoveListener<GameOverEvent>(HandleGameOver);
            eventChannel.RemoveListener<WaveChangeEvent>(HandleWaveChange);
        }

        public void HandleGameOver(GameOverEvent channel)
        {
            background.gameObject.SetActive(true);
            background.DOFade(0.7f, 0.5f);
            _rect.DOAnchorPosY(-1000f, 0.5f);
            CurrencyManager.Instance.ModifyCurrency(CurrencyType.Gem, _currentWave * 20);
        }
    }
}