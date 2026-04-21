using _01_Script._01_Entities;
using UnityEngine;

namespace _01_Script._03_Enemy
{
    [CreateAssetMenu(fileName = "EnemySO", menuName = "SO/EnemySO", order = 0)]
    public class EnemySO : EntitySO
    {
        public int damage;
        public int requireWave;
        
        public int minCoin;
        public int maxCoin;
    }
}