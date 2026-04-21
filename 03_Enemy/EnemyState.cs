using Unity.Behavior;
using UnityEngine;

namespace _01_Script._03_Enemy
{
    [BlackboardEnum]
    public enum EnemyState
    {
        IDLE = 1,
        MOVE = 2,
        ATTACK = 3,
        HIT = 4,
        DEAD = 5
    }
}