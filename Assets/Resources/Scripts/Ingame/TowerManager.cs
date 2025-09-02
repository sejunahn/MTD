using UnityEditor;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    // [SerializeField] private GameObject towerPrefab;
    // [SerializeField] private TileGrid tileGrid;
    // [SerializeField] private TowerData[] towerDatabase; // 여러 종류의 타워 데이터
    //
    // // 버튼 클릭 시 타워 소환
    // public void SpawnTower()
    // {
    //     var towerId = Random.Range(0, 2);
    //     
    //     TowerData data = GetTowerDataById(towerId);
    //     if (data == null)
    //     {
    //         Debug.LogError($"[TowerManager] Tower ID {towerId}를 찾을 수 없습니다!");
    //         return;
    //     }
    //     foreach (var tile in tileGrid.tiles)
    //     {
    //         if (tile.IsEmpty)
    //         {
    //             GameObject t = Instantiate(towerPrefab, tile.transform.position, Quaternion.identity);
    //             tile.towerOnTile = t;
    //
    //             Tower tower = t.GetComponent<Tower>();
    //             tower.SettingTower(data); // 데이터 세팅
    //             
    //             return; // 첫 번째 빈 타일에만 소환 후 종료
    //         }
    //     }
    // }
    //
    // private TowerData GetTowerDataById(int id)
    // {
    //     foreach (var data in towerDatabase)
    //     {
    //         if (data.id == id) return data;
    //     }
    //     return null;
    // }
}