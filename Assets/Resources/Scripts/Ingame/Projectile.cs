using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Enemy target;
    private int damage;
    private GameSceneManager gm;
    [SerializeField] private float speed = 10f;

    public void Fire(Enemy target, int damage, GameSceneManager gm)
    {
        this.target = target;
        this.damage = damage;
        this.gm = gm;
    }

    void Update()
    {
        if (target == null)
        {
            gm.ReturnProjectile(this);
            return;
        }

        Vector3 dir = target.transform.position - transform.position;
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, target.transform.position) < 0.1f)
        {
            target.TakeDamage(damage);
            gm.ReturnProjectile(this);
        }
    }
}