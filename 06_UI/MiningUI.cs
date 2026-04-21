using System;
using _01_Script._00_Core.EventChannel;
using _01_Script._05_GameSystem;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MiningUI : MonoBehaviour
{
    [SerializeField] private GameEventChannelSO eventChannel;

    [SerializeField] private MiningSystem miningSystem;
    [SerializeField] private GameObject miningUI;
    [SerializeField] private Button upgradeBtn;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI prevCoinText;
    [SerializeField] private TextMeshProUGUI nextCoinText;
    [SerializeField] private TextMeshProUGUI priceText;

    private MiningUpgradeEvent _upgradeEvent = GameSystemEventChannel.MiningUpgradeEvent;
    private bool _isActive;

    private void Awake()
    {
        upgradeBtn.onClick.AddListener(HandleUpgradeClick);
        eventChannel.AddListener<MiningUIOpenEvent>(HandleMiningUIOpen);
        
        miningUI.SetActive(true);
        miningUI.transform.localScale = Vector3.zero;
    }

    private void OnDestroy()
    {
        upgradeBtn.onClick.RemoveListener(HandleUpgradeClick);
        eventChannel.RemoveListener<MiningUIOpenEvent>(HandleMiningUIOpen);
    }

    private void HandleMiningUIOpen(MiningUIOpenEvent channel)
    {
        _isActive = !_isActive;

        if (_isActive)
        {
            miningUI.transform.DOScale(1f, 0.4f).SetEase(Ease.OutBounce);
        }
        else
        {
            miningUI.transform.DOScale(0f, 0.2f);
        }
        
        InitUI();
    }

    private void Start()
    {
        InitUI();
    }

    private void InitUI()
    {
        levelText.text = $"광산 Lv.{miningSystem.CurrentLevel}";
        prevCoinText.text = $"{Mathf.Round(miningSystem.CoolTime * 10) / 10}초당 {miningSystem.GoldAmount}코인";
        nextCoinText.text = $"{Mathf.Round(miningSystem.NextCoolTime * 10) / 10}초당 {miningSystem.NextGoldAmount}코인";
        priceText.text = $"업그레이드 코스트 : {miningSystem.Price}";
        
        prevCoinText.transform.DOScale(1.2f, 0.1f).OnComplete(() =>
        {
            prevCoinText.transform.DOScale(1f, 0.1f);
        });
        
        nextCoinText.transform.DOScale(1.2f, 0.1f).OnComplete(() =>
        {
            nextCoinText.transform.DOScale(1f, 0.1f);
        });

        if (miningSystem.CurrentLevel == 50)
        {
            upgradeBtn.GetComponentInChildren<TextMeshProUGUI>().text = "최대 레벨";
        }
    }

    private void HandleUpgradeClick()
    {
        eventChannel.RaiseEvent(_upgradeEvent);
        InitUI();
    }
}
