using UnityEngine;
using UnityEngine.EventSystems;

public class TowerDragHandler : MonoBehaviour, 
    IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector3 originalPos;
    private TileController originalTile;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = FindFirstObjectByType<Canvas>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // 정보 팝업 띄우기
        Debug.Log("타워 정보 팝업");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalPos = transform.position;
        originalTile = GetCurrentTile();
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position += (Vector3)eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        TileController targetTile = GetTileUnderPointer(eventData);
        if (targetTile != null)
        {
            if (targetTile.IsEmpty)
            {
                // 이동
                MoveToTile(targetTile);
            }
            else
            {
                // 합성 체크
                TryMerge(targetTile);
            }
        }
        else
        {
            // 원래 자리로 복귀
            transform.position = originalPos;
        }
    }

    private TileController GetCurrentTile()
    {
        return GetComponentInParent<TileController>();
    }

    private TileController GetTileUnderPointer(PointerEventData eventData)
    {
        var obj = eventData.pointerCurrentRaycast.gameObject;
        return obj ? obj.GetComponent<TileController>() : null;
    }

    private void MoveToTile(TileController targetTile)
    {
        if (originalTile != null) originalTile.towerOnTile = null;
        targetTile.towerOnTile = gameObject;
        transform.position = targetTile.transform.position;
    }

    private void TryMerge(TileController targetTile)
    {
        var otherTower = targetTile.towerOnTile;
        if (otherTower != null && otherTower.name == gameObject.name)
        {
            Debug.Log("합성 성공!");
            Destroy(otherTower);
            // 업그레이드된 타워 생성
            GameObject merged = Instantiate(gameObject, targetTile.transform.position, Quaternion.identity);
            targetTile.towerOnTile = merged;
            Destroy(gameObject);
        }
        else
        {
            // 교환
            Vector3 tempPos = otherTower.transform.position;
            otherTower.transform.position = originalPos;
            transform.position = tempPos;
        }
    }
}
