using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    [Header("기본 설정")]
    [SerializeField] private int startGold = 10;
    [SerializeField] private int maxEnemies = 150;

    [Header("데이터")]
    [SerializeField] private EnemyData[] enemyDatabase;
    [SerializeField] private TowerData[] towerDatabase;
 
    [Header("프리팹")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject towerPrefab;
    [SerializeField] private GameObject projectilePrefab;

    private int gold;
    private int currentLevel = 1;
    private int activeEnemyCount = 0;

    private float levelDuration = 60f;
    private float spawnDuration = 20f;

    [SerializeField] private Transform[] waypoints;
    [SerializeField] private TileController[] tiles;
    private Queue<Projectile> projectilePool = new Queue<Projectile>();
    public GameObject TowerPrefab => towerPrefab;

    void Start()
    {
        gold = startGold;
        // tiles = FindObjectsOfType<TileController>();

        StartCoroutine(GoldRoutine());
        StartCoroutine(LevelRoutine());
    }

    private IEnumerator GoldRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);
            AddGold(1);
        }
    }

    private IEnumerator LevelRoutine()
    {
        while (true)
        {
            yield return StartCoroutine(SpawnEnemies());
            yield return new WaitForSeconds(levelDuration);
            currentLevel++;
        }
    }

    private IEnumerator SpawnEnemies()
    {
        float elapsed = 0f;
        while (elapsed < spawnDuration)
        {
            if (activeEnemyCount >= maxEnemies)
            {
                GameOver();
                yield break;
            }

            SpawnEnemy("Enemy01", currentLevel, laps: 2);
            yield return new WaitForSeconds(1f);
            elapsed += 1f;
        }
    }

    public void SpawnEnemy(string enemyId, int level, int laps)
    {
        EnemyData data = System.Array.Find(enemyDatabase, x => x.id == enemyId);
        if (data == null) return;

        GameObject go = Instantiate(enemyPrefab, waypoints[0].position, Quaternion.identity);
        Enemy e = go.GetComponent<Enemy>();
        e.Initialize(data, level, this, laps);
        activeEnemyCount++;
    }

    public void EnemyKilled(int reward)
    {
        activeEnemyCount--;
        AddGold(reward);
    }


    public Projectile GetProjectile()
    {
        if (projectilePool.Count > 0)
        {
            Projectile p = projectilePool.Dequeue();
            p.gameObject.SetActive(true);
            return p;
        }
        else
        {
            GameObject go = Instantiate(projectilePrefab);
            return go.GetComponent<Projectile>();
        }
    }

    public void ReturnProjectile(Projectile p)
    {
        p.gameObject.SetActive(false);
        projectilePool.Enqueue(p);
    }

    public void AddGold(int amount) => gold += amount;
    public void SpendGold(int amount) => gold -= amount;

    public void GameOver()
    {
        Debug.Log("게임 오버!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public Transform[] GetWaypoints() => waypoints;


    #region TowerSetting

    private List<Tower> activeTowers = new List<Tower>();
    private int towerSerialCounter = 0;
    
    public void SpawnTower()
    {
        int randIndex = Random.Range(0, towerDatabase.Length - 1);
        TowerData data = towerDatabase[randIndex];

        if (gold < data.cost)
        {
            Debug.Log("골드 부족!");
            return;
        }

        foreach (var tile in tiles)
        {
            if (tile.IsEmpty)
            {
                gold -= data.cost;

                GameObject towerObj = Instantiate(towerPrefab, tile.transform.position, Quaternion.identity);
                Tower tower = towerObj.GetComponent<Tower>();

                tower.Initialize(data, GenerateTowerSerial(), this);

                tile.towerOnTile = towerObj;
                activeTowers.Add(tower);
                return;
            }
        }

        Debug.Log("빈 타일이 없습니다!");
    }

    // 고유 Serial 생성
    private int GenerateTowerSerial()
    {
        towerSerialCounter++;
        return towerSerialCounter;
    }

    // 타워 제거
    public void RemoveTower(Tower tower)
    {
        if (activeTowers.Contains(tower))
        {
            activeTowers.Remove(tower);
            Destroy(tower.gameObject);
        }
    }

    #endregion
}
