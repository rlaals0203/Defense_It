using System;
using _01_Script._00_Core.EventChannel;
using TMPro;
using UnityEngine;

public class WaveShowUI : MonoBehaviour
{
    [SerializeField] private GameEventChannelSO eventChannel;

    private TextMeshProUGUI _waveText;

    private void Awake()
    {
        _waveText = GetComponentInChildren<TextMeshProUGUI>();
        eventChannel.AddListener<WaveChangeEvent>(HandleWaveChange);
    }

    private void OnDestroy()
    {
        eventChannel.RemoveListener<WaveChangeEvent>(HandleWaveChange);
    }

    private void HandleWaveChange(WaveChangeEvent channel)
    {
        _waveText.text = $"웨이브 {channel.currentWave}";
    }
}
