using System;
using TMPro;
using UnityEngine;
using UnityEngine.Video;

namespace _01_Script._06_UI.Tutorial
{
    public class Tutorial : MonoBehaviour
    {
        [TextArea] public string[] texts;
        public VideoClip[] videoClips;
        
        [SerializeField] private VideoPlayer videoPlayer;
        [SerializeField] private TextMeshProUGUI description;

        private int _idx = 0;

        private void Awake()
        {
            ShowTutorial();
        }

        private void ShowTutorial()
        {
            videoPlayer.clip = videoClips[_idx];
            description.text = texts[_idx];
        }

        public void ShowNext()
        {
            _idx++;
            
            if(_idx >= texts.Length)
                _idx = 0;
            
            ShowTutorial();
        }
    }
}
