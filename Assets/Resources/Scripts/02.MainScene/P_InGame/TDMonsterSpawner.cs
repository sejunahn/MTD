using UnityEngine;
using TMPro;
using System.Collections;

public class TDMonsterSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject monsterPrefab;   // 몬스터 프리팹
    public Transform monsterParent;    // MonsterParent
    public TDMapGenerator mapGen;      // 맵 제너레이터 참조
    public float spawnInterval = 2f;   // 몬스터 생성 주기
    public int maxMonstersPerStage = 30; // 스테이지당 최대 몬스터 수
    public float stageDelay = 10f;     // 몬스터 다 나오고 쉬는 시간

    [Header("UI")]
    public TMP_Text monsterCountText;

    private int spawnedCount = 0;
    private bool stageActive = true;

    void Start()
    {
        UpdateUI();
        StartCoroutine(StageLoop());
    }

    void UpdateUI()
    {
        if (monsterCountText != null)
        {
            monsterCountText.text = $"{spawnedCount}/{maxMonstersPerStage}";
        }
    }

    IEnumerator StageLoop()
    {
        while (true)
        {
            spawnedCount = 0;
            stageActive = true;

            while (spawnedCount < maxMonstersPerStage)
            {
                SpawnMonster();
                spawnedCount++;
                UpdateUI();
                yield return new WaitForSeconds(spawnInterval);
            }

            // 몬스터가 모두 나온 후 10초 대기
            stageActive = false;
            for (int i = Mathf.RoundToInt(stageDelay); i > 0; i--)
            {
                if (monsterCountText != null)
                    monsterCountText.text = $"Waiting {i}s...";
                yield return new WaitForSeconds(1f);
            }

            // 다음 스테이지를 위해 초기화
            UpdateUI();
        }
    }

    void SpawnMonster()
    {
        if (monsterPrefab == null || monsterParent == null || mapGen == null) return;

        GameObject monster = Instantiate(monsterPrefab, monsterParent);

        // 시작 위치 = 좌상단
        monster.transform.position = mapGen.tiles[0, 0].transform.position;

        TDMonsterMovement move = monster.GetComponent<TDMonsterMovement>();
        if (move != null)
            move.InitPath(mapGen.GetMonsterPath());
    }
}
