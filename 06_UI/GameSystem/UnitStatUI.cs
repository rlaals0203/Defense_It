using System;
using _01_Script._00_Core.EventChannel;
using _01_Script._00_Core.StatSystem;
using _01_Script._02_Unit;
using MinLibrary.Dependencies;
using MinLibrary.ObjectPool.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _01_Script._06_UI.GameSystem
{
    public class UnitStatUI : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO eventChannel;

        [Header("Elements")]
        [SerializeField] private Canvas canvas;
        [SerializeField] private RectTransform infoRect;
        [SerializeField] private TextMeshProUGUI unitNameText;
        [SerializeField] private Image unitIcon;
        [SerializeField] private GameObject background;
        [SerializeField] private Transform unitStats;
        [SerializeField] private Button sellButton;
        [SerializeField] private Button mergeButton;
        [SerializeField] private GameObject lockImage;
        
        [Header("StatSO")]
        [SerializeField] private StatSO attackPower;
        [SerializeField] private StatSO attackSpeed;
        [SerializeField] private StatSO attackRange;
        [SerializeField] private StatSO criticalPower;
        
        [SerializeField] private Vector3 offset = new Vector3(0, 0, 0);

        private TextMeshProUGUI _attackPowerText;
        private TextMeshProUGUI _attackSpeedText;
        private TextMeshProUGUI _attackRangeText;
        private TextMeshProUGUI _criticalPowerText;
        private TextMeshProUGUI _sellText;

        private Unit _unit;
        private int _price;

        private void Awake()
        {
            eventChannel.AddListener<UnitSelectEvent>(HandleUnitSelect);
            eventChannel.AddListener<UnitMergeSelectEvent>(HandleUnitMergeSelect);
            eventChannel.AddListener<UnitUpgradeEvent>(HandleUnitUpgrade);
            eventChannel.AddListener<UnitMergeEvent>(HandleUnitMerge);
            sellButton.onClick.AddListener(HandleUnitSell);

            _attackPowerText = unitStats.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
            _attackSpeedText = unitStats.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();
            _attackRangeText = unitStats.GetChild(2).GetComponentInChildren<TextMeshProUGUI>();
            _criticalPowerText = unitStats.GetChild(3).GetComponentInChildren<TextMeshProUGUI>();
            _sellText = sellButton.GetComponentInChildren<TextMeshProUGUI>();
            
            background.SetActive(false);
        }

        private void OnDestroy()
        {
            sellButton.onClick.RemoveListener(HandleUnitSell);
            eventChannel.RemoveListener<UnitSelectEvent>(HandleUnitSelect);
            eventChannel.RemoveListener<UnitUpgradeEvent>(HandleUnitUpgrade);
            eventChannel.RemoveListener<UnitMergeEvent>(HandleUnitMerge);
            eventChannel.RemoveListener<UnitMergeSelectEvent>(HandleUnitMergeSelect);
        }

        private void HandleUnitMerge(UnitMergeEvent obj)
        {
            background.SetActive(false);
        }

        private void HandleUnitMergeSelect(UnitMergeSelectEvent channel)
        {
            background.SetActive(false);
        }

        private void HandleUnitSell()
        {
            UnitSellEvent sellEvent = UnitGameEventChannel.UnitSellEvent;
            eventChannel.RaiseEvent(sellEvent.Initializer(_unit));
            background.SetActive(false);
            CurrencyManager.Instance.ModifyCurrency(CurrencyType.Coin, _price / 2);
        }

        private void Update()
        {
            if(_unit != null && background.activeSelf)
                UpdatePosition(_unit.transform.position);
        }

        private void HandleUnitSelect(UnitSelectEvent channel)
        {
            _unit = channel.selectedUnit;
            _unit.OnSelectedEvent += HandleUnitSelected;

            UpdateStatUI(channel.selectedUnit);
        }
        
        private void HandleUnitUpgrade(UnitUpgradeEvent channel)
        {
            UpdateStatUI(channel.selectedUnit);
        }

        private void UpdateStatUI(Unit unit)
        {
            mergeButton.interactable = unit.CurrentLevel == 4;
            lockImage.SetActive(unit.CurrentLevel <= 3);
            var unitSO = unit.UnitSO;

            _attackPowerText.text = _unit.GetUnitStatValue(attackPower).ToString();
            _attackSpeedText.text = _unit.GetUnitStatValue(attackSpeed).ToString();
            _attackRangeText.text = _unit.GetUnitStatValue(attackRange).ToString();
            _criticalPowerText.text = _unit.GetUnitStatValue(criticalPower).ToString();
            
            unitIcon.sprite = unitSO.icon;
            unitNameText.text = unitSO.entityName;
            
            SetPrice(unitSO);
            background.SetActive(_unit.IsSelected);
        }

        private void SetPrice(UnitSO unit)
        {
            _price = unit.price;

            for (int i = 0; i < _unit.CurrentLevel; i++)
            {
                _price += unit.upgradePrice[i];
            }

            if (unit.canMerge == false)
                mergeButton.interactable = false;

            _sellText.text = $"판매 : {_price / 2}원";
        }
        

        private void HandleUnitSelected(bool isSelected)
        {
            if (!isSelected)
            {
                background.SetActive(false);
                _unit.OnSelectedEvent -= HandleUnitSelected;
            }
        }
        
        private void UpdatePosition(Vector3 worldPos)
        {
            Vector3 targetPos = worldPos + Vector3.up * 2f;
            Vector3 screenPos = Camera.main.WorldToScreenPoint(targetPos);

            if (canvas.renderMode == RenderMode.ScreenSpaceCamera || canvas.renderMode == RenderMode.WorldSpace)
            {
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                        canvas.transform as RectTransform, screenPos, 
                        canvas.worldCamera, out Vector2 localPoint)) {
                    infoRect.localPosition = localPoint + (Vector2)offset;
                }
            }
            else
                infoRect.position = screenPos + offset;
        }
    }
}