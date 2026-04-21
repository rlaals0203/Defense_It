using System;
using System.Collections;
using System.Collections.Generic;
using _01_Script._00_Core.EventChannel;
using _01_Script._00_Core.StatSystem;
using _01_Script._01_Entities.Stat;
using _01_Script._02_Unit;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _01_Script._06_UI.GameSystem
{
    public class UnitGizmoStatUI : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO eventChannel;

        [Header("Elements")]
        [SerializeField] private Canvas canvas;
        [SerializeField] private RectTransform infoRect;
        [SerializeField] private TextMeshProUGUI unitNameText;
        [SerializeField] private Image unitIcon;
        [SerializeField] private GameObject background;
        [SerializeField] private Transform unitStats;
        
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

        private GameObject _gizmo;
        private Unit _unit;
        
        private readonly UnitPlaceStartEvent _unitPlaceStart = UnitGameEventChannel.UnitPlaceStartEvent;

        private void Awake()
        {
            eventChannel.AddListener<UnitPlaceStartEvent>(HandleUnitGizmo);
            eventChannel.AddListener<UnitPlaceEndEvent>(HandlePlaceEnd);
            eventChannel.AddListener<UnitPlaceCancelEvent>(HandlePlaceCancel);
            
            _attackPowerText = unitStats.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
            _attackSpeedText = unitStats.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();
            _attackRangeText = unitStats.GetChild(2).GetComponentInChildren<TextMeshProUGUI>();
            _criticalPowerText = unitStats.GetChild(3).GetComponentInChildren<TextMeshProUGUI>();
            
            background.SetActive(false);
        }

        private void HandlePlaceCancel(UnitPlaceCancelEvent obj)
        {
            background.SetActive(false);
        }

        private void HandlePlaceEnd(UnitPlaceEndEvent obj)
        {
            background.SetActive(false);
        }

        private void Update()
        {
            if(_gizmo != null && background.activeSelf)
                UpdatePosition(_gizmo.transform.position);
        }

        private void HandleUnitGizmo(UnitPlaceStartEvent channel)
        {
            background.SetActive(true);
            UpdateUI(channel.selectedUnit as UnitSO);

            StartCoroutine(GizmoRoutine());
        }

        private IEnumerator GizmoRoutine()
        {
            yield return null;
            _gizmo = _unitPlaceStart.gizmo;
        }

        private void UpdateUI(UnitSO unit)
        {
            _attackPowerText.text = unit.attackDamage.ToString();
            _attackSpeedText.text = unit.attackCooltime.ToString();
            _attackRangeText.text = unit.range.ToString();
            _criticalPowerText.text = unit.criticalDamage.ToString();

            unitIcon.sprite = unit.icon;
            unitNameText.text = unit.entityName;
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

        private void OnDestroy()
        {
            eventChannel.RemoveListener<UnitPlaceCancelEvent>(HandlePlaceCancel);
            eventChannel.RemoveListener<UnitPlaceStartEvent>(HandleUnitGizmo);
            eventChannel.RemoveListener<UnitPlaceEndEvent>(HandlePlaceEnd);
        }
    }
}