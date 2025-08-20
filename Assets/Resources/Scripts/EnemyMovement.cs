using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Transform[] waypoints;   // 경로 포인트들
    public float speed = 1.0f;

    private int currentIndex = 0;

    void Update()
    {
        if (waypoints.Length == 0) return;

        Transform target = waypoints[currentIndex];
        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

        // 목표 지점 도달 체크
        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            currentIndex++;
            if (currentIndex >= waypoints.Length)
            {
                // 마지막 웨이포인트 도착 → 처리
                OnPathEnd();
            }
        }
    }

    void OnPathEnd()
    {
        // 예: 체력 감소, 자기 자신 삭제
        Debug.Log($"{gameObject.name} reached the end.");
        Destroy(gameObject);
    }
}