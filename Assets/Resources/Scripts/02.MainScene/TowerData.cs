using UnityEngine;

[CreateAssetMenu(fileName = "TowerData", menuName = "Scriptable Objects/TowerData")]

public class TowerData : ScriptableObject
{
    public int id;
    public string towerName;
    public int attackPower;
    public float attackRange;
    public float attackSpeed;
    public int sellPrice;
    public Sprite towerImage;
}