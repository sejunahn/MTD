using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData", menuName = "Scriptable Objects/MonsterData")]
public class MonsterData : ScriptableObject
{
    public int level;
    public int hp;
    public float speed;
    public float armor;
    public Sprite img_sprite;
}
