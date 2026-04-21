using System;
using _01_Script._03_Enemy;
using MinLibrary.ObjectPool.Runtime;
using UnityEngine;

namespace _01_Script._05_GameSystem.Wave
{
    public class EnemySpawnCommand : IWaveCommand
    {
        private SpawnData _spawnData;
        private Vector3 _spawnPos; 
        private PoolManagerMono _poolManager;
        
        private Action _onComplete;
        
        public EnemySpawnCommand(SpawnData spawnData, Vector3 spawnPos, PoolManagerMono pool)
        {
            _spawnData = spawnData;
            _spawnPos = spawnPos;
            _poolManager = pool;
        }
        
        public void StartCommand(Action onComplete)
        {
            _onComplete = onComplete;
            SpawnEnemy();
        }

        public void UpdateCommand(float deltaTime) { }
        
        public void SpawnEnemy()
        {
            var enemy = _poolManager.Pop<Enemy>(_spawnData.enemySO.entityPool, _spawnPos);
            enemy.IsActive = true;
            _onComplete?.Invoke();
        }
    }
}