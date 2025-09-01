using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    [Header("Managers & Spawner")]
    [SerializeField] private EnemySpawner spawner;

    [Header("Enemy Settings")]
    [SerializeField] private int maxEnemies = 150;
    [SerializeField] private string defaultEnemyId = "M1"; // 기본 적 ID

    [Header("Game Settings")]
    [SerializeField] private float levelDuration = 60f; // (선택 대기 포함) 레벨 1분 주기
    [SerializeField] private float spawnDuration = 20f; // 레벨 시작 시 20초 동안만 스폰
    [SerializeField] private int spawnPerSecond = 1;    // 초당 적 수
    
    private Queue<Projectile> projectilePool = new Queue<Projectile>();
    [SerializeField]private GameObject projectilePrefab;

    public static GameSceneManager Instance { get; private set; }
    public int ActiveEnemyCount { get; set; } = 0;
    public int CurrentLevel { get; private set; } = 1;
    public int Gold { get; private set; } = 0;

    private bool isWaitingForChoice = false;

    private void Awake()
    {
        Instance = this;
        InitProjectilePool();
    }
    
    private void Start()
    {
        StartCoroutine(GameLoop());
    }

    private IEnumerator GameLoop()
    {
        while (true)
        {
            Debug.Log($"레벨 {CurrentLevel} 시작!");

            // 20초간 적 스폰
            yield return StartCoroutine(SpawnEnemies());

            // 플레이어 선택 대기
            isWaitingForChoice = true;
            Debug.Log("플레이어 선택 대기 중...");

            yield return new WaitUntil(() => !isWaitingForChoice);

            // 다음 레벨 준비
            CurrentLevel++;
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

            // EnemySpawner를 통해 적 생성
            var enemy = spawner.SpawnEnemy(defaultEnemyId, CurrentLevel, this);
            if (enemy != null)
            {
                ActiveEnemyCount++;
            }

            yield return new WaitForSeconds(1f / spawnPerSecond);
            elapsed += 1f;
        }
    }
    private void InitProjectilePool()
    {
        for (int i = 0; i < 30; i++)
        {
            GameObject obj = Instantiate(projectilePrefab);
            obj.SetActive(false);
            projectilePool.Enqueue(obj.GetComponent<Projectile>());
        }
    }

    public void FireProjectile(Transform spawnPos, Enemy target, int damage)
    {
        if (projectilePool.Count == 0) InitProjectilePool();

        Projectile proj = projectilePool.Dequeue();
        proj.transform.position = spawnPos.position;
        proj.Init(target, damage);
    }

    public void ReturnProjectile(Projectile proj)
    {
        proj.gameObject.SetActive(false);
        projectilePool.Enqueue(proj);
    }
    
    public void EnemyKilled(int reward)
    {
        ActiveEnemyCount--;
        Gold += reward;
        Debug.Log($"골드 획득! 현재 골드: {Gold}");
    }

    public void PlayerMadeChoice()
    {
        Debug.Log("플레이어가 선택을 완료했습니다.");
        isWaitingForChoice = false;
    }

    private void GameOver()
    {
        Debug.Log("게임 패배!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
