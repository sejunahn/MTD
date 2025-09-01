using UnityEngine;

public class SimpleDragHandler : MonoBehaviour
{
    private Camera mainCam;
    private bool isDragging = false;
    private float zOffset;
    private TileController currentTile; 
    void Start()
    {
        mainCam = Camera.main;
    }

    void OnMouseDrag()
    {
        if (!isDragging) return;

        // 마우스 위치를 월드 좌표로 변환
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = zOffset; 
        Vector3 worldPos = mainCam.ScreenToWorldPoint(mousePos);

        transform.position = worldPos;
    }
    private void OnMouseDown()
    {
        isDragging = true;
        zOffset = mainCam.WorldToScreenPoint(transform.position).z;

        // 현재 위치 기준으로 타일을 저장
        currentTile = GetTileUnderPosition(transform.position);
    }

    void OnMouseUp()
    {
        isDragging = false;

        TileController targetTile = GetTileUnderPosition(transform.position);

        // 유효하지 않은 타일이면 원래 자리로 복귀
        if (targetTile == null || targetTile.tileType != TileType.Ground)
        {
            if (currentTile != null)
                transform.position = currentTile.transform.position;
            return;
        }

        // ⬇️ 여기서 같은 타일인지 먼저 체크
        if (targetTile == currentTile)
        {
            // 같은 자리면 그냥 제자리 복귀, 합성/스왑 로직 스킵
            transform.position = currentTile.transform.position;
            return;
        }

        // 빈 타일이면 이동
        if (targetTile.IsEmpty)
        {
            MoveToTile(targetTile);
        }
        else
        {
            // 다른 타일에 타워가 있으면 합성 or 자리교환
            TryMergeOrSwap(targetTile);
        }
    }

    private TileController GetTileUnderPosition(Vector3 pos)
    {
        Ray ray = new Ray(pos + Vector3.up * 5f, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 10f))
        {
            return hit.collider.GetComponent<TileController>();
        }
        return null;
    }
    
    private void MoveToTile(TileController tile)
    {
        // 원래 타일 비우기
        if (currentTile != null)
            currentTile.towerOnTile = null;

        // 새 타일에 자신 배치
        transform.position = tile.transform.position;
        tile.towerOnTile = gameObject;

        // 새 타일을 "현재 타일"로 갱신
        currentTile = tile;
    }

    private void TryMergeOrSwap(TileController tile)
    {
        GameObject other = tile.towerOnTile;

        // 같은 종류라면 합성
        if (other != null && other.name == gameObject.name)
        {
            Debug.Log("합성 성공!");

            // 임시: 자기 자신을 "업그레이드된 버전"으로 재생성
            Destroy(other);
            GameObject merged = Instantiate(gameObject, tile.transform.position, Quaternion.identity);

            tile.towerOnTile = merged;
            Destroy(gameObject);
        }
        else
        {
            // 교환 처리
            Debug.Log("자리 교환!");

            Vector3 tempPos = other.transform.position;
            other.transform.position = currentTile.transform.position;
            transform.position = tempPos;

            // towerOnTile 교체
            currentTile.towerOnTile = other;
            tile.towerOnTile = gameObject;

            // 자리 이동 후 originalTile 갱신
            currentTile = tile;
        }
    }

}