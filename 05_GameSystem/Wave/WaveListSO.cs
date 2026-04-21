using System.Collections.Generic;
using UnityEngine;

namespace _01_Script._05_GameSystem.Wave
{
    [CreateAssetMenu(fileName = "WaveSO", menuName = "SO/Wave/WaveSO", order = 0)]
    public class WaveListSO : ScriptableObject
    {
        public List<WaveSO> waveList = new();
    }
}