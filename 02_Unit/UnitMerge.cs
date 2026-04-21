using System;
using _01_Script._00_Core.EventChannel;
using _01_Script._100_Misc.Effects;
using _01_Script.Core.ETC;
using DG.Tweening;
using MinLibrary.Dependencies;
using MinLibrary.ObjectPool.Runtime;
using Unity.Mathematics;
using UnityEngine;

namespace _01_Script._02_Unit
{
    public class UnitMerge : MonoBehaviour
    {
        [Inject] private PoolManagerMono _poolManager;
        
        [SerializeField] private GameEventChannelSO eventChannel;
        [SerializeField] private PlayerInputSO playerInput;
        [SerializeField] private PoolItemSO mergeEffect;
        [SerializeField] private PoolItemSO mergeUnit;
        [SerializeField] private LayerMask whatIsUnit;
        
        private bool _isSelectMode;
        private Unit _targetUnit;
        private Unit _selfUnit;

        private readonly UnitMergeSelectEvent _unitMergeSelectEvent = UnitGameEventChannel.UnitMergeSelectEvent;
        private readonly UnitMergeEvent _unitMergeEvent = UnitGameEventChannel.UnitMergeEvent;

        private void Awake()
        {
            eventChannel.AddListener<UnitSelectEvent>(HandleUnitSelect);
            playerInput.OnCancelEvent += HandleCancelClick;
        }
        
        public void SetSelectMode()
        {
            eventChannel.RaiseEvent(_unitMergeSelectEvent);
            _selfUnit.ShowRangeGizmo(false);
            
            playerInput.OnClickEvent += HandleClick;
            _isSelectMode = true;
        }

        private void HandleCancelClick()
        {
            CancelSelect();
        }

        private void CancelSelect()
        {
            playerInput.OnClickEvent -= HandleClick;
            _isSelectMode = false;
            _selfUnit = null;
        }

        private void HandleClick()
        {
            if (CanMerge(_selfUnit, _targetUnit))
            {
                MergeUnit(_targetUnit, _selfUnit);
                _targetUnit.ShowRangeGizmo(false);
            }
            else
            {
                CancelSelect();
            }
        }

        private void Update()
        {
            if (_isSelectMode)
                CheckMergeSelect();
        }

        private void CheckMergeSelect()
        {
            playerInput.GetWorldPosition(out RaycastHit hit, whatIsUnit);

            if (hit.collider == null || !hit.collider.TryGetComponent(out Unit unit)) return;
            bool canMerge = CanMerge(_selfUnit, unit);
            unit.SetOutline(canMerge, canMerge ? Color.green : Color.red);

            if (canMerge)
            {
                _targetUnit = unit;
            }
        }

        private bool CanMerge(Unit self, Unit target)
        {
            return self != null && target != null
                && self.CanMerge() && target.CanMerge()
                && self.GetType() == target.GetType()
                && self != target;
        }

        private void HandleUnitSelect(UnitSelectEvent channel)
        {
            if (_isSelectMode) return;
            _selfUnit = channel.selectedUnit;
        }

        private async void MergeUnit(Unit root, Unit other)
        {
            eventChannel.RaiseEvent(_unitMergeEvent);
            
            PoolingEntity rootPooling = root.GetComponent<PoolingEntity>();
            PoolingEntity otherPooling = other.GetComponent<PoolingEntity>();
            PoolingEffect poolingEffect = null;
            
            other.transform.DOMove(root.transform.position, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
            {
                _poolManager.Push(otherPooling);
                _poolManager.Push(rootPooling);
                
                poolingEffect = _poolManager.Pop<PoolingEffect>(mergeEffect);
                poolingEffect.PlayVFX(root.transform.position, quaternion.identity);
                
                PoolingEntity entity = _poolManager.Pop<PoolingEntity>(_selfUnit.UnitSO.upgradeUnitPool);
                entity.transform.position = root.transform.position;

                Unit unit = entity.Entity as Unit;
                unit.SetLevel(4);
            });
            
            await Awaitable.WaitForSecondsAsync(3f);
            _poolManager.Push(poolingEffect);
        }
        
        private void OnDestroy()
        {
            eventChannel.RemoveListener<UnitSelectEvent>(HandleUnitSelect);
            playerInput.OnClickEvent -= HandleClick;
            playerInput.OnCancelEvent -= HandleCancelClick;
        }
    }
}