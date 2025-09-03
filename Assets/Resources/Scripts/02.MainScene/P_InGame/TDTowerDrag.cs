using UnityEngine;
using UnityEngine.EventSystems;

public class TDTowerDrag : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler, IBeginDragHandler, IEndDragHandler
{
    private Vector3 offset;
    public TDMapGenerator mapGen;
    private TDTileData currentTile;
    private Vector3 startPos; // �巡�� ���� ��ġ
    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // TowerManager���� ó�� ���� �� �Ҹ�
    public void InitTile(TDTileData startTile)
    {
        currentTile = startTile;
        currentTile.isOccupied = true;
        transform.position = currentTile.transform.position;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        startPos = transform.position;

        // �巡�� ���� �� ���� Ÿ�� ����ֱ�
        if (currentTile != null)
        {
            currentTile.isOccupied = false;
            //currentTile = null;
        }

        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(eventData.position);
        offset = transform.position - new Vector3(worldPoint.x, worldPoint.y, 0);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // �巡�� ���� �� �׻� �� ���� ���̵���
        if (sr != null)
            sr.sortingOrder = 100;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(eventData.position);
        Vector3 newPos = new Vector3(worldPoint.x, worldPoint.y, 0) + offset;

        newPos = ClampToBuildableArea(newPos);

        // �׻� Z=0 ����
        newPos.z = 0;

        transform.position = newPos;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        TDTileData nearestTile = GetNearestBuildableTile(transform.position);

        if (nearestTile != null && !nearestTile.isOccupied)
        {
            transform.position = nearestTile.transform.position;
            currentTile = nearestTile;
            currentTile.isOccupied = true;
        }
        else
        {
            // �߸��� ���� �θ� ���� ��ġ�� ����
            transform.position = startPos;

            if (currentTile != null)
                currentTile.isOccupied = true;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // �巡�� �������� SortingOrder ���󺹱�
        if (sr != null)
            sr.sortingOrder = 0;
    }

    void OnMouseDown()
    {
        Debug.Log($"{name} Ŭ����");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log($"Ŭ����, UI ��? {EventSystem.current.IsPointerOverGameObject()}");
        }
    }

    private Vector3 ClampToBuildableArea(Vector3 pos)
    {
        float tileW = mapGen.tilePrefab.GetComponent<SpriteRenderer>().bounds.size.x;
        float tileH = mapGen.tilePrefab.GetComponent<SpriteRenderer>().bounds.size.y;
        Vector3 offset = mapGen.mapOffset;

        float minX = offset.x + tileW * 1;
        float maxX = offset.x + tileW * (mapGen.cols - 2);
        float minY = offset.y - tileH * (mapGen.rows - 2);
        float maxY = offset.y - tileH * 1;

        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        return pos;
    }

    private TDTileData GetNearestBuildableTile(Vector3 pos)
    {
        float tileW = mapGen.tilePrefab.GetComponent<SpriteRenderer>().bounds.size.x;
        float tileH = mapGen.tilePrefab.GetComponent<SpriteRenderer>().bounds.size.y;

        Vector3 localPos = pos - mapGen.mapOffset;
        int col = Mathf.RoundToInt(localPos.x / tileW);
        int row = -Mathf.RoundToInt(localPos.y / tileH);

        col = Mathf.Clamp(col, 1, mapGen.cols - 2);
        row = Mathf.Clamp(row, 1, mapGen.rows - 2);

        return mapGen.tiles[row, col];
    }
}
