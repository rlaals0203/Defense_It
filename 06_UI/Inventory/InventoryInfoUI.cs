using System.Linq;
using _01_Script._00_Core.EventChannel;
using _01_Script._02_Unit;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _01_Script._06_UI.Inventory
{
    public class InventoryInfoUI : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO eventChannel;
        [SerializeField] private TextMeshProUGUI unitNameText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        
        [SerializeField] private TextMeshProUGUI attackDmgText;
        [SerializeField] private TextMeshProUGUI attackCoolText;
        [SerializeField] private TextMeshProUGUI rangeText;
        [SerializeField] private TextMeshProUGUI criticalDmgText;
        
        [SerializeField] private GameObject starParent;
        [SerializeField] private GameObject itemsParent;

        [SerializeField] private Button equipButton;
        [SerializeField] private Button cancelButton;
        
        [SerializeField] private Sprite fillStar;
        [SerializeField] private Sprite emptyStar;
        [SerializeField] private Vector3 offset;
        
        private readonly Image[] _stars = new Image[5];
        private RectTransform _rectTrm;
        private UnitSO _unit;
        
        private readonly UnitEquipStartEvent _unitEquipStart = new UnitEquipStartEvent();

        private void Awake()
        {
            _rectTrm = GetComponent<RectTransform>();
            
            for (int i = 0; i < starParent.transform.childCount; i++)
            {
                _stars[i] = starParent.transform.GetChild(i).GetComponent<Image>();
            }
            
            equipButton.onClick.AddListener(HandleUnitEquipStart);
            cancelButton.onClick.AddListener(HandleUnitEquipCancel);
            eventChannel.AddListener<UnitEquipEvent>(HandleUnitEquip);
            gameObject.SetActive(false);
        }

        private void HandleUnitEquipCancel()
        {
            ActiveInfoUI(false);
        }

        private void HandleUnitEquip(UnitEquipEvent obj)
        {
            ActiveInfoUI(false);
        }

        private void HandleUnitEquipStart()
        {
            if (_unit.isEquipped) return;
            eventChannel.RaiseEvent(_unitEquipStart.Initializer(_unit));
        }

        public void InitializeInfoUI()
        {
            itemsParent.GetComponentsInChildren<InventoryItem>().ToList().ForEach(item =>
            {
                item.Initialize(this);
            });
        }

        public void SetInfoUI(UnitSO unit)
        {
            unitNameText.text = unit.entityName;
            descriptionText.text = unit.description;

            attackDmgText.text = unit.attackDamage.ToString();
            attackCoolText.text = unit.attackCooltime.ToString();
            rangeText.text = unit.range.ToString();
            criticalDmgText.text = unit.criticalDamage.ToString();
            
            _stars.ToList().ForEach(star => star.sprite = emptyStar);

            for (int i = 0; i < unit.rating; i++)
            {
                _stars[i].sprite = fillStar;
            }

            _unit = unit;
        }
        
        public void ActiveInfoUI(bool isActive, RectTransform rectTrm = default)
        {
            gameObject.SetActive(isActive);

            if (rectTrm != default)
            {
                _rectTrm.position = rectTrm.position + offset;
            }
        }

        private void OnDestroy()
        {
            equipButton.onClick.RemoveListener(HandleUnitEquipStart);
            cancelButton.onClick.RemoveListener(HandleUnitEquipCancel);
            eventChannel.RemoveListener<UnitEquipEvent>(HandleUnitEquip);
        }
    }
}