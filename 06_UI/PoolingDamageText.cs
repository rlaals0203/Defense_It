using System.Collections.Generic;
using DG.Tweening;
using MinLibrary.ObjectPool.Runtime;
using TMPro;
using UnityEngine;

public class PoolingDamageText : MonoBehaviour, IPoolable
{
    [field:SerializeField] public PoolItemSO PoolItem { get; private set; }
    [SerializeField] private TextMeshPro damageText;
    
    public GameObject GameObject => gameObject;
    
    private Pool _pool;
    
    public async void SetDamageText(string text)
    {
        damageText.text = text;
        
        await Awaitable.WaitForSecondsAsync(1f);
        _pool.Push(this);
    }
    
    public void CriticalText()
    {
        damageText.transform.DOScale(1.5f, 0.3f);
    }
    
    public void SetUpPool(Pool pool)
    {
        _pool = pool;
    }

    public void ResetItem()
    {
        damageText.DOKill();
        damageText.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        damageText.transform.localScale = Vector3.one;
    }
}
