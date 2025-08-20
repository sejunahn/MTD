using UnityEngine;

public interface ITower
{
    string Id { get; }
    string Name { get; }
    int Attack { get; }
    float AttackSpeed { get; }
    float Range { get; }
    int MergeLevel { get; }
    int Cost { get; }
}
