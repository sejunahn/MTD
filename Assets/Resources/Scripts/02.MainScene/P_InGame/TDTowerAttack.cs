using UnityEngine;

public class TDTowerAttack : MonoBehaviour
{
    public float range = 3f;
    public float attackCooldown = 1f;
    private float timer;
    //public TDProjectilePool projectilePool; // 총알 풀
    private int damage = 10;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= attackCooldown)
        {
            TDMonster target = GetNearestMonster();
            if (target != null)
            {
                Shoot(target.transform);
                timer = 0;
            }
        }
    }

    TDMonster GetNearestMonster()
    {
        TDMonster nearest = null;
        float minDist = Mathf.Infinity;

        foreach (TDMonster m in FindObjectsOfType<TDMonster>())
        {
            float dist = Vector3.Distance(transform.position, m.transform.position);
            if (dist <= range && dist < minDist)
            {
                nearest = m;
                minDist = dist;
            }
        }
        return nearest;
    }

    void Shoot(Transform target)
    {
        TDProjectile proj = TDProjectilePool.Instance.GetProjectile();
        proj.gameObject.SetActive(true);
        proj.transform.position = transform.position;
        proj.Init(target, damage);
    }


}
