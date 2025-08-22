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


    // public string Id => "T001";
    // public string Name => "기본포탑";
    // public int Attack => 10;
    // public float AttackSpeed => 1.0f;
    // public float Range => 2f;
    // public int MergeLevel => 3;
    // public int Cost => 50;
}