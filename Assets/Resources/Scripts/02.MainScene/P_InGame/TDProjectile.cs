using UnityEngine;

public class TDProjectile : MonoBehaviour
{
    public float speed = 5f;
    public int damage = 10;
    private Transform target;

    public void Init(Transform target, int damage)
    {
        this.target = target;
        this.damage = damage;
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = (target.position - transform.position).normalized;
        transform.position += dir * speed * Time.deltaTime;

        // 거리 체크
        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            TDMonster monster = target.GetComponent<TDMonster>();
            if (monster != null)
            {
                monster.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}
