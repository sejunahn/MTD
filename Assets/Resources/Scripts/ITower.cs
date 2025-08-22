using UnityEngine;

public interface ITower
{
    int Id { get; }            // 타워 ID
    int TowerID { get; }
    string TowerName { get; }  // 이름
    int AttackPower { get; }   // 공격력
    float AttackRange { get; } // 공격 범위
    float AttackSpeed { get; } // 공격 속도
    int SellPrice { get; }     // 판매 가격

    Sprite TowerImage { get; }
    void Upgrade();
    void SettingTower();
}
