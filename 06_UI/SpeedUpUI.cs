using UnityEngine;
using UnityEngine.UI;

namespace _01_Script._06_UI
{
    public class SpeedUpUI : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private Sprite normalIcon;
        [SerializeField] private Sprite speedIcon;
        [SerializeField] private Sprite superSpeedIcon;

        private int modeCnt = 0;

        public void OnSpeedChangeClick()
        {
            modeCnt++;
            
            if(modeCnt == 3)
                modeCnt = 0;

            if (modeCnt == 0)
            {
                icon.sprite = normalIcon;
                Time.timeScale = 1f;
            }
            else if (modeCnt == 1)
            {
                icon.sprite = speedIcon;
                Time.timeScale = 2f;
            }
            else if (modeCnt == 2)
            {
                icon.sprite = superSpeedIcon;
                Time.timeScale = 4f;
            }
        }
    }
}