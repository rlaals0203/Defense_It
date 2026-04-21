using System.Collections;
using _01_Script._00_Core.EventChannel;
using _01_Script._00_Core.StatSystem;
using _01_Script._02_Unit;
using _01_Script.Core.ETC;
using MinLibrary.Dependencies;
using MinLibrary.ObjectPool.Runtime;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _01_Script._05_GameSystem
{
    public class UnitSelect : MonoBehaviour
    {
        [Inject] private PoolManagerMono _poolManager;
        [SerializeField] private GameEventChannelSO eventChannel;
        [SerializeField] private StatSO rangeStat;
        [SerializeField] private PlayerInputSO playerInput;
        [SerializeField] private LayerMask whatIsUnit;
        
        private UnitSelectEvent _unitSelectEvent = UnitGameEventChannel.UnitSelectEvent;
        private Unit _prevUnit;
        
        private bool _isSelected = false;

        private void Awake()
        {
            playerInput.OnClickEvent += HandleClick;
            eventChannel.AddListener<UnitUpgradeEvent>(HandleUnitUpgrade);
            eventChannel.AddListener<UnitSellEvent>(HandleUnitSell);
        }

        private void OnDestroy()
        {
            playerInput.OnClickEvent -= HandleClick;
            eventChannel.RemoveListener<UnitUpgradeEvent>(HandleUnitUpgrade);
            eventChannel.RemoveListener<UnitSellEvent>(HandleUnitSell);
        }

        private void HandleUnitSell(UnitSellEvent channel)
        {
            _prevUnit.ResetUnit();
            _poolManager.Push(channel.selectedUnit.GetComponent<PoolingEntity>());
            _prevUnit = null;
        }

        private void HandleUnitUpgrade(UnitUpgradeEvent channel)
        {
            StartCoroutine(UpgradeRangeGizmo(channel.selectedUnit));

            if (channel.selectedUnit.CurrentLevel >= 2)
            {
                EntitySkill skill = channel.selectedUnit.GetCompo<EntitySkill>();
                skill.IsActive = true;
            }
        }

        private IEnumerator UpgradeRangeGizmo(Unit unit)
        {
            yield return null;
            unit.ShowRangeGizmo(_isSelected, unit.GetUnitStatValue(rangeStat));
        }

        private void HandleClick()
        { 
            playerInput.GetWorldPosition(out RaycastHit hitInfo, whatIsUnit);
            if (EventSystem.current.IsPointerOverGameObject()) return;

            if (_isSelected && _prevUnit != null)
            {
                _prevUnit.ShowRangeGizmo(false, 1f);
                _prevUnit.IsSelected = false;
                _isSelected = false;
            }

            if (hitInfo.collider == null) return;

            if (hitInfo.collider.TryGetComponent(out Unit unit))
            {
                if (_prevUnit != null && _prevUnit.Equals(unit) == false)
                {
                    _prevUnit.IsSelected = false;
                    _prevUnit.ShowRangeGizmo(false);
                }
                
                unit.IsSelected = !unit.IsSelected;
                _isSelected = unit.IsSelected;

                unit.ShowRangeGizmo(_isSelected, unit.GetUnitStatValue(rangeStat));

                if (_isSelected)
                {
                    EntitySkill skill = unit.GetCompo<EntitySkill>();
                    eventChannel.RaiseEvent(_unitSelectEvent.Initializer(unit, skill));
                }
                
                _prevUnit = unit;
            }
        }
    }
}