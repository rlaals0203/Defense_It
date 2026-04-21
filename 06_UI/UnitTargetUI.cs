using System;
using _01_Script._00_Core.EventChannel;
using _01_Script._02_Unit;
using _01_Script._02_Unit.Combat;
using TMPro;
using UnityEngine;

namespace _01_Script._06_UI
{
    public class UnitTargetUI : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO eventChannel;
        [SerializeField] private TextMeshProUGUI targetText;

        private string[] _targetText = { "앞", "뒤", "체력" };
        private Unit _unit;

        private void Awake()
        {
            eventChannel.AddListener<UnitSelectEvent>(HandleUnitSelect);
        }

        private void OnDestroy()
        {
            eventChannel.RemoveListener<UnitSelectEvent>(HandleUnitSelect);
        }

        private void HandleUnitSelect(UnitSelectEvent channel)
        {
            _unit = channel.selectedUnit;
            UpdateUI();
        }

        public void UpdateTarget()
        {
            int idx = (int)_unit.AttackMethod;
            idx++;

            if (idx == 3)
                idx = 0;
            
            _unit.SetAttackMethod((UnitAttackMethod)idx);
            targetText.text = _targetText[idx];
        }

        public void UpdateUI()
        {
            int idx = (int)_unit.AttackMethod;
            targetText.text = _targetText[idx];
        }
    }
}