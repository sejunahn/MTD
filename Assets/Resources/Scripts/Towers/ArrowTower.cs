using UnityEngine;

public class ArrowTower : ITower
{
    public string Id => "T001";
    public string Name => "기본포탑";
    public int Attack => 10;
    public float AttackSpeed => 1.0f;
    public float Range => 2f;
    public int MergeLevel => 3;
    public int Cost => 50;
}