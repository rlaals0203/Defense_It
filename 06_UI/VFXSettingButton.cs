using System;
using _01_Script._00_Core.EventChannel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VFXSettingButton : MonoBehaviour
{
    [SerializeField] private GameEventChannelSO eventChannel;

    private TextMeshProUGUI _onOffText;
    private Button _button;
    private ChangeSFXEvent _changeSFXEvent = UnitGameEventChannel.ChangeSFXEvent;

    private bool _isActive = true;

    private void Awake()
    {   
        _button = GetComponent<Button>();
        _onOffText = GetComponentInChildren<TextMeshProUGUI>();
        _button.onClick.AddListener(HandleClick);
    }

    private void HandleClick()
    {
        _isActive = !_isActive;
        eventChannel.RaiseEvent(_changeSFXEvent.Initializer(_isActive));
        
        _onOffText.text = _isActive ? "X" : "O";
    }
}
