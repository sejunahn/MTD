using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // [Header("Database")]
    // public EnemyData[] enemyDatabase;
    //
    // [Header("Waypoints (기본 경로)")]
    // // public Transform[] waypoints; // 기본 4개 웨이포인트
    // [SerializeField] private int defaultLaps = 3; // 몇 바퀴 돌릴지 (기본값)
    //
    // public Enemy SpawnEnemy(string enemyId, int level, GameSceneManager gm, int laps = -1)
    // {
    //     foreach (var data in enemyDatabase)
    //     {
    //         if (data.enemyId == enemyId)
    //         {
    //             GameObject go = Instantiate(data.prefab);
    //             Enemy e = go.GetComponent<Enemy>();
    //
    //             if (e != null)
    //             {
    //                 int lapCount = (laps == -1) ? defaultLaps : laps;
    //                 e.Initialize(data, level, gm);
    //                 return e;
    //             }
    //             else
    //             {
    //                 Debug.LogError($"[EnemySpawner] {data.prefab.name} 프리팹에 Enemy 컴포넌트가 없습니다!");
    //             }
    //         }
    //     }
    //
    //     Debug.LogError($"[EnemySpawner] enemyId {enemyId} 를 찾을 수 없습니다!");
    //     return null;
    // }
}