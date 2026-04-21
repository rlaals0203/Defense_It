using System;
using _01_Script._00_Core.EventChannel;
using _01_Script._02_Unit;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitUpgradeUI : MonoBehaviour
{
    [SerializeField] private GameEventChannelSO eventChannel;
    [SerializeField] private TextMeshProUGUI upgradePrice;
    [SerializeField] private Image fillImage;

    private Button _upgradeButton;
    private Unit _unit;
    
    private readonly UnitUpgradeEvent _upgradeEvent = new UnitUpgradeEvent();

    private void Awake()
    {
        eventChannel.AddListener<UnitSelectEvent>(HandleUnitSelect);

        _upgradeButton = GetComponentInChildren<Button>();
        _upgradeButton.onClick.AddListener(HandleUpgradeClick);
    }

    private void OnDestroy()
    {
        eventChannel.RemoveListener<UnitSelectEvent>(HandleUnitSelect);
        _upgradeButton.onClick.RemoveListener(HandleUpgradeClick);
    }

    private void HandleUnitSelect(UnitSelectEvent channel)
    {
        _unit = channel.selectedUnit;
        ChangeUpgradeSlider();
        UpdateUpgradePriceText();
    }

    private void ChangeUpgradeSlider()
    {
        if (_unit == null) return;
        
        float xSize = _unit.CurrentLevel * 40;
        float ySize = fillImage.rectTransform.sizeDelta.y;
        
        fillImage.rectTransform.sizeDelta = new Vector2(xSize, ySize);
    }
    
    private void UpdateUpgradePriceText()
    {
        int level = _unit.CurrentLevel;
        
        if (level >= _unit.UnitSO.upgradePrice.Length)
            upgradePrice.text = "최대 레벨";
        else
        {
            upgradePrice.text = $"업그레이드 코스트 {_unit.UnitSO.upgradePrice[level]}";
        }
    }

    private void HandleUpgradeClick()
    {
        int currentLevel = _unit.CurrentLevel;

        if (currentLevel >= _unit.UnitSO.upgradePrice.Length)
            return;

        int price = _unit.UnitSO.upgradePrice[currentLevel];
        if (!CurrencyManager.Instance.TryModifyCurrency(CurrencyType.Coin, -price)) return;
        
        _unit.SetLevel(_unit.CurrentLevel + 1);
        ChangeUpgradeSlider();
        UpdateUpgradePriceText();
        eventChannel.RaiseEvent(_upgradeEvent.Initializer(_unit));
    }
}
