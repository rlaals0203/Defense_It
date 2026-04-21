using System;
using System.Linq;
using _01_Script._00_Core.EventChannel;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _01_Script._06_UI.CardSelect
{
    public class CardSelection : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO eventChannel;
        [SerializeField] private Image background;
        [SerializeField] private RectTransform[] rects;
        [SerializeField] private Card[] cards;
        [SerializeField] private GameObject cardGroup;
        [SerializeField] private CardSO[] cardStats;

        private float _prevSped;

        private void Awake()
        {
            eventChannel.AddListener<CardSelectStartEvent>(HandleStartCardSelect);
            eventChannel.AddListener<CardSelectEvent>(HandleCardSelected);
        }

        private void OnDestroy()
        {
            eventChannel.RemoveListener<CardSelectStartEvent>(HandleStartCardSelect);
            eventChannel.RemoveListener<CardSelectEvent>(HandleCardSelected);
        }

        private void InitCard()
        {
            for (int i = 0; i < 3; i++)
            {
                int idx = UnityEngine.Random.Range(0, cardStats.Length);
                cards[i].InitializeCard(cards[i], cardStats[idx]);
            }
        }

        private void CardAnimation()
        {
            _prevSped = Time.timeScale;
            Time.timeScale = 1f;
            Sequence seq = DOTween.Sequence();
            seq.Append(background.DOFade(0.7f, 0.8f));

            for (int i = 0; i < 3; i++)
            {
                cards[i].transform.position = new Vector2(rects[i].position.x, rects[i].position.y - 1000f);
                cards[i].transform.localScale = Vector3.one * 0.5f;
                seq.Append(cards[i].transform.DOMoveY(rects[i].position.y, 0.65f).SetEase(Ease.InOutBack));
                seq.Join(cards[i].transform.DOScale(1f, 0.65f));
            }
        }

        private void HandleStartCardSelect(CardSelectStartEvent channel)
        {
            cardGroup.SetActive(true);
            CardAnimation();
            InitCard();
        }
        
        private void HandleCardSelected(CardSelectEvent channel)
        {
            cards.ToList().ForEach(card => card.cardButton.onClick.RemoveAllListeners());
            Sequence seq = DOTween.Sequence();

            channel.Card.IsSelected = true;
            string eventName = channel.CardSO.eventName;
            eventChannel.RaiseEvent(CardEventChannel.GetCardEvent(eventName));
            
            seq.Append(channel.Card.transform.DOScale(1.4f, 0.5f));

            for (int i = 0; i < 3; i++)
            {
                seq.Append(cards[i].transform.DOMoveY(2000, 0.5f).SetEase(Ease.InBack));
            }
            
            seq.Append(background.DOFade(0, 0.8f)).OnComplete(() =>
            {
                cardGroup.SetActive(false);
                Time.timeScale = _prevSped;
            });
        }
    }
}