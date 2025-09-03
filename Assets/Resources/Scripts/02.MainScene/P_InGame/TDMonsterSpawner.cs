using UnityEngine;

public class TDMonsterSpawner : MonoBehaviour
{
    public GameObject monsterPrefab;   // 몬스터 프리팹
    public Transform monsterParent;    // MonsterParent (Hierarchy에 있는 애)
    public TDMapGenerator mapGen;      // 맵 제너레이터 참조

    public float spawnInterval = 2f;   // 몬스터 생성 주기
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
