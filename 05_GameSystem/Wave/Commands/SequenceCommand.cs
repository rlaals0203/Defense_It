using System;
using System.Collections.Generic;

namespace _01_Script._05_GameSystem.Wave
{
    public class SequenceCommand : IWaveCommand
    {
        private readonly Queue<IWaveCommand> _commands;
        private IWaveCommand _current;

        private Action _onComplete;

        public SequenceCommand(List<IWaveCommand> commands)
        {
            _commands = new Queue<IWaveCommand>(commands);
        }

        public void StartCommand(Action onComplete)
        {
            _onComplete = onComplete;
            ChangeCommand();
        }

        private void ChangeCommand()
        {
            if (_commands.Count == 0)
            {
                _onComplete?.Invoke();
                return;
            }

            _current = _commands.Dequeue();
            _current.StartCommand(ChangeCommand);
        }

        public void UpdateCommand(float dt)
        {
            _current?.UpdateCommand(dt);
        }
    }
}