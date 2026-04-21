using MinLibrary.ObjectPool.Runtime;
using UnityEngine;

namespace _01_Script._02_Unit.Skill
{
    [CreateAssetMenu(fileName = "SkillSO", menuName = "SO/Skill", order = 0)]
    public class SkillSO : ScriptableObject
    {
        public Sprite icon;
        public int damage;
        public float cooltime;
        public float attackTime;
        public float skillDuration;
        public PoolItemSO effectPool;
    }
}