using System;
using Ami.BroAudio;
using UnityEngine;
using UnityEngine.UI;

public class SoundSetting : MonoBehaviour
{
    [SerializeField] private BroAudioType bgm;
    [SerializeField] private BroAudioType sfx;
    [SerializeField] private Slider slider;

    private void Awake()
    {
        slider.onValueChanged.AddListener(SFX);
    }

    private void OnDestroy()
    {
        slider.onValueChanged.RemoveListener(SFX);
    }

    public void BGM(float volume)
    {
        BroAudio.SetVolume(bgm, volume);
    }

    public void SFX(float volume)
    {
        BroAudio.SetVolume(sfx,volume);
    }
}
