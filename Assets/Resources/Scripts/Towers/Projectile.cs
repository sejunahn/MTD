using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Enemy target;
    private int damage;
    private float speed = 10f;

    public void Init(Enemy target, int damage)
    {
        this.target = target;
        this.damage = damage;
        gameObject.SetActive(true);
    }

    private void Update()
    {
        if (target == null)
        {
            // 타겟이 죽었거나 사라졌으면 반환
            GameSceneManager.Instance.ReturnProjectile(this);
            return;
        }

        // 목표 방향으로 이동
        Vector3 dir = (target.transform.position - transform.position).normalized;
        transform.position += dir * speed * Time.deltaTime;

        // 적에게 충분히 가까워지면 타격 처리
        if (Vector3.Distance(transform.position, target.transform.position) < 0.2f)
        {
            target.TakeDamage(damage);
            GameSceneManager.Instance.ReturnProjectile(this);
        }
    }
}