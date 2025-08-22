using UnityEngine;

public class SimpleDragHandler : MonoBehaviour
{
    private Camera mainCam;
    private bool isDragging = false;
    private float zOffset;

    void Start()
    {
        mainCam = Camera.main;
    }

    void OnMouseDown()
    {
        isDragging = true;

        // 오브젝트와 카메라 간 Z 거리 기억
        zOffset = mainCam.WorldToScreenPoint(transform.position).z;
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

    void OnMouseUp()
    {
        isDragging = false;
        // 손 떼면 그냥 현재 위치에 멈춤 (아무 처리 없음)
    }
}