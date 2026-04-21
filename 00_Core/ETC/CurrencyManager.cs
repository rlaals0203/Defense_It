using System;
using System.Collections.Generic;
using System.IO;
using _01_Script._00_Core.EventChannel;
using UnityEngine;

public enum CurrencyType
{
    Coin, Gem
}

[System.Serializable]
public class CurrencyData
{
    public float gem;
}

[DefaultExecutionOrder(-10)]
public class CurrencyManager : MonoBehaviour
{
    public delegate void ValueChangedHanlder(CurrencyType type, float value);
    public static CurrencyManager Instance;
    
    [SerializeField] private GameEventChannelSO eventChannel;
    private Dictionary<CurrencyType, float> _currencyDic;
    private float _coinMultiplier = 1f;
    
    public event ValueChangedHanlder OnValueChanged;
    public event Action OnPurchaseFail;


    private void Awake()
    {
        _currencyDic = new Dictionary<CurrencyType, float>
        {
            { CurrencyType.Coin, 0 },
            { CurrencyType.Gem, 0 },
        };

        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
        
        eventChannel.AddListener<IncreaseCoinEvent>(HandleCoinIncrease);
        eventChannel.AddListener<MultiplyCoinEvent>(HandleCoinMultiply);
        
        LoadCurrency();
    }

    public void SetCurrency(CurrencyType currencyType, float value)
    {
        _currencyDic[currencyType] = value;
    }

    private void HandleCoinMultiply(MultiplyCoinEvent channel)
    {
        int coin = Mathf.RoundToInt(_currencyDic[CurrencyType.Coin]);
        ModifyCurrency(CurrencyType.Coin, coin * 2);
    }

    private void HandleCoinIncrease(IncreaseCoinEvent channel)
    {
        _coinMultiplier += 0.1f;
    }

    public float GetCurrency(CurrencyType currencyType)
    {
        if (!_currencyDic.ContainsKey(currencyType))
            return 0;
        
        return _currencyDic[currencyType];
    }

    public void ModifyCurrency(CurrencyType currencyType, float amount)
    {
        if (!_currencyDic.ContainsKey(currencyType))
            _currencyDic[currencyType] = 0;
        
        float finalAmount = currencyType == CurrencyType.Coin ? amount * _coinMultiplier : amount;
        _currencyDic[currencyType] += finalAmount;
        OnValueChanged?.Invoke(currencyType, _currencyDic[currencyType]);
        
        if (currencyType == CurrencyType.Gem) 
            SaveCurrency();
    }

    public bool TryModifyCurrency(CurrencyType currencyType, float amount)
    {
        if (!_currencyDic.ContainsKey(currencyType)){
            _currencyDic[currencyType] = 0;
            return false;
        }
        
        if (_currencyDic[currencyType] + amount < 0) {
            OnPurchaseFail?.Invoke();
            return false;
        }
        
        _currencyDic[currencyType] += amount;
        OnValueChanged?.Invoke(currencyType, _currencyDic[currencyType]);
        SaveCurrency();

        return true;
    }
    
    private const string CurrencyFile = "currency.json";
    
    private string GetCurrencyPath()
    {
        return Path.Combine(Application.persistentDataPath, CurrencyFile);
    }

    [ContextMenu("Save Currency")]
    public void SaveCurrency()
    {
        CurrencyData data = new CurrencyData
        {
            gem = _currencyDic[CurrencyType.Gem]
        };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(GetCurrencyPath(), json);
    }

    [ContextMenu("Load Currency")]
    public void LoadCurrency()
    {
        string path = GetCurrencyPath();
        if (!File.Exists(path))
        {
            Debug.LogWarning("Currency JSON not found.");
            return;
        }

        string json = File.ReadAllText(path);
        CurrencyData data = JsonUtility.FromJson<CurrencyData>(json);

        _currencyDic[CurrencyType.Gem] = data.gem;
        OnValueChanged?.Invoke(CurrencyType.Gem, data.gem);
    }

    [ContextMenu("Reset Currency")]
    public void ResetCurrency()
    {
        _currencyDic[CurrencyType.Gem] = 0;
        OnValueChanged?.Invoke(CurrencyType.Gem, 0);
        SaveCurrency();
    }

    private void OnDestroy()
    {
        eventChannel.RemoveListener<IncreaseCoinEvent>(HandleCoinIncrease);
        eventChannel.RemoveListener<MultiplyCoinEvent>(HandleCoinMultiply);
    }
}