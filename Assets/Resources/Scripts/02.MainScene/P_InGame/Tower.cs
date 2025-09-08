using UnityEngine;

public class Tower : MonoBehaviour
{
    
    private int damage = 10;
    private float range = 3f;
    private float attackCooldown = 1f;
    private float timer;
    public GameObject projectilePrefab;

    [SerializeField] private SpriteRenderer towerRenderer;
    
    public void Init(TowerData data)
    {
        damage = data.damage;
        range = data.range;
        attackCooldown = data.attack_cooltime;
        towerRenderer.sprite = data.tower_sprite;
    }

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
        GameObject proj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        TDProjectile p = proj.GetComponent<TDProjectile>();
        p.Init(target, damage); // 10 ������
    }
}
