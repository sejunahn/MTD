using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class TDMonsterSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject monsterPrefab;   // ���� ������
    public Transform monsterParent;    // MonsterParent
    public TDMapGenerator mapGen;      // �� ���ʷ����� ����
    public float spawnInterval = 2f;   // ���� ���� �ֱ�
    public int maxMonstersPerStage = 30; // ���������� �ִ� ���� ��
    public float stageDelay = 10f;     // ���� �� ������ ���� �ð�

    [Header("UI")]
    public TMP_Text monsterCountText;

    private int spawnedCount = 0;
    private bool stageActive = true;

    [SerializeField] private List<MonsterData> monsterData;
    
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

            // ���Ͱ� ��� ���� �� 10�� ���
            stageActive = false;
            for (int i = Mathf.RoundToInt(stageDelay); i > 0; i--)
            {
                if (monsterCountText != null)
                    monsterCountText.text = $"Waiting {i}s...";
                yield return new WaitForSeconds(1f);
            }

            // ���� ���������� ���� �ʱ�ȭ
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
}
