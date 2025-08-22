using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    [SerializeField] EnemySpawner spawner;
    [SerializeField] private int maxEnemies = 150;

    private int currentLevel = 1;
    private float levelDuration = 60f; // 1분마다 레벨 증가
    private float spawnDuration = 20f; // 레벨 시작 시 20초간 스폰
    private int spawnPerSecond = 1;    // 초당 1마리
    public int ActiveEnemyCount = 0;
    private void Start()
    {
        //여기서 몬가 스타트 애니메이션
        
        //대기 했다가
        
        //여기서 레벨 시작, 이후 계속진행
        StartCoroutine(LevelRoutine());
    }

    private IEnumerator LevelRoutine()
    {
        while (true)
        {
            Debug.Log($"레벨 {currentLevel} 시작!");

            // 20초간 적 생성 루틴
            yield return StartCoroutine(SpawnEnemies());

            // 1분 기다렸다가 다음 레벨로
            yield return new WaitForSeconds(levelDuration);

            currentLevel++;
        }
    }

    private IEnumerator SpawnEnemies()
    {
        float elapsed = 0f;

        while (elapsed < spawnDuration)
        {
            if (ActiveEnemyCount >= maxEnemies)
            {
                GameOver();
                yield break;
            }
            
            spawner.SpawnEnemy("M"+1);
            yield return new WaitForSeconds(1f / spawnPerSecond);
            elapsed += 1f;
        }
    }
    
    private void GameOver()
    {
        Debug.Log("게임 패배!");
        // 씬 재시작 예시
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}