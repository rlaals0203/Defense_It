using _01_Script._00_Core.EventChannel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _01_Script._06_UI
{
    public class UnitSkillUI : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO eventChannel;
        [SerializeField] private TextMeshProUGUI cooltimeText;
        [SerializeField] private Image skillIcon;
        
        private delegate void OnTrySKillEvent();
        private OnTrySKillEvent OnTrySkill;
        private EntitySkill _prevSkill;
        private Button _skillButton;

        private void Awake()
        {
            _skillButton = GetComponent<Button>();
            _skillButton.onClick.AddListener(OnSkillButtonClick);
            
            eventChannel.AddListener<UnitSelectEvent>(HandleUnitSelect);
        }

        private void Update()
        {
            if (_prevSkill == null)
                cooltimeText.text = string.Empty;
            else
                UpdateUI();

        }

        private void UpdateUI()
        {
            if (!_prevSkill.IsActive)
            {
                cooltimeText.text = string.Empty;
            }
            else if (_prevSkill.CanUse())
            {
                cooltimeText.text = $"사용";
            }
            else if(_prevSkill != null)
            {
                float remainTime = _prevSkill.SkillSO.cooltime - (Time.time - _prevSkill.UsedTime);
                cooltimeText.text = $"{Mathf.RoundToInt(remainTime)}";
            }
        }

        private void HandleUnitSelect(UnitSelectEvent channel)
        {
            if (_prevSkill != null)
                OnTrySkill -= _prevSkill.UseSkill;
            
            OnTrySkill += channel.unitSkill.UseSkill;
            _prevSkill = channel.unitSkill;
            skillIcon.sprite = channel.unitSkill.SkillSO.icon;
        }

        private void OnSkillButtonClick()
        {
            OnTrySkill?.Invoke();
        }

        private void OnDestroy()
        {
            _skillButton.onClick.RemoveListener(OnSkillButtonClick);
            eventChannel.RemoveListener<UnitSelectEvent>(HandleUnitSelect);
        }
    }
}