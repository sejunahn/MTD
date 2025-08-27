using UnityEngine;

public class Enemy : MonoBehaviour, IEnemy
{
    // IEnemy 인터페이스 구현 ------------------
    public string Id { get; private set; }
    public string Name { get; private set; }
    public int Hp { get; private set; }
    public float MoveSpeed { get; private set; }
    public int Reward { get; private set; }
    public string Trait { get; private set; }
    //-----------------------------------------

    private GameSceneManager gm;
    private Transform[] waypoints;
    private int currentIndex = 0;
    private int lapsRemaining = 1;

    // 초기화 메서드 (Spawner에서 호출)
    public void Initialize(EnemyData data, int level, GameSceneManager gm, Transform[] waypoints, int laps)
    {
        this.gm = gm;
        this.waypoints = waypoints;
        this.lapsRemaining = laps;

        Id = data.enemyId;
        Name = data.enemyName;
        Hp = data.baseHp + (level - 1) * 50;    // 레벨에 따른 HP 증가
        MoveSpeed = data.baseMoveSpeed;
        Reward = data.baseReward;
        Trait = data.trait;

        currentIndex = 0;
        transform.position = waypoints[0].position;
    }

    private void Update()
    {
        if (waypoints == null || waypoints.Length == 0) return;

        Transform target = waypoints[currentIndex];
        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * MoveSpeed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            currentIndex++;
            if (currentIndex >= waypoints.Length)
            {
                lapsRemaining--;
                if (lapsRemaining > 0)
                {
                    currentIndex = 0; // 다시 처음부터 순환
                }
                else
                {
                    OnPathEnd();
                }
            }
        }
    }

    public void TakeDamage(int dmg)
    {
        Hp -= dmg;
        if (Hp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        gm.EnemyKilled(Reward);
        Destroy(gameObject);
    }

    private void OnPathEnd()
    {
        // 경로 끝에 도달했을 때 처리 (예: 플레이어 HP 감소)
        Debug.Log($"{Name} reached the end of path.");
        gm.ActiveEnemyCount--; // 자연 소멸도 카운트 감소
        Destroy(gameObject);
    }
}
