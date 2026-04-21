using _01_Script._00_Core.EventChannel;
using _01_Script._01_Entities;
using _01_Script._02_Unit;
using _01_Script.Core.ETC;
using Ami.BroAudio;
using DG.Tweening;
using MinLibrary.Dependencies;
using MinLibrary.ObjectPool.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace _01_Script._05_GameSystem.BuildingSystem
{
    public class DeployUnit : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO eventChannel;
        [SerializeField] private PlayerInputSO playerInput;
        [SerializeField] private Grid mapGrid;
        [SerializeField] private Material gizmoMat;
        [SerializeField] private LayerMask deployableLayer;
        [SerializeField] private LayerMask whatIsUnit;
        
        [SerializeField] private TextMeshProUGUI cancelText;
        [SerializeField] private SoundID deploySound;
        [SerializeField] private SoundID failSound;

        [Inject] private PoolManagerMono _poolManager;

        private bool _constructionMode;
        private bool _canDeploy;
        private UnitRangeGizmo _gizmo;
        private PoolItemSO _unitPool;
        private EntitySO _unitSO;
        private GameObject _gizmoObject;
        
        private readonly Color _redCol = new Color(1f, 0.25f, 0.25f, 0.07f);
        private readonly Color _greenCol = new Color(0.25f, 0.8f, 0.25f, 0.07f);
        private readonly UnitPlaceEndEvent _unitPlaceEndEvent = UnitGameEventChannel.UnitPlaceEndEvent;
        private readonly UnitPlaceCancelEvent _unitPlaceCancelEvent = UnitGameEventChannel.UnitPlaceCancelEvent;
        
        private const float S = 0.75f;
        private readonly float[] dx = { 0, 0, S, S, S, 0, -S, -S, -S };
        private readonly float[] dz = { 0, S, S, 0, -S, -S, -S, 0, S };
        
        public bool ConstructionMode 
        {
            get => this;
            set 
            {
                if (_gizmoObject != null)
                    _gizmoObject.SetActive(value);
                
                cancelText.gameObject.SetActive(value);
                _constructionMode = value;
            }
        }
        
        public bool CanDeploy
        {
            get => this;
            set
            {
                _canDeploy = value;
                
                var color = _canDeploy ? _greenCol : _redCol;
                _gizmo.ChangeGizmoColor(color);
                gizmoMat.color = color;
            }
        }
        
        public UnityEvent<Vector3> DeployEvent;
 
        private void Awake()
        {
            playerInput.OnClickEvent += HandleClick;
            playerInput.OnCancelEvent += HandleCancel;
            
            eventChannel.AddListener<UnitPlaceStartEvent>(HandlePressUnitButton);
        }
        
        private void OnDestroy()
        {
            playerInput.OnCancelEvent -= HandleCancel;
            playerInput.OnClickEvent -= HandleClick;
            eventChannel.RemoveListener<UnitPlaceStartEvent>(HandlePressUnitButton);
        }
    
        private void HandleCancel()
        {
            eventChannel.RaiseEvent(_unitPlaceCancelEvent);
            ConstructionMode = false;
        }
        
        private void Update()
        {
            if (_constructionMode)
                UpdateGizmoPosition();
        }
        
        private void HandlePressUnitButton(UnitPlaceStartEvent channel)
        {
            _unitSO = channel.selectedUnit;
            _unitPool = channel.selectedUnit.entityPool;

            SetupOrUpdateGizmo(channel.selectedUnit.unitGizmo, channel.selectedUnit);
            channel.gizmo = _gizmoObject;
            ConstructionMode = true;
        }
        
        private void SetupOrUpdateGizmo(GameObject newGizmoPrefab, EntitySO entitySO)
        {
            UnitSO unitSO = entitySO as UnitSO;
            if (_gizmoObject == null)
                _gizmoObject = Instantiate(newGizmoPrefab, transform);
            else
                ChangeGizmo(_gizmoObject, newGizmoPrefab);

            _gizmo = _gizmoObject.GetComponentInChildren<UnitRangeGizmo>();
            _gizmo?.ChangeGizmoStatus(true, unitSO.range);
        }

        private void HandleClick()
        {
            if (EventSystem.current.IsPointerOverGameObject()
                || !_constructionMode || !_canDeploy) return;

            if (!CurrencyManager.Instance.TryModifyCurrency(CurrencyType.Coin, -_unitSO.price)) 
            {
                failSound.Play();
                return;
            };

            Vector3 center = mapGrid.GetCellCenterWorld(GetCellPoint());
            center.y += 1f;
            
            if (!CheckUnit(center)) return;
            
            GameObject unitObj = _poolManager.Pop<PoolingEntity>(_unitPool).gameObject;
            unitObj.transform.position = center;
            unitObj.transform.localScale = Vector3.one * 0.1f;
            unitObj.transform.SetParent(transform);
            unitObj.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBounce);
            
            BroAudio.Play(deploySound);
            DeployEvent?.Invoke(center);
            AfterClick();
        }
        
        private void AfterClick()
        {
            ConstructionMode = false;
            eventChannel.RaiseEvent(_unitPlaceEndEvent);

            if (_gizmo != null)
                _gizmo.ChangeGizmoStatus(false, 1f);
        }

        private Vector3Int GetCellPoint()
        {
            Vector3 worldPosition = playerInput.GetWorldPosition();
            Vector3Int cellPoint = mapGrid.WorldToCell(worldPosition);
            return cellPoint;
        }

        private void UpdateGizmoPosition()
        {
            if (_gizmoObject == null) return;

            Physics.Raycast(_gizmoObject.transform.position, -Vector3.up, 
                out RaycastHit hit, Mathf.Infinity, deployableLayer);
            
            Vector3Int cellPoint = GetCellPoint();
            Vector3 center = mapGrid.GetCellCenterWorld(cellPoint);
            center.y += 1f;
            
            _gizmoObject.transform.position = center;
            CanDeploy = hit.collider && CheckUnit(center);
        }

        private bool CheckUnit(Vector3 center)
        {
            center.y = 5f;
            
            for (int i = 0; i < dx.Length; i++)
            {
                Vector3 pos = center + new Vector3(dx[i], 5, dz[i]);
                if (Physics.Raycast(pos, -Vector3.up, 10f, whatIsUnit))
                {
                    return false;
                }
            }

            return true;
        }
        
        void ChangeGizmo(GameObject prevObject, GameObject newObject)
        {
            var prevFilter = prevObject.GetComponent<MeshFilter>();
            var newFilter = newObject.GetComponent<MeshFilter>();
            if (prevFilter != null && newFilter != null)
            {
                prevFilter.sharedMesh = newFilter.sharedMesh;
            }
        }
    }
}
