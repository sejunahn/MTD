using UnityEngine;
public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class EnemyData
    {
        public string enemyId;
        public GameObject prefab;
    }

    public EnemyData[] enemyPrefabs;
    public Transform spawnPoint;

    public void SpawnEnemy(string enemyId)
    {
        foreach (var enemy in enemyPrefabs)
        {
            if (enemy.enemyId == enemyId)
            {
                Instantiate(enemy.prefab, spawnPoint.position, Quaternion.identity);
                return;
            }
        }
    }

    public void SpawnSlime()
    {
        SpawnEnemy("M001");
    }

}