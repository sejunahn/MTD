using UnityEngine;

public class TDMonsterSpawner : MonoBehaviour
{
    public GameObject monsterPrefab;   // ���� ������
    public Transform monsterParent;    // MonsterParent (Hierarchy�� �ִ� ��)
    public TDMapGenerator mapGen;      // �� ���ʷ����� ����

    public float spawnInterval = 2f;   // ���� ���� �ֱ�
    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0;
            SpawnMonster();
        }
    }

    void SpawnMonster()
    {
        GameObject monster = Instantiate(monsterPrefab, Vector3.zero, Quaternion.identity, monsterParent);

        MonsterMovement move = monster.GetComponent<MonsterMovement>();
        move.InitPath(mapGen.GetMonsterPath());
    }

}
