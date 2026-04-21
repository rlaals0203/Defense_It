using _01_Script._00_Core.EventChannel;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _01_Script._00_Core.ETC
{
    public class SceneChanger : MonoBehaviour
    {
        public string sceneName;
        
        [SerializeField] private GameEventChannelSO uiChannel;
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(ChangeScene);
        }

        public void ChangeScene()
        {
            FadeEvent fadeEvt = UIGameEventChannel.FadeEvent;
            fadeEvt.isFadeIn = false;
            fadeEvt.fadeTime = 0.5f;
                
            uiChannel.AddListener<FadeCompleteEvent>(HandleFadeComplete);
            uiChannel.RaiseEvent(fadeEvt);
        }

        private void HandleFadeComplete(FadeCompleteEvent obj)
        {
            uiChannel.RemoveListener<FadeCompleteEvent>(HandleFadeComplete);
            SceneManager.LoadScene(sceneName);
        }
    }
}