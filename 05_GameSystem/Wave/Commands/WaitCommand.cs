using System;
using UnityEngine;

namespace _01_Script._05_GameSystem.Wave
{
    public class WaitCommand : IWaveCommand
    {
        private readonly float _time;
        private float _currentTime;
        
        private Action _onComplete;

        public WaitCommand(float time)
        {
            _time = time;
        }

        public void StartCommand(Action onComplete)
        {
            _onComplete = onComplete;
            _currentTime = 0f;
        }

        public void UpdateCommand(float deltaTime)
        {
            _currentTime += deltaTime;

            if (_currentTime >= _time)
            {
                _onComplete?.Invoke();
            }
        }
    }
}