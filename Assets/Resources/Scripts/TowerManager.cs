using UnityEngine;

public class TowerManager : MonoBehaviour
{
    [SerializeField] private GameObject towerPrefab;
    [SerializeField] private TileGrid tileGrid;

    // 버튼 클릭 시 타워 소환
    public void SpawnTower()
    {
        foreach (var tile in tileGrid.tiles)
        {
            if (tile.IsEmpty)
            {
                GameObject t = Instantiate(towerPrefab, tile.transform.position, Quaternion.identity);
                tile.towerOnTile = t;
                return; // 첫 번째 빈 타일에만 소환 후 종료
            }
        }
    }
}