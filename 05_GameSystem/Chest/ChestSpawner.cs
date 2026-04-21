using System;
using _01_Script._00_Core.EventChannel;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace _01_Script._05_GameSystem
{
    public class ChestSpawner : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO eventChannel;
        [SerializeField] private GameObject chestPrefab;
        [SerializeField] private float dropRange;

        [SerializeField] private LayerMask whatIsSpawnable;
        [SerializeField] private LayerMask whatIsNotSpawnable;

        private int _currentWave = 0;
        private float _chanceToSpawn = 0.5f;
        private float _time;

        private void Start()
        {
            _time = Time.time;
            eventChannel.AddListener<WaveChangeEvent>(HandleWaveChange);
            eventChannel.AddListener<ChestChanceEvent>(HandleChestChance);
        }

        private void OnDestroy()
        {
            eventChannel.RemoveListener<WaveChangeEvent>(HandleWaveChange);
            eventChannel.RemoveListener<ChestChanceEvent>(HandleChestChance);
        }
        
        private void HandleChestChance(ChestChanceEvent channel) => _chanceToSpawn += 0.5f;

        private void Update()
        {
            if (Time.time - _time >= 1)
            {
                float rand = UnityEngine.Random.Range(0f, 100f);

                if (rand <= _chanceToSpawn)
                    TrySpawnChest();
                
                _time = Time.time;
            }
        }
        
        private void HandleWaveChange(WaveChangeEvent channel)
        {
            _currentWave = channel.currentWave - 1;
        }

        private void TrySpawnChest()
        {
            Vector3 offset = UnityEngine.Random.insideUnitSphere * dropRange;
            Vector3 position = Vector3.zero + offset;

            if (CheckLayer(position, whatIsSpawnable) && !CheckLayer(position, whatIsNotSpawnable))
            {
                position.y = 0f;
                SpawnChest(position);
            }
            else
            {
                TrySpawnChest();
            }
        }

        private void SpawnChest(Vector3 position)
        {
            ChestItem chest = Instantiate(chestPrefab, position, Quaternion.identity).GetComponent<ChestItem>();
            chest.AdditionalHp = _currentWave * 25f;
        }
        
        private bool CheckLayer(Vector3 position, LayerMask whatisTarget)
        {
            position.y = 10f;
            return Physics.Raycast(position, -Vector3.up,Mathf.Infinity, whatisTarget);
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, dropRange);
        }
    }
}