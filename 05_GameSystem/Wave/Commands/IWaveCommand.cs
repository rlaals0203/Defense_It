using System;

namespace _01_Script._05_GameSystem.Wave
{
    public interface IWaveCommand
    {
        public void StartCommand(Action onComplete);
        public void UpdateCommand(float deltaTime);
    }
}