using _01_Script._03_Enemy;
using _01_Script._04_Combat;
using _01_Script.Core.ETC;
using _01_Script.Entities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _01_Script._06_UI
{
    public class EnemyInformation : MonoBehaviour
    {
        [SerializeField] private PlayerInputSO playerInput;
        [SerializeField] private LayerMask whatIsEnemy;
        [SerializeField] private Canvas canvas;
        [SerializeField] private GameObject infoGroup;
        [SerializeField] private RectTransform infoRect;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI healthText;
        [SerializeField] private Image fillImage;
        [SerializeField] private Vector3 offset = new Vector3(0, 100f, 0);

        private EntityHealth _healthCompo;
        private Entity _prevEnemy;

        private void Update()
        {
            UpdateEnemyInfo();
        }

        private void UpdateEnemyInfo()
        {
            playerInput.GetWorldPosition(out RaycastHit hit, whatIsEnemy);

            if (hit.collider == null || !hit.collider.TryGetComponent(out Enemy enemy))
            {
                infoGroup.SetActive(false);
                _prevEnemy = null;
                _healthCompo = null;
                return;
            }

            if (_prevEnemy != enemy)
            {
                _prevEnemy = enemy;
                _healthCompo = enemy.GetCompo<EntityHealth>();
                nameText.text = enemy.EnemySO.entityName;
            }

            infoGroup.SetActive(true);
            UpdateHealthUI();
            UpdatePosition(enemy.transform.position);
        }

        private void UpdateHealthUI()
        {
            if (_healthCompo == null) return;

            float current = _healthCompo.CurrentHealth;
            float max = _healthCompo.MaxHealth;

            healthText.text = $"{Mathf.Round(current * 10f) / 10f}/{max}";
            fillImage.fillAmount = current / max;
        }

        private void UpdatePosition(Vector3 worldPos)
        {
            Vector3 targetPos = worldPos + Vector3.up * 2f;
            Vector3 screenPos = Camera.main.WorldToScreenPoint(targetPos);

            if (canvas.renderMode == RenderMode.ScreenSpaceCamera || canvas.renderMode == RenderMode.WorldSpace)
            {
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                        canvas.transform as RectTransform, screenPos, 
                        canvas.worldCamera, out Vector2 localPoint)) {
                    infoRect.localPosition = localPoint + (Vector2)offset;
                }
            }
            else
                infoRect.position = screenPos + offset;
        }
    }
}