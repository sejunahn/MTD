using UnityEngine;

public class Tower : MonoBehaviour,ITower
{
    [SerializeField] private int id;
    [SerializeField] private string towerName;
    [SerializeField] private int attackPower;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackSpeed;
    [SerializeField] private int sellPrice;
    [SerializeField] private Sprite towerImage;
    [SerializeField] private int towerId;
    public int Id => id;
    public int TowerID => towerId;
    public string TowerName => towerName;
    public int AttackPower => attackPower;
    public float AttackRange => attackRange;
    public float AttackSpeed => attackSpeed;
    public int SellPrice => sellPrice;
    public Sprite TowerImage => towerImage;

    [SerializeField] private SpriteRenderer Image;
    public void SettingTower()
    {
        Image.sprite = TowerImage;
    }

    public void Upgrade()
    {
        attackPower = Mathf.RoundToInt(attackPower * 1.5f);
        sellPrice += 50;
    }
    
    private void Update()
    {
        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer <= 0f)
        {
            Enemy target = FindClosestEnemy();
            if (target != null)
            {
                target.TakeDamage(attackPower);
                cooldownTimer = AttackSpeed;
            }
        }
    }
    private float cooldownTimer;
    private Enemy FindClosestEnemy()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        Enemy closest = null;
        float minDist = Mathf.Infinity;

        foreach (var e in enemies)
        {
            float dist = Vector3.Distance(transform.position, e.transform.position);
            if (dist < attackRange && dist < minDist)
            {
                closest = e;
                minDist = dist;
            }
        }
        return closest;
    }
}