using System;
using _01_Script._00_Core.EventChannel;
using _01_Script.Core.ETC;
using UnityEngine;

namespace _01_Script._05_GameSystem
{
    public class MiningSystem : MonoBehaviour
    {
        [SerializeField] private PlayerInputSO playerInput;
        [SerializeField] private GameEventChannelSO eventChannel;
        [SerializeField] private LayerMask whatIsMiner;

        private float _time;
        private readonly MiningUIOpenEvent _miningOpen = GameSystemEventChannel.MiningUIOpenEvent;
        
        public int CurrentLevel { get; set; } = 1;
        public int GoldAmount { get; set; } = 1;
        public float CoolTime { get; set; } = 4f;
        public float NextCoolTime { get; set; }
        public int NextGoldAmount { get; set; }
        public int Price { get; set; } = 10;

        private void Awake()
        {
            eventChannel.AddListener<MiningUpgradeEvent>(HandleUpgrade);
            eventChannel.AddListener<WaveSkipEvent>(HandleWaveSkip);
            playerInput.OnClickEvent += HandleClick;

            _time = Time.time;
            GetNextStat();
        }

        private void OnDestroy()
        {
            eventChannel.RemoveListener<MiningUpgradeEvent>(HandleUpgrade);
            eventChannel.RemoveListener<WaveSkipEvent>(HandleWaveSkip);
            playerInput.OnClickEvent -= HandleClick;
        }
        
        private void HandleWaveSkip(WaveSkipEvent channel)
        {
            int amount = Mathf.RoundToInt((channel.leftTime * GoldAmount / CoolTime) * 1.75f);
            ChangeCoin(amount);
        }

        private void Update()
        {
            if (Time.time - _time > CoolTime)
            {
                _time = Time.time;
                ChangeCoin(GoldAmount);
            }
        }

        private void HandleClick()
        { 
            playerInput.GetWorldPosition(out RaycastHit hitInfo, whatIsMiner);
            
            if(hitInfo.collider != null)
                eventChannel.RaiseEvent(_miningOpen);
        }

        private void GetNextStat()
        {
            NextGoldAmount = GoldAmount + GetGoldAmount();
            NextCoolTime = CoolTime - GetNextCoolTime();
        }

        private void HandleUpgrade(MiningUpgradeEvent channel)
        {
            if (CurrentLevel >= 50) return;
            if (!CurrencyManager.Instance.TryModifyCurrency(CurrencyType.Coin, -Price)) return;
            
            CurrentLevel++;
            GoldAmount += GetGoldAmount();
            CoolTime -= GetNextCoolTime();
            Price += Mathf.RoundToInt(CurrentLevel * 1.2f);

            GetNextStat();
        }

        private float GetNextCoolTime()
        {
            return CurrentLevel % 6 == 0 ? 0.2f : 0;
        }

        private int GetGoldAmount()
        {
            return 1 + CurrentLevel / 4;
        }

        private void ChangeCoin(float value)
            => CurrencyManager.Instance.ModifyCurrency(CurrencyType.Coin, value);
    }
}