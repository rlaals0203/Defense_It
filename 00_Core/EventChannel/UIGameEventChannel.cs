using _01_Script._02_Unit;

namespace _01_Script._00_Core.EventChannel
{
    public class UIGameEventChannel
    {
        public static readonly UnitEquipStartEvent UnitEquipStartEvent = new UnitEquipStartEvent();
        public static readonly UnitEquipEvent UnitEquipEvent = new UnitEquipEvent();
        public static readonly UnitPurchaseEvent UnitPurchaseEvent = new UnitPurchaseEvent();
        public static readonly FadeEvent FadeEvent = new FadeEvent();
        public static readonly FadeCompleteEvent FadeCompleteEvent = new FadeCompleteEvent();
    }

    public class UnitEquipStartEvent : GameEvent
    {
        public UnitSO Unit;
        public UnitEquipStartEvent Initializer(UnitSO unit)
        {
            Unit = unit;
            return this;
        }
    }

    public class UnitEquipEvent : GameEvent
    {
        public int SlotIdx;
        public UnitSO Unit;
        
        public UnitEquipEvent Initializer(int idx, UnitSO unit)
        {
            SlotIdx = idx;
            Unit = unit;
            return this;
        }
    }
    
    public class UnitPurchaseEvent : GameEvent
    {
        public UnitSO Unit;
        public UnitPurchaseEvent Initializer(UnitSO unit)
        {
            Unit = unit;
            return this;
        }
    }
    
    public class FadeEvent : GameEvent
    {
        public bool isFadeIn;
        public float fadeTime;
        public bool isSaveOrLoad;
    }
    
    public class FadeCompleteEvent : GameEvent
    {
    }
}