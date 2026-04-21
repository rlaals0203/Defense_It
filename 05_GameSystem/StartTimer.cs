using System;
using _01_Script._00_Core.EventChannel;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _01_Script._05_GameSystem
{
    public class StartTimer : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO eventChannel;
        [SerializeField] private RectTransform waveUI;
        [SerializeField] private TextMeshProUGUI timerText;

        private float _time;
        private int _leftTime = 30;
        
        private readonly int _defaultTime = 20;
        private readonly Color _redCol = new Color(1f, 0.3f, 0.3f, 1f);

        private void Awake()
        {
            InitText(30);
            eventChannel.AddListener<CanWaveSkipEvent>(HandleCanWaveSkip);
            eventChannel.AddListener<WaveSkipEvent>(HandleWaveSkip);
        }

        private void OnDestroy()
        {
            eventChannel.RemoveListener<CanWaveSkipEvent>(HandleCanWaveSkip);
            eventChannel.RemoveListener<WaveSkipEvent>(HandleWaveSkip);
        }

        private void HandleCanWaveSkip(CanWaveSkipEvent channel)
        {
            InitText(_defaultTime);
        }

        private void Update()
        {
            if (Time.time - _time > 1)
            {
                _time = Time.time;
                UpdateText(--_leftTime);
            }
        }
        
        private void HandleWaveSkip(WaveSkipEvent obj) => UpdateText(0);


        private void InitText(int time)
        {
            _leftTime = time;
            _time = Time.time;

            timerText.transform.localScale = Vector3.one;
            timerText.color = Color.white;
            waveUI.DOAnchorPosY(-125f, 0.5f).SetEase(Ease.OutBack);
            timerText.text = $"남은 시간 : {_leftTime}초";
        }

        private void UpdateText(int timeLeft)
        {
            timerText.text = $"남은 시간 : {timeLeft}초";

            if (timeLeft <= 5)
            {
                timerText.transform.DOScale(1.4f, 0.1f).OnComplete(() =>
                {
                    timerText.transform.DOScale(1.2f, 0.1f);
                });
                
                timerText.DOColor(_redCol, 0.5f);
            }

            if (timeLeft <= 0)
            {
                waveUI.DOAnchorPosY(125f, 0.5f).SetEase(Ease.InBack);
            }
        }
    }
}