using UnityEngine;

public class TDMapGenerator : MonoBehaviour
{
    public GameObject tilePrefab;
    public int rows = 7;
    public int cols = 9;
    public Transform tileParent;

    public TDTileData[,] tiles;
    public Vector3 mapOffset;

    void Start()
    {
        SpriteRenderer sr = tilePrefab.GetComponent<SpriteRenderer>();
        float tileWidth = sr.bounds.size.x;
        float tileHeight = sr.bounds.size.y;

        float mapWidth = cols * tileWidth;
        float mapHeight = rows * tileHeight;
        AdjustCamera(mapWidth, mapHeight);

        tiles = new TDTileData[rows, cols];

        mapOffset = new Vector3(-mapWidth / 2 + tileWidth / 2, mapHeight / 2 - tileHeight / 2, 0);

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                Vector3 pos = new Vector3(c * tileWidth, -r * tileHeight, 0) + mapOffset;
                GameObject tileGO = Instantiate(tilePrefab, pos, Quaternion.identity, tileParent);

                TDTileData tileData = tileGO.AddComponent<TDTileData>();
                tileData.gridPosition = new Vector2Int(r, c);

                if (r == 0 || r == rows - 1 || c == 0 || c == cols - 1)
                    tileGO.GetComponent<SpriteRenderer>().color = Color.gray;
                else
                    tileGO.GetComponent<SpriteRenderer>().color = Color.green;

                tiles[r, c] = tileData;
            }
        }
    }

    void AdjustCamera(float mapWidth, float mapHeight)
    {
        Camera cam = Camera.main;
        float screenRatio = (float)Screen.width / Screen.height;
        float targetRatio = mapWidth / mapHeight;

        if (screenRatio >= targetRatio)
            cam.orthographicSize = mapHeight / 2f;
        else
            cam.orthographicSize = (mapWidth / screenRatio) / 2f;
    }

    // TDMapGenerator.cs에 추가
    public TDTileData[] GetMonsterPath()
    {
        int total = (rows + cols - 2) * 2;
        TDTileData[] path = new TDTileData[total];
        int index = 0;

        // Top row (왼쪽→오른쪽)
        for (int c = 0; c < cols; c++)
            path[index++] = tiles[0, c];

        // Right column (위→아래)
        for (int r = 1; r < rows; r++)
            path[index++] = tiles[r, cols - 1];

        // Bottom row (오른쪽→왼쪽)
        for (int c = cols - 2; c >= 0; c--)
            path[index++] = tiles[rows - 1, c];

        // Left column (아래→위)
        for (int r = rows - 2; r > 0; r--)
            path[index++] = tiles[r, 0];

        return path;
    }

}
