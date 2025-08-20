using UnityEngine;

public class TowerManager : MonoBehaviour
{
    public GameObject towerPrefab;
    public TileController[,] grid;  // 게임 시작 시 세팅
    public int gridSize = 10;

    public void SpawnTower()
    {
        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                if (grid[x,y].IsEmpty)
                {
                    GameObject t = Instantiate(towerPrefab, grid[x,y].transform.position, Quaternion.identity);
                    grid[x,y].towerOnTile = t;
                    return;
                }
            }
        }
    }
}
