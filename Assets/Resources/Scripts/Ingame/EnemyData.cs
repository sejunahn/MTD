using UnityEngine;

[CreateAssetMenu(menuName = "Data/EnemyData")]
public class EnemyData : ScriptableObject
{
    public string id;
    public string enemyName;
    public int baseHp;
    public float moveSpeed;
    public int reward;
}