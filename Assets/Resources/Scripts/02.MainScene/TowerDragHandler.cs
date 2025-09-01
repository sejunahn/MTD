using UnityEngine;

public class TowerDragHandler : MonoBehaviour
{
    private Camera mainCam;
    private bool isDragging;
    private float zOffset;

    private TileSlot currentSlot;

    void Start()
    {
        mainCam = Camera.main;
        currentSlot = TileGridManager.Instance.GetSlotFromTile(
            GetTileUnderPosition(transform.position)
        );
        if (currentSlot != null) currentSlot.tower = GetComponent<Tower>();
    }

    void OnMouseDown()
    {
        isDragging = true;
        zOffset = mainCam.WorldToScreenPoint(transform.position).z;
    }

    void OnMouseDrag()
    {
        if (!isDragging) return;
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = zOffset;
        transform.position = mainCam.ScreenToWorldPoint(mousePos);
    }

    void OnMouseUp()
    {
        isDragging = false;

        TileController targetTile = GetTileUnderPosition(transform.position);
        if (targetTile == null) {
            // 원위치 복귀
            transform.position = currentSlot.tile.transform.position;
            return;
        }

        TileSlot targetSlot = TileGridManager.Instance.GetSlotFromTile(targetTile);
        if (targetSlot == null) return;

        if (targetSlot.IsEmpty)
        {
            MoveToSlot(targetSlot);
        }
        else
        {
            TryMergeOrSwap(targetSlot);
        }
    }

    private void MoveToSlot(TileSlot newSlot)
    {
        if (currentSlot != null) currentSlot.tower = null;

        transform.position = newSlot.tile.transform.position;
        newSlot.tower = GetComponent<Tower>();
        currentSlot = newSlot;
    }

    private void TryMergeOrSwap(TileSlot targetSlot)
    {
        Tower other = targetSlot.tower;
        Tower thisTower = GetComponent<Tower>();

        if (other != null && other.Id == thisTower.Id)
        {
            // 합성
            Debug.Log("합성 성공!");
            Destroy(other.gameObject);
            Destroy(gameObject);

            GameObject merged = Instantiate(gameObject, targetSlot.tile.transform.position, Quaternion.identity);
            Tower mergedTower = merged.GetComponent<Tower>();
            mergedTower.Upgrade();

            targetSlot.tower = mergedTower;
        }
        else
        {
            // 자리 교환
            Debug.Log("자리 교환!");

            Vector3 tempPos = other.transform.position;
            other.transform.position = currentSlot.tile.transform.position;
            transform.position = targetSlot.tile.transform.position;

            currentSlot.tower = other;
            targetSlot.tower = GetComponent<Tower>();

            currentSlot = targetSlot;
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
}
