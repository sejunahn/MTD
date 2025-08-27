using UnityEngine;

[CreateAssetMenu(menuName = "Game/EnemyData", fileName = "EnemyData")]
public class EnemyData : ScriptableObject
{
    public string enemyId;
    public string enemyName;
    public GameObject prefab;   // 프리팹 정보는 데이터 안에만 저장
    public int baseHp;
    public float baseMoveSpeed;
    public int baseReward;
    public string trait;
    
    // 레벨 기반 스케일링 함수
    public int GetHpByLevel(int level) => baseHp + (level * 5);
    public float GetSpeedByLevel(int level) => baseMoveSpeed + (level * 0.1f);
    public int GetRewardByLevel(int level) => baseReward + (level * 2);
}
