using _01_Script._00_Core.StatSystem;
using UnityEngine;

namespace _01_Script._06_UI.CardSelect
{
    [CreateAssetMenu(fileName = "CardSO", menuName = "SO/Card", order = 0)]
    public class CardSO : ScriptableObject
    {
        public string eventName;
        public string description;
        public Sprite icon;
    }
}