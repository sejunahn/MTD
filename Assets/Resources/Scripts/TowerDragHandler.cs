using UnityEngine;

public class TowerDragHandler : MonoBehaviour
{
    private ITower tower;
    private Camera mainCam;
    private bool isDragging = false;
    private float zOffset;

    private TileController currentTile; // 현재 올라가 있는 타일

    void Start()
    {
        mainCam = Camera.main;
        // 시작 시 타일과 연동
        
    }

    void OnMouseDown()
    {
        isDragging = true;
        zOffset = mainCam.WorldToScreenPoint(transform.position).z;
        currentTile = GetTileUnderPosition(transform.position);
    }

    void OnMouseDrag()
    {
        if (!isDragging) return;

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = zOffset;
        Vector3 worldPos = mainCam.ScreenToWorldPoint(mousePos);

        transform.position = worldPos;
    }

    void OnMouseUp()
    {
        isDragging = false;

        // 마우스 뗐을 때, 현재 위치 기준으로 타일 찾기
        TileController targetTile = GetTileUnderPosition(transform.position);
        if (targetTile == null || targetTile.tileType != TileType.Ground)
            return;
        
        if (targetTile != null && targetTile.IsEmpty)
        {
            // // 타일 위에 정확히 위치시킴
            // transform.position = targetTile.transform.position;
            // targetTile.towerOnTile = gameObject;
            // currentTile.towerOnTile = null;
            // currentTile = targetTile;
            MoveToTile(targetTile);
        }
        else
        {
            // if (currentTile != null)
            //     transform.position = currentTile.transform.position;
            TryMergeOrSwap(targetTile);

        }
    }
    private void MoveToTile(TileController targetTile)
    {
        // 원래 타일 비우기
        if (currentTile != null)
            currentTile.towerOnTile = null;

        // 새 타일에 자신 배치
        transform.position = targetTile.transform.position;
        targetTile.towerOnTile = gameObject;

        // 새 타일을 "현재 타일"로 갱신
        currentTile = targetTile;
    }
    
    private void TryMergeOrSwap(TileController tile)
    {
        GameObject other = tile.towerOnTile;
        if (other == null) return;

        var thisTower = GetComponent<ITower>();
        var otherTower = other.GetComponent<ITower>();

        if (thisTower != null && otherTower != null && thisTower.Id == otherTower.Id)
        {
            // 같은 타워라면 합성
            Debug.Log($"합성 성공! {thisTower.TowerName}");

            Destroy(other);
            Destroy(gameObject);

            // 업그레이드된 타워 프리팹 생성 (임시: 자기 자신 복제 후 Upgrade 호출)
            GameObject merged = Instantiate(other, tile.transform.position, Quaternion.identity);
            var mergedTower = merged.GetComponent<ITower>();
            mergedTower?.Upgrade();

            tile.towerOnTile = merged;
        }
        else
        {
            // 자리 교환
            Debug.Log("자리 교환!");

            Vector3 tempPos = other.transform.position;
            other.transform.position = currentTile.transform.position;
            transform.position = tempPos;

            currentTile.towerOnTile = other;
            tile.towerOnTile = gameObject;
            currentTile = tile;
        }
    }

    
    private TileController GetTileUnderPosition(Vector3 pos)
    {
        Ray ray = new Ray(pos + Vector3.up * 2f, Vector3.down); // 조금만 위에서 쏨
        if (Physics.Raycast(ray, out RaycastHit hit, 10f)) // 타일 전용 Layer
        {
            return hit.collider.GetComponent<TileController>();
        }
        return null;
    }

}