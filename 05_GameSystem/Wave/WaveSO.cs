using System.Collections.Generic;
using _01_Script._03_Enemy;
using MinLibrary.ObjectPool.Runtime;
using UnityEngine;
using UnityEngine.Serialization;

namespace _01_Script._05_GameSystem.Wave
{
    [System.Serializable]
    public struct SpawnData
    {
        public EnemySO enemySO;
        public float startDelay;
        public float spawnDelay;
        public int spawnCount;
        
        [Header("Repeat Setting")]
        public int repeatCount;
        public float repeatDelay; 
        
        public bool isJoin;
        
        public List<IWaveCommand> GenerateCommands(Vector3 position, PoolManagerMono poolManager)
        {
            var list = new List<IWaveCommand>();

            if (startDelay > 0) 
                list.Add(new WaitCommand(startDelay));
            
            int spawnCount = Mathf.Max(1, this.spawnCount);
            int repeatCount = Mathf.Max(1, this.repeatCount);
            
            for (int i = 0; i < repeatCount; i++)
            {
                for (int j = 0; j < spawnCount; j++)
                {
                    list.Add(new EnemySpawnCommand(this, position, poolManager));

                    if (spawnDelay > 0)
                    {
                        list.Add(new WaitCommand(spawnDelay));
                    }
                }
                
                if (repeatDelay > 0)
                {
                    list.Add(new WaitCommand(repeatDelay));
                }
            }
 
            return list;
        }
    }
    
    
    [CreateAssetMenu(fileName = "WavePreset", menuName = "SO/Wave/WaveSetting")]
    public class WaveSO : ScriptableObject
    {
        public List<SpawnData> spawnData = new();
    }
}