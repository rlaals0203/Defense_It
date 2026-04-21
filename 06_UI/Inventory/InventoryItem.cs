using System;
using _01_Script._02_Unit;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _01_Script._06_UI.Inventory
{
    public class InventoryItem : MonoBehaviour
    {
        [SerializeField] private Image unitIcon;
        [SerializeField] private TextMeshProUGUI unitNameText;
        
        private Button _button;

        private UnitSO _unitSO;
        private InventoryInfoUI _infoUI;
        private RectTransform _rectTrm;

        private void Awake()
        {
            _rectTrm = GetComponent<RectTransform>();
            _button = GetComponentInChildren<Button>();
            _button.onClick.AddListener(HandleClick);
        }
        
        public void Initialize(InventoryInfoUI info)
        {
            _infoUI = info;
        }

        private void HandleClick()
        {
            if (_infoUI == null) return;
            
            _infoUI.ActiveInfoUI(true, _rectTrm);
            _infoUI.SetInfoUI(_unitSO);
        }

        private void Start()
        {
            InitUI(_unitSO);
        }

        public void InitUI(UnitSO unit)
        {
            if(unit == null) return;
            
            _unitSO = unit;
            unitIcon.sprite = unit.icon;
            unitNameText.text = unit.entityName;
        }
        
        private void OnDestroy()
        {
            _button.onClick.RemoveListener(HandleClick);
        }
    }
}