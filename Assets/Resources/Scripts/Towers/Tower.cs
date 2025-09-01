using System.Collections.Generic;
using UnityEngine;


public class Tower : MonoBehaviour, ITower
{
    [Header("타워 데이터")]
    [SerializeField] private int id;
    [SerializeField] private string towerName;
    [SerializeField] private int attackPower;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackSpeed;   // 초당 공격 횟수
    [SerializeField] private int sellPrice;
    [SerializeField] private Sprite towerImage;

    public int Id => id;
    public string TowerName => towerName;
    public int AttackPower => attackPower;
    public float AttackRange => attackRange;
    public float AttackSpeed => attackSpeed;
    public int SellPrice => sellPrice;
    public Sprite TowerImage => towerImage;

    [Header("컴포넌트")]
    [SerializeField] private SpriteRenderer image;
    [SerializeField] private Transform firePoint;             // 발사 위치

    [SerializeField] private List<Enemy> enemiesInRange = new List<Enemy>();
    private Enemy currentTarget;
    private float attackCooldown = 0f;

    // TowerData로 값 세팅
    public void SettingTower(TowerData data)
    {
        id = data.id;
        towerName = data.towerName;
        attackPower = data.attackPower;
        attackRange = data.attackRange;
        attackSpeed = data.attackSpeed;
        sellPrice = data.sellPrice;
        towerImage = data.towerImage;

        if (image != null && towerImage != null)
            image.sprite = towerImage;

        // 감지 범위 업데이트
        var col = firePoint.GetComponent<CircleCollider2D>();
        col.isTrigger = true;
        col.radius = attackRange;
    }

    private void Update()
    {
        attackCooldown -= Time.deltaTime;

        if (currentTarget == null || !IsInRange(currentTarget))
        {
            currentTarget = FindTarget();
        }

        if (currentTarget != null && attackCooldown <= 0f)
        {
            Attack(currentTarget);
            attackCooldown = 1f / attackSpeed; // 공격속도 반영
        }
    }

    private Enemy FindTarget()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        float minDist = float.MaxValue;
        Enemy nearest = null;

        foreach (Enemy e in enemies)
        {
            float dist = Vector3.Distance(transform.position, e.transform.position);
            if (dist < minDist && dist <= attackRange)
            {
                minDist = dist;
                nearest = e;
            }
        }
        return nearest;
    }

    private bool IsInRange(Enemy enemy)
    {
        return Vector3.Distance(transform.position, enemy.transform.position) <= attackRange;
    }

    private void Attack(Enemy target)
    {
        // GameSceneManager의 풀에서 발사체 꺼내서 날림
        GameSceneManager.Instance.FireProjectile(transform, target, attackPower);
    }

    public void Upgrade()
    {
        attackPower = Mathf.RoundToInt(attackPower * 1.5f);
        sellPrice += 50;
    }
}
