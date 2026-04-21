using System;
using _01_Script._00_Core.EventChannel;
using _01_Script._01_Entities;
using _01_Script._02_Unit;
using _01_Script._06_UI.Shop;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _01_Script._06_UI.GameSystem
{
    public class UnitSelectButton : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO eventChannel;
        [SerializeField] private EquippedUnit inventorySO;
        [SerializeField] private TextMeshProUGUI priceText;
        [SerializeField] private Image unitImage;
        [SerializeField] private Image outLine;

        private readonly UnitPlaceStartEvent _unitStart = UnitGameEventChannel.UnitPlaceStartEvent;
        private readonly ChangeSelectStatement _changeSelectMode = UnitGameEventChannel.ChangeSelectStatement;
        private Button _purchaseButton;
        
        private UnitSO _unitSO;


        public bool IsSelectMode
        {
            get => this;
            set => _isSelectMode = value;
        }

        private bool _isSelectMode;

        private void Awake()
        {
            inventorySO.LoadFromJson();

            _purchaseButton = gameObject.GetComponent<Button>();
            _purchaseButton.onClick.AddListener(() => OnPurchaseClick());
            eventChannel.AddListener<UnitPlaceEndEvent>(HandleUnitPlaceEnd);
            eventChannel.AddListener<UnitPlaceCancelEvent>(HandleUnitPlaceCancel);
            eventChannel.AddListener<ChangeSelectStatement>(HandleChangeSelectState);
            
            _unitSO = inventorySO.inventory[transform.GetSiblingIndex()];
            unitImage.sprite = _unitSO.icon;
            priceText.text = _unitSO.price.ToString();
        }

        private void OnDestroy()
        {
            _purchaseButton.onClick.RemoveListener(() => OnPurchaseClick());
            eventChannel.RemoveListener<UnitPlaceEndEvent>(HandleUnitPlaceEnd);
            eventChannel.RemoveListener<UnitPlaceCancelEvent>(HandleUnitPlaceCancel);
            eventChannel.RemoveListener<ChangeSelectStatement>(HandleChangeSelectState);
        }

        private void HandleChangeSelectState(ChangeSelectStatement channel)
        {
            _isSelectMode = channel.isSelectMode;
        }

        private void HandleUnitPlaceCancel(UnitPlaceCancelEvent channel)
        {
            eventChannel.RaiseEvent(_changeSelectMode.Initializer(false));
            outLine.DOFade(0, 0.1f);
        }

        private void HandleUnitPlaceEnd(UnitPlaceEndEvent channel)
        {
            eventChannel.RaiseEvent(_changeSelectMode.Initializer(false));
            outLine.DOFade(0f, 0.1f);
        }

        private void OnPurchaseClick()
        {
            if(_isSelectMode) return;
            outLine.DOFade(1f, 0.1f);
            eventChannel.RaiseEvent(_changeSelectMode.Initializer(true));

            eventChannel.RaiseEvent(_unitStart.Initializer(_unitSO));
        }
    }
}