using UnityEngine;

[CreateAssetMenu(menuName = "Data/TowerData")]
public class TowerData : ScriptableObject
{
    public int id;
    public string towerName;
    public int attackPower;
    public float attackRange;
    public float attackSpeed;
    public int cost;
}