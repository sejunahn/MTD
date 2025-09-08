using UnityEngine;

public class TDProjectile : MonoBehaviour
{
    public float speed = 5f;
    public int damage = 10;
    private Transform target;
    private TDProjectilePool pool;

    public void SetPool(TDProjectilePool pool)
    {
        this.pool = pool;
    }

    public void Init(Transform target, int damage)
    {
        this.target = target;
        this.damage = damage;
    }

    void Update()
    {
        if (target == null)
        {
            ReturnToPool();
            return;
        }

        Vector3 dir = (target.position - transform.position).normalized;
        transform.position += dir * speed * Time.deltaTime;

        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            TDMonster monster = target.GetComponent<TDMonster>();
            if (monster != null)
                monster.TakeDamage(damage);

            ReturnToPool();
        }
    }

    void ReturnToPool()
    {
        if (pool != null)
            pool.ReturnProjectile(this);
        else
            Destroy(gameObject);
    }
}
