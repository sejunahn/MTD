using UnityEngine;

[CreateAssetMenu(fileName = "TowerData", menuName = "Scriptable Objects/TowerData")]
public class TowerData : ScriptableObject
{
    public int damage;
    public float range;
    public float attack_cooltime;
    public Sprite tower_sprite;
    public Sprite projectile_sprite;
}
