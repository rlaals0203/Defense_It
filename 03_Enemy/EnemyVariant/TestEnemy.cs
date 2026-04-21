using System;
using _01_Script._03_Enemy.BT.Event;

namespace _01_Script._03_Enemy.EnemyVariant
{
    public class TestEnemy : Enemy
    {
        private StateChange _stateChannel;

        private void Start()
        {
            _stateChannel = GetBlackboardVariable<StateChange>("StateChange").Value;
        }
    }
}
