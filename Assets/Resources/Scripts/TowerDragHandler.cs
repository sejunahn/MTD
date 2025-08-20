using UnityEngine;

public class TowerDragHandler : MonoBehaviour
{
    private Camera mainCam;
    private Vector3 offset;
    private bool isDragging = false;

    private TileController originalTile;
    private TileController targetTile;

    void Start()
    {
        mainCam = Camera.main;
    }

    void OnMouseDown()
    {
        // 드래그 시작
        isDragging = true;
        offset = transform.position - GetMouseWorldPos();

        // 원래 있던 타일 저장
        originalTile = GetTileUnderTower();
    }

    void OnMouseDrag()
    {
        if (!isDragging) return;

        // 마우스 위치로 이동 (y=0 고정)
        Vector3 pos = GetMouseWorldPos() + offset;
        pos.y = 0; 
        transform.position = pos;
    }

    void OnMouseUp()
    {
        isDragging = false;

        // 드롭 위치 확인
        targetTile = GetTileUnderPointer();
        if (targetTile != null)
        {
            if (targetTile.IsEmpty)
            {
                MoveToTile(targetTile);
            }
            else if (targetTile.towerOnTile != null)
            {
                TryMergeOrSwap(targetTile);
            }
        }
        else
        {
            // 타일 아니면 원위치 복귀
            transform.position = originalTile.transform.position;
        }
    }

    private Vector3 GetMouseWorldPos()
    {
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        Plane ground = new Plane(Vector3.up, Vector3.zero);
        if (ground.Raycast(ray, out float dist))
        {
            return ray.GetPoint(dist);
        }
        return Vector3.zero;
    }

    private TileController GetTileUnderTower()
    {
        Collider col = Physics.OverlapSphere(transform.position, 0.1f)[0];
        return col.GetComponent<TileController>();
    }

    private TileController GetTileUnderPointer()
    {
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            return hit.collider.GetComponent<TileController>();
        }
        return null;
    }

    private void MoveToTile(TileController tile)
    {
        if (originalTile != null) originalTile.towerOnTile = null;

        transform.position = tile.transform.position;
        tile.towerOnTile = gameObject;
    }

    private void TryMergeOrSwap(TileController tile)
    {
        var other = tile.towerOnTile;
        if (other != null && other.name == gameObject.name)
        {
            // 합성
            Debug.Log("합성 성공!");
            Destroy(other);

            // 업그레이드된 타워 프리팹 생성 (임시로 자기 자신 재생성)
            GameObject merged = Instantiate(gameObject, tile.transform.position, Quaternion.identity);
            tile.towerOnTile = merged;

            Destroy(gameObject);
        }
        else
        {
            // 교환
            Vector3 tempPos = other.transform.position;
            other.transform.position = originalTile.transform.position;
            transform.position = tempPos;

            tile.towerOnTile = gameObject;
            originalTile.towerOnTile = other;
        }
    }
}
