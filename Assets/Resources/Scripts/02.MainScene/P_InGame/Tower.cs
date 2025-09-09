
using UnityEngine;

public enum UpgradeType 
{
    Damage,Range,Cooltime
}

public class Tower : MonoBehaviour
{
    private TDTowerManager manager;
    private float damage;
    private float range;
    private float attackCooldown;
    private float timer;

    private float std_damage;
    private float std_range;
    private float std_cooldown;
    
    public GameObject projectilePrefab;

    [SerializeField] private SpriteRenderer towerRenderer;
    
    public void Init(TDTowerManager manager,TowerData data)
    {
        this.manager = manager;
        //                              기본데미지 + (기본데미지 * 기본값) * 0.1f * 업그레이드 여태 한값
        
        damage =  manager.atk_ref != 1 ? data.damage + (data.damage)* 0.5f * manager.atk_ref : data.damage;
        std_damage = data.damage;
        
        range =  manager.range_ref != 1 ? data.range + (data.range)* 0.1f * manager.range_ref : data.range;
        std_range= data.range;
        
        attackCooldown =  manager.atk_ref != 1 ? data.attack_cooltime - (data.attack_cooltime* 0.05f * manager.spd_ref) : data.attack_cooltime;
        std_cooldown = data.attack_cooltime;
        towerRenderer.sprite = data.tower_sprite;
    }

    public void UpgradeTower(UpgradeType type)
    {
        if(type == UpgradeType.Damage)
            damage += (std_damage) * 0.5f;
        if(type == UpgradeType.Range)
            range +=  (std_range ) * 0.1f;
        if(type == UpgradeType.Cooltime)
            attackCooldown -= 0.05f;
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
