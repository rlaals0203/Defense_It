using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _01_Script._00_Core.ETC
{
    public class CurrencyViewer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI currencyText;
        [SerializeField] private TextMeshProUGUI failWarningText;
        [SerializeField] private CurrencyType currencyType = CurrencyType.Coin;
        
        private Tween failTween;

        private void Awake()
        {
            CurrencyManager.Instance.OnValueChanged += HandleValueChanged;
            CurrencyManager.Instance.OnPurchaseFail += HandlePurchaseFail;
            CurrencyManager.Instance.ModifyCurrency(currencyType, 0f);
        }

        private void OnDestroy()
        {
            CurrencyManager.Instance.OnValueChanged -= HandleValueChanged;
        }

        private void HandleValueChanged(CurrencyType type, float value)
        {
            if (currencyType != type) return;
            
            currencyText.transform.DOScale(1.3f, 0.1f).OnComplete(() =>
            {
                currencyText.transform.DOScale(1f, 0.1f);
            });
            
            currencyText.text = $"{Mathf.RoundToInt(value)}";
        }
        
        private void HandlePurchaseFail()
        {
            failTween?.Kill();
            failWarningText.alpha = 1f;
            failTween = failWarningText.DOFade(0f, 2f);
        }

        [ContextMenu("TestCurrency")]
        private void TestCurrency()
        {
            CurrencyManager.Instance.ModifyCurrency(currencyType, 1000f);
        }
    }
}