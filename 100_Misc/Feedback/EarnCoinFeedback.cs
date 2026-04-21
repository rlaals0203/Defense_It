using _01_Script._00_Core.ETC;
using _01_Script._03_Enemy;
using UnityEngine;

public class EarnCoinFeedback : Feedback
{
    [SerializeField] private Enemy enemy;

    public override void CreateFeedback()
    {
        int coin = Random.Range(enemy.EnemySO.minCoin, enemy.EnemySO.minCoin);
        CurrencyManager.Instance.ModifyCurrency(CurrencyType.Coin, coin);
    }

    public override void StopFeedback() { }
}
