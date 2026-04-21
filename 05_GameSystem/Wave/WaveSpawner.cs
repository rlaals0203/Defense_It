using UnityEngine;
using _01_Script._00_Core.EventChannel;
using _01_Script._05_GameSystem.Wave;
using Ami.BroAudio;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace _01_Script._05_GameSystem
{
    public class WaveSpawner : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO eventChannel;
        [SerializeField] private WaveCommandExecutor commandExecutor;
        [SerializeField] private WaveListSO waveList;
        [SerializeField] private int startCoin;
        
        [SerializeField] private Transform spawnPos;
        [SerializeField] private Button waveSkipButton;
        [SerializeField] private SoundID waveSkipSound;

        private int _currentWave = 0;
        private bool _waveRunning;
        private float _waveDelayStartTime;
        
        private readonly float _waveDelayDuration = 25f;
        private readonly WaveChangeEvent _waveChangeEvent = WaveEvntChannel.WaveChangeEvent;
        private readonly GameCompleteEvent _gameCompleteEvent = WaveEvntChannel.GameCompleteEvent;
        private readonly CardSelectStartEvent _cardSelectEvent = CardEventChannel.CardSelectStartEvent;
        private readonly WaveSkipEvent _waveSkipEvent = WaveEvntChannel.WaveSkipEvent;

        private void Start()
        {
            CurrencyManager.Instance.SetCurrency(CurrencyType.Coin, startCoin);
            waveSkipButton.onClick.AddListener(SkipWave);
            commandExecutor.OnWaveComplete += HandleWaveComplete;
            StartNextWave();
        }

        private void OnDestroy()
        {
            waveSkipButton.onClick.RemoveListener(SkipWave);
            commandExecutor.OnWaveComplete -= HandleWaveComplete;
        }

        private void HandleWaveComplete()
        {
            StartNextWave();
        }

        private void StartNextWave()
        {
            _currentWave++;

            int coinScale = 100 * (_currentWave - 1);
            int coinEarn = Random.Range(coinScale, coinScale * 2);
            CurrencyManager.Instance.ModifyCurrency(CurrencyType.Coin, coinEarn);
            eventChannel.RaiseEvent(_waveChangeEvent.Initializer(_currentWave));

            if (_currentWave % 3 == 0)
                eventChannel.RaiseEvent(_cardSelectEvent);
            if (_currentWave >= 21)
                GameComplete();
            if (_currentWave > waveList.waveList.Count) 
                return;

            StartWave();
        }

        private void GameComplete()
        {
            eventChannel.RaiseEvent(_gameCompleteEvent);
            CurrencyManager.Instance.ModifyCurrency(CurrencyType.Gem, _currentWave * 30);
        }

        private void StartWave()
        {
            WaveSO waveSO = waveList.waveList[_currentWave - 1];
            commandExecutor.InitStage(waveSO, spawnPos);
        }
        
        public void SkipWave()
        {
            eventChannel.RaiseEvent(_waveSkipEvent.Initializer(GetRemainingDelayTime()));
            BroAudio.Play(waveSkipSound);

            StartNextWave();
        }
        
        private int GetRemainingDelayTime()
        {
            float elapsed = Time.time - _waveDelayStartTime;
            return Mathf.RoundToInt(Mathf.Max(0f, _waveDelayDuration - elapsed));
        }
    }
}
