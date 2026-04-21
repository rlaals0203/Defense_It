using _01_Script._05_GameSystem.ItemDrop;
using _01_Script.Core.ETC;
using Ami.BroAudio;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class PickUpItem : MonoBehaviour
{
    [SerializeField] private PlayerInputSO playerInput;
    [SerializeField] private RectTransform coinRect;
    [SerializeField] private RectTransform canvasRect;
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private LayerMask whatIsItem;
    [SerializeField] private SoundID pickUpSound;

    private Camera _cam;
    
    private void Awake()
    {
        _cam = Camera.main;
        playerInput.OnClickEvent += HandleClick;
    }

    private void OnDestroy()
    {
        playerInput.OnClickEvent -= HandleClick;
    }

    private void HandleClick()
    {
        Vector3 worldPos = playerInput.GetWorldPosition(out RaycastHit hitInfo, whatIsItem);

        if (hitInfo.collider != null 
            && hitInfo.collider.TryGetComponent(out IDropable dropable))
        {
            PickUp(dropable, worldPos);
        }
    }

    public void PickUp(IDropable dropable, Vector3 worldPos)
    {
        if (dropable is not ItemDrop itemDrop) return;
        dropable.PickupItem(() => PickUpAnim(worldPos, itemDrop.DropItemSO));
        BroAudio.Play(pickUpSound);
    }

    private void PickUpAnim(Vector3 worldPos, DropItemSO dropItem)
    {
        GameObject coinUI = Instantiate(coinPrefab, canvasRect);
        RectTransform coinUIRect = coinUI.GetComponent<RectTransform>();
        Vector3 screenPos = _cam.WorldToScreenPoint(worldPos);
        
        coinUIRect.transform.position = screenPos;
        coinUIRect.transform.localScale = Vector2.one * 2f;
        coinUIRect.transform.DOScale(1f, 1f);
        coinUIRect.DOMove(coinRect.transform.position, 1.5f).SetEase(Ease.InExpo).OnComplete(() =>
        {
            coinUI.SetActive(false);
            int coin = Random.Range(dropItem.minCoin, dropItem.maxCoin);
            CurrencyManager.Instance.ModifyCurrency(CurrencyType.Coin, coin);
        });
    }
}
