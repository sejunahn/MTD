using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class TDMonsterSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject monsterPrefab;
    public Transform monsterParent;
    public TDMapGenerator mapGen;
    public float spawnInterval = 2f;
    public int maxMonstersPerStage = 30;
    public float stageDelay = 10f;

    [Header("UI")]
    public TMP_Text stageText;
    public TMP_Text monsterCountText;
    public TMP_Text moneyText;
    public TMP_Text waitingText;

    private int spawnedCount = 0;
    private int stage = 1;
    private int money = 0;

    [SerializeField] private List<MonsterData> monsterData;

    void Start()
    {
        UpdateUI();
        StartCoroutine(StageLoop());
    }

    void UpdateUI()
    {
        if (stageText != null)
            stageText.text = $"Stage {stage}";

        if (monsterCountText != null)
            monsterCountText.text = $"{spawnedCount}/{maxMonstersPerStage}";

        if (moneyText != null)
            moneyText.text = $"Money: {money}";

        if (waitingText != null)
            waitingText.text = ""; // 평소엔 비워둠
    }

    IEnumerator StageLoop()
    {
        while (true)
        {
            spawnedCount = 0;
            UpdateUI();

            while (spawnedCount < maxMonstersPerStage)
            {
                SpawnMonsters(stage);
                spawnedCount++;
                UpdateUI();
                yield return new WaitForSeconds(spawnInterval);
            }

            // 스테이지 클리어 후 대기
            for (int i = Mathf.RoundToInt(stageDelay); i > 0; i--)
            {
                if (waitingText != null)
                    waitingText.text = $"Waiting {i}s...";
                yield return new WaitForSeconds(1f);
            }

            stage++;
            waitingText.text = ""; // 대기 끝나면 지움
            UpdateUI();
        }
    }

    void SpawnMonster()
    {
        if (monsterPrefab == null || monsterParent == null || mapGen == null) return;

        GameObject monster = Instantiate(monsterPrefab, monsterParent);
        monster.transform.position = mapGen.tiles[0, 0].transform.position;

        TDMonsterMovement move = monster.GetComponent<TDMonsterMovement>();
        if (move != null)
            move.InitPath(mapGen.GetMonsterPath());

        TDMonster monsterScript = monster.GetComponent<TDMonster>();
        if (monsterScript != null)
        {
            monsterScript.OnMonsterDied += OnMonsterDied;
        }
    }

    public void SpawnMonsters(int level)
    {
        if (monsterPrefab == null || monsterParent == null || mapGen == null) return;

        MonsterData data = monsterData.Find(x => level == x.level);
        GameObject monster = Instantiate(monsterPrefab, Vector3.zero, Quaternion.identity, monsterParent);
        TDMonster tDMonster = monster.GetComponent<TDMonster>();
        tDMonster.InitMonster(data);
        tDMonster.InitPath(mapGen.GetMonsterPath());
    }

    private void OnMonsterDied(TDMonster monster)
    {
        money += 50;
        UpdateUI();
    }
}
