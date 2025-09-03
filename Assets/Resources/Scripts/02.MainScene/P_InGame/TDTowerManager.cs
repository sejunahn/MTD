using UnityEngine;

public class TDTowerManager : MonoBehaviour
{
    public GameObject towerPrefab;
    public Transform towerParent;
    private TDMapGenerator mapGen;

    void Start()
    {
        mapGen = FindObjectOfType<TDMapGenerator>();
    }

    public void SpawnTower()
    {
        for (int r = 1; r < mapGen.rows - 1; r++)
        {
            for (int c = 1; c < mapGen.cols - 1; c++)
            {
                TDTileData tile = mapGen.tiles[r, c];
                if (!tile.isOccupied)
                {
                    GameObject tower = Instantiate(towerPrefab, tile.transform.position, Quaternion.identity, towerParent);

                    TDTowerDrag drag = tower.GetComponent<TDTowerDrag>();
                    drag.mapGen = mapGen;
                    drag.InitTile(tile); // �ʱ� Ÿ�� ���� + Occupied ó��

                    return;
                }
            }
        }
        Debug.Log("�� Ÿ���� �����ϴ�!");
    }
}
