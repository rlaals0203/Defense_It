using System;
using System.Collections.Generic;
using MinLibrary.Dependencies;
using MinLibrary.ObjectPool.Runtime;
using UnityEngine;

namespace _01_Script._05_GameSystem.Wave
{
    public class WaveCommandExecutor : MonoBehaviour
    {
        [Inject] private PoolManagerMono _poolManager;
        
        private readonly Queue<IWaveCommand> _commandQueue = new();
        private readonly List<IWaveCommand> _joinCommands = new();
        private IWaveCommand _currentCommand;
        
        public event Action OnWaveComplete; 
        
        public void InitStage(WaveSO waveData, Transform spawnTransform)
        {
            foreach (var data in waveData.spawnData)
            {
                var commands = data.GenerateCommands(spawnTransform.position, _poolManager);
                var sequence = new SequenceCommand(commands);
                AddCommand(sequence, data);
            }
        }

        private void AddCommand(IWaveCommand command, SpawnData spawnData)
        {
            if (spawnData.isJoin)
            {
                _joinCommands.Add(command);
            }
            else
            {
                _commandQueue.Enqueue(command);

                if (_currentCommand == null)
                    ChangeCommand();
            }
        }

        private void ChangeCommand()
        {
            if (_commandQueue.Count == 0)
            {
                if (_joinCommands.Count == 0)
                {
                    OnWaveComplete?.Invoke();
                }
                return;
            }
            
            _currentCommand = _commandQueue.Dequeue();
            
            _currentCommand.StartCommand(() => 
            {
                _currentCommand = null;
                ChangeCommand();
            });
            
            for (int i = 0; i < _joinCommands.Count; i++)
            {
                var command = _joinCommands[i];
                command.StartCommand(() => _joinCommands.Remove(command));
            }
        }

        private void Update()
        {
            _currentCommand?.UpdateCommand(Time.deltaTime);

            if (_joinCommands.Count > 0)
            {
                for (int i = _joinCommands.Count - 1; i >= 0; i--)
                {
                    _joinCommands[i].UpdateCommand(Time.deltaTime);
                }
            }
        }
    }
}