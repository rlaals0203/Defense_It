using System;
using System.Linq;
using _01_Script._00_Core.EventChannel;
using _01_Script._02_Unit;
using _01_Script._06_UI.Shop;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _01_Script._06_UI.Inventory
{
    public class InventorySlot : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO eventChannel;
        [SerializeField] private EquippedUnit equippedUnit;
        [SerializeField] private UnitSO currentUnit;
        [SerializeField] private Image unitIcon;
        [SerializeField] private Image outline;
        [SerializeField] private TextMeshProUGUI unitNameText;

        private UnitSO _targetUnit;
        private Button _button;
        private bool _canSelect;
        private int index;
        
        private readonly UnitEquipEvent _unitEquipEvent = new UnitEquipEvent();

        private void Awake()
        {
            _button = GetComponent<Button>();

            eventChannel.AddListener<UnitEquipStartEvent>(HandleUnitEquipStart);
            eventChannel.AddListener<UnitEquipEvent>(HandleUnitEquip);
            _button.onClick.AddListener(HandleButtonClick);
            index = transform.GetSiblingIndex();
            
            UpdateUI(equippedUnit.inventory[index]);
        }
        
        private void HandleUnitEquip(UnitEquipEvent channel)
        {
            outline.DOFade(0f, 0.5f);
        }

        private void HandleButtonClick()
        {
            if (_canSelect == false || equippedUnit.inventory.Contains(_targetUnit)) return;
            
            equippedUnit.inventory[index] = _targetUnit;
            _targetUnit.isEquipped = true;
            
            currentUnit.isEquipped = false;
            currentUnit = _targetUnit;
            _canSelect = false;
            
            UpdateUI(_targetUnit);
            eventChannel.RaiseEvent(_unitEquipEvent.Initializer(index, _targetUnit));
            
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(equippedUnit);
            UnityEditor.AssetDatabase.SaveAssets();
#endif
            equippedUnit.SaveToJson();
        }

        private void HandleUnitEquipStart(UnitEquipStartEvent channel)
        {
            if (currentUnit != channel.Unit)
            {
                _targetUnit = channel.Unit;
                outline.DOFade(1f, 0.5f);
                _canSelect = true;
            }
        }

        private void UpdateUI(UnitSO unit)
        {
            if (unit == null) return;
            
            unitIcon.sprite = unit.icon;
            unitNameText.text = unit.entityName;
        }

        private void OnDestroy()
        {
            eventChannel.RemoveListener<UnitEquipStartEvent>(HandleUnitEquipStart);
            eventChannel.RemoveListener<UnitEquipEvent>(HandleUnitEquip);
            _button.onClick.RemoveListener(HandleButtonClick);
        }
    }
}