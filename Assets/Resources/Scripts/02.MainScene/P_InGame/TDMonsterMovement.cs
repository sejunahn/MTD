using UnityEngine;

public class MonsterMovement : MonoBehaviour
{
    private TDTileData[] path;
    private int currentIndex = 0;
    public float speed = 2f;

    public void InitPath(TDTileData[] pathData)
    {
        path = pathData;
        currentIndex = 0;

        if (path.Length > 0)
            transform.position = path[0].transform.position;
    }

    void Update()
    {
        if (path == null || path.Length == 0) return;

        Vector3 targetPos = path[currentIndex].transform.position;
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPos) < 0.01f)
        {
            currentIndex++;
            if (currentIndex >= path.Length)
                currentIndex = 0; // 반복 루프
        }
    }
}
