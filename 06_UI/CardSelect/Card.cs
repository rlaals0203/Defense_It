using System;
using _01_Script._00_Core.EventChannel;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _01_Script._06_UI.CardSelect
{
    public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public bool IsSelected { get; set; }

        [SerializeField] private GameEventChannelSO eventChannel;
        [SerializeField] private TextMeshProUGUI statText;
        [SerializeField] private Image cardIcon;
        [SerializeField] private Image outLine;
        
        public Button cardButton;
        
        private readonly CardSelectEvent _cardSelectEvent = new CardSelectEvent();

        private void Awake()
        {
            cardButton = GetComponent<Button>();
        }

        public void InitializeCard(Card card, CardSO cardSO)
        {
            IsSelected = false;

            statText.text = cardSO.description;

            cardIcon.sprite = cardSO.icon;
            cardButton.onClick.AddListener(() => HandleCardSelect(card, cardSO));
        }

        private void HandleCardSelect(Card card, CardSO cardSO)
        {
            eventChannel.RaiseEvent(_cardSelectEvent.Initializer(card, cardSO));
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (IsSelected) return;
            transform.DOScale(1.2f, 0.2f);
            outLine.DOFade(1f, 0.1f);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (IsSelected) return;
            transform.DOScale(1f, 0.2f);
            outLine.DOFade(0f, 0.1f);
        }
    }
}