using System;
using System.Linq;
using _01_Script._00_Core.EventChannel;
using _01_Script._02_Unit;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _01_Script._06_UI.Shop
{
    public class ShopItemInfo : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO eventChannel;
        
        [SerializeField] private Image unitIcon;
        [SerializeField] private TextMeshProUGUI unitNameText;
        [SerializeField] private TextMeshProUGUI unitPriceText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        
        [SerializeField] private TextMeshProUGUI attackDmgText;
        [SerializeField] private TextMeshProUGUI attackCoolText;
        [SerializeField] private TextMeshProUGUI rangeText;
        [SerializeField] private TextMeshProUGUI criticalDmgText;
        
        [SerializeField] private Button purchaseButton;
        [SerializeField] private Button cancelButton;

        [SerializeField] private GameObject starParent;
        [SerializeField] private GameObject itemsParent;

        [SerializeField] private Sprite fillStar;
        [SerializeField] private Sprite emptyStar;
        [SerializeField] private Vector3 offset;

        private readonly Image[] _stars = new Image[5];
        private readonly UnitPurchaseEvent _purchaseEvent = UIGameEventChannel.UnitPurchaseEvent;
        
        private RectTransform _rectTrm;
        private UnitSO _unit;

        private void Awake()
        {
            for (int i = 0; i < starParent.transform.childCount; i++)
            {
                _stars[i] = starParent.transform.GetChild(i).GetComponent<Image>();
            }

            itemsParent.GetComponentsInChildren<ShopItemUI>().ToList().ForEach(item =>
            {
                item.Initialize(this);
            });

            purchaseButton.onClick.AddListener(HandlePurchase);
            cancelButton.onClick.AddListener(HandleCancel);
            
            _rectTrm = GetComponent<RectTransform>();
            ActiveInfoUI(false);
        }

        private void HandlePurchase()
        {
            if (CurrencyManager.Instance.TryModifyCurrency(CurrencyType.Gem, -_unit.shopPrice))
            {
                eventChannel.RaiseEvent(_purchaseEvent.Initializer(_unit));
                ActiveInfoUI(false);
            }
        }

        private void HandleCancel()
        {
            ActiveInfoUI(false);
        }

        public void ActiveInfoUI(bool isActive, RectTransform rectTrm = default)
        {
            gameObject.SetActive(isActive);

            if (rectTrm != default)
            {
                _rectTrm.position = rectTrm.position + offset;
            }
        }

        public void SetInfoUI(UnitSO unit)
        {
            _unit = unit;
            unitIcon.sprite = unit.icon;
            unitNameText.text = unit.entityName;
            unitPriceText.text = unit.shopPrice.ToString();
            descriptionText.text = unit.description;

            attackDmgText.text = unit.attackDamage.ToString();
            attackCoolText.text = unit.attackCooltime.ToString();
            rangeText.text = unit.range.ToString();
            criticalDmgText.text = unit.criticalDamage.ToString();
            
            _stars.ToList().ForEach(star => star.sprite = emptyStar);

            for (int i = 0; i < unit.rating - 1; i++)
            {
                _stars[i].sprite = fillStar;
            }
        }

        private void OnDestroy()
        {
            purchaseButton.onClick.RemoveListener(HandlePurchase);
            cancelButton.onClick.RemoveListener(HandleCancel);
        }
    }
}