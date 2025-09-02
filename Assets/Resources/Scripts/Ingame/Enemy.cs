using UnityEngine;

public class Enemy : MonoBehaviour
{
    private EnemyData data;
    private GameSceneManager gm;
    private int hp;
    private int lapsRemaining;
    private Transform[] waypoints;
    private int currentIndex;

    public void Initialize(EnemyData data, int level, GameSceneManager gm, int laps)
    {
        this.data = data;
        this.gm = gm;
        hp = data.baseHp + (level * 10);
        lapsRemaining = laps;
        waypoints = gm.GetWaypoints();
        currentIndex = 1;
        transform.position = waypoints[0].position;
    }

    void Update()
    {
        if (waypoints == null || waypoints.Length == 0) return;

        Transform target = waypoints[currentIndex];
        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * data.moveSpeed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            currentIndex++;
            if (currentIndex >= waypoints.Length)
            {
                lapsRemaining--;
                if (lapsRemaining > 0) currentIndex = 0;
                else OnPathEnd();
            }
        }
    }

    public void TakeDamage(int dmg)
    {
        hp -= dmg;
        if (hp <= 0) Die();
    }

    private void Die()
    {
        gm.EnemyKilled(data.reward);
        Destroy(gameObject);
    }

    private void OnPathEnd()
    {
        gm.EnemyKilled(0);
        Destroy(gameObject);
    }
}