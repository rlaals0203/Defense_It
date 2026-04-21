using System;
using _01_Script._00_Core.EventChannel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _01_Script._06_UI
{
    public class TowerHealthUI : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO eventChannel;
        [SerializeField] private Image fillImage;
        [SerializeField] private TextMeshProUGUI healthText;

        private void Awake()
        {
            eventChannel.AddListener<TowerAttackEvent>(HandleTowerAttack);
        }

        private void OnDestroy()
        {
            eventChannel.RemoveListener<TowerAttackEvent>(HandleTowerAttack);
        }

        private void HandleTowerAttack(TowerAttackEvent channel)
        {
            float health = Mathf.Clamp(channel.currentHealth, 0f, 1000f);
            healthText.text = $"{health} / 500";
            fillImage.rectTransform.sizeDelta = 
                new Vector2((channel.currentHealth / 500f) * 600f, fillImage.rectTransform.sizeDelta.y);
        }
    }
}