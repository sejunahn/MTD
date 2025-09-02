using UnityEngine;

public class GridManager : MonoBehaviour
{
    // public int width = 7;
    // public int height = 9;
    // public GameObject groundPrefab;
    // public GameObject pathPrefab;
    //
    // private TileController[,] grid;
    //
    // void Start()
    // {
    //     GenerateGrid();
    // }
    //
    // void GenerateGrid()
    // {
    //     grid = new TileController[width, height];
    //
    //     float minX = -6f;
    //     float maxX = 6f;
    //     float minY = -8f;
    //     float maxY = 8f;
    //
    //     float cellWidth = (maxX - minX) / (width - 1);
    //     float cellHeight = (maxY - minY) / (height - 1);
    //
    //     for (int y = 0; y < height; y++)
    //     {
    //         for (int x = 0; x < width; x++)
    //         {
    //             // 외곽은 Path, 내부는 Ground
    //             GameObject prefabToUse = 
    //                 (x == 0 || y == 0 || x == width - 1 || y == height - 1) 
    //                     ? pathPrefab 
    //                     : groundPrefab;
    //
    //             float worldX = minX + (x * cellWidth);
    //             float worldY = maxY - (y * cellHeight);
    //
    //             Vector3 pos = new Vector3(worldX, worldY, 0);
    //
    //             GameObject tile = Instantiate(prefabToUse, pos, Quaternion.identity, transform);
    //             TileController tc = tile.GetComponent<TileController>();
    //             tc.gridPos = new Vector2Int(x, y);
    //             tc.tileType = prefabToUse == pathPrefab ? TileType.Path : TileType.Ground;
    //
    //             grid[x, y] = tc;
    //         }
    //     }
    // }
}