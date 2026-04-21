using UnityEngine;

namespace _01_Script._00_Core.EventChannel
{
    public class GameSystemEventChannel : MonoBehaviour
    {
        public static readonly TowerAttackEvent TowerAttackEvent = new TowerAttackEvent();
        public static readonly MiningUIOpenEvent MiningUIOpenEvent = new MiningUIOpenEvent();
        public static readonly MiningUpgradeEvent MiningUpgradeEvent = new MiningUpgradeEvent();
        public static readonly GameOverEvent GameOverEvent = new GameOverEvent();
    }

    public class TowerAttackEvent : GameEvent
    {
        public float currentHealth;
        
        public TowerAttackEvent Initializer(float health)
        {
            currentHealth = health;
            return this;
        }
    }
    public class MiningUIOpenEvent : GameEvent { }
    
    public class MiningUpgradeEvent : GameEvent { }
    
    public class GameOverEvent : GameEvent { }
}