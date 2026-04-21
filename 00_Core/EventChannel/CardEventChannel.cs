using _01_Script._06_UI.CardSelect;
using UnityEngine;

namespace _01_Script._00_Core.EventChannel
{
    public class CardEventChannel
    {
        public static readonly CardSelectStartEvent CardSelectStartEvent = new CardSelectStartEvent();
        public static readonly CardSelectEvent CardSelectEvent = new CardSelectEvent();
        public static readonly AutoCoinCollectEvent AutoCoinCollectEvent = new AutoCoinCollectEvent();
        public static readonly IncreaseCoinEvent IncreaseCoinEvent = new IncreaseCoinEvent();
        public static readonly ClickAttackPowerEvent ClickAttackPowerEvent = new ClickAttackPowerEvent();
        public static readonly ChestChanceEvent ChestChanceEvent = new ChestChanceEvent();
        public static readonly TowerHealthEvent TowerHealthEvent = new TowerHealthEvent();
        public static readonly MultiplyCoinEvent MultiplyCoinEvent = new MultiplyCoinEvent();

        public static GameEvent GetCardEvent(string eventName)
        {
            GameEvent gameEvent;

            switch (eventName)
            {
                case "AutoCoinCollectEvent":
                    gameEvent = AutoCoinCollectEvent;
                    break;
                case "IncreaseCoinEvent":
                    gameEvent = IncreaseCoinEvent;
                    break;
                case "ClickAttackPowerEvent":
                    gameEvent = ClickAttackPowerEvent;
                    break;
                case "ChestChanceEvent":
                    gameEvent = ChestChanceEvent;
                    break;
                case "TowerHealthEvent":
                    gameEvent = TowerHealthEvent;
                    break;
                case "MultiplyCoinEvent":
                    gameEvent = MultiplyCoinEvent;
                    break;
                default:
                    gameEvent = null;
                    break;
            }
            
            Debug.Log(gameEvent);
            return gameEvent;
        }
    }
    
    public class CardSelectStartEvent : GameEvent
    {
    }
    
    public class CardSelectEvent : GameEvent
    {
        public Card Card;
        public CardSO CardSO;
    
        public CardSelectEvent Initializer(Card card, CardSO cardSO)
        {
            Card = card;
            CardSO = cardSO;
            return this;
        }
    }
    
    public class AutoCoinCollectEvent : GameEvent {}

    public class IncreaseCoinEvent : GameEvent { }

    public class ClickAttackPowerEvent : GameEvent {}

    public class ChestChanceEvent : GameEvent {}

    public class TowerHealthEvent : GameEvent {}

    public class MultiplyCoinEvent : GameEvent {}
}