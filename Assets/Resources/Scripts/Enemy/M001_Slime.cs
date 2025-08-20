using UnityEngine;

public class M001_Slime : IEnemy
{
    public string Id => "M001";
    public string Name => "Slime";
    public int Hp => 50;
    public float MoveSpeed => 1.0f;
    public int Reward => 5;
    public string Trait => "기본";
}
