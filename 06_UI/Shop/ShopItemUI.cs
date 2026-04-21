using System;
using System.Globalization;
using _01_Script._00_Core.EventChannel;
using _01_Script._02_Unit;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _01_Script._06_UI.Shop
{
    public class ShopItemUI : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO eventChannel;
        [SerializeField] private InventorySO inventorySO;
        [SerializeField] private UnitSO unitSO;
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI itemName;
        [SerializeField] private TextMeshProUGUI itemPrice;

        private RectTransform _rectTrm;
        
        private Button _button;
        private ShopItemInfo _infoUI;
        
        private void Awake()
        {
            _button = GetComponentInChildren<Button>();
            _button.onClick.AddListener(HandleClick);
            eventChannel.AddListener<UnitPurchaseEvent>(HandleUnitPurchase);
            _rectTrm = GetComponent<RectTransform>();
            
            InitUI();
        }
        
        private void HandleUnitPurchase(UnitPurchaseEvent channel)
        {
            var purchasedUnit = channel.Unit;

            Debug.Log(purchasedUnit.isOwned);
            purchasedUnit.isOwned = true;
            purchasedUnit.isEquipped = false;

            if (!purchasedUnit.isOwned)
                inventorySO.inventory.Add(purchasedUnit);
            
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(inventorySO);
            UnityEditor.AssetDatabase.SaveAssets();
#endif
            inventorySO.SaveToJson();

            InitUI();
        }

        private void HandleClick()
        {
            if (_infoUI == null || unitSO.isOwned) return;
            
            _infoUI.ActiveInfoUI(true, _rectTrm);
            _infoUI.SetInfoUI(unitSO);

            unitSO.isEquipped = true;
            InitUI();
        }

        public void Initialize(ShopItemInfo shopItemInfo)
        {
            _infoUI = shopItemInfo;
        }

        private void InitUI()
        {
            icon.sprite = unitSO.icon;
            itemName.text = unitSO.entityName;

            itemPrice.text = unitSO.isOwned ? "소유중" : unitSO.shopPrice.ToString();
            _button.interactable = !unitSO.isOwned;
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(HandleClick);
        }
    }
}
