using _01_Script._00_Core.EventChannel;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GameCompleteUI : MonoBehaviour
{
    [SerializeField] private GameEventChannelSO eventChannel;
    [SerializeField] private Image background;
    [SerializeField] private GameObject gameOverUI;
    
    private RectTransform _rect;
    
    private void Awake()
    {
        eventChannel.AddListener<GameCompleteEvent>(HandleGameComplete);
        _rect = gameOverUI.GetComponent<RectTransform>();
        background.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        eventChannel.RemoveListener<GameCompleteEvent>(HandleGameComplete);
    }

    public void HandleGameComplete(GameCompleteEvent channel)
    {
        background.gameObject.SetActive(true);
        background.DOFade(0.7f, 0.5f);
        _rect.DOAnchorPosY(-1000f, 0.5f);
    }
}
