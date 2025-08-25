using UnityEngine;

public static class SaveManager
{
    // 저장 키 상수
    private const string MoneyKey = "Money";
    private const string StatALevelKey = "StatA_Level";
    private const string StatAPriceKey = "StatA_Price";
    private const string StatBLevelKey = "StatB_Level";
    private const string StatBPriceKey = "StatB_Price";
    private const string StatCLevelKey = "StatC_Level";
    private const string StatCPriceKey = "StatC_Price";
    private const string StatDLevelKey = "StatD_Level";
    private const string StatDPriceKey = "StatD_Price";

    // 현재 데이터 캐싱 (게임 중 참조용)
    public static int Money { get; set; }
    public static int StatA_Level { get; set; }
    public static int StatA_Price { get; set; }
    public static int StatB_Level { get; set; }
    public static int StatB_Price { get; set; }
    public static int StatC_Level { get; set; }
    public static int StatC_Price { get; set; }
    public static int StatD_Level { get; set; }
    public static int StatD_Price { get; set; }

    // 저장 함수
    public static void SaveData()
    {
        PlayerPrefs.SetInt(MoneyKey, Money);

        PlayerPrefs.SetInt(StatALevelKey, StatA_Level);
        PlayerPrefs.SetInt(StatAPriceKey, StatA_Price);

        PlayerPrefs.SetInt(StatBLevelKey, StatB_Level);
        PlayerPrefs.SetInt(StatBPriceKey, StatB_Price);

        PlayerPrefs.SetInt(StatCLevelKey, StatC_Level);
        PlayerPrefs.SetInt(StatCPriceKey, StatC_Price);

        PlayerPrefs.SetInt(StatDLevelKey, StatD_Level);
        PlayerPrefs.SetInt(StatDPriceKey, StatD_Price);

        PlayerPrefs.Save();
        Debug.Log("SaveManager: 데이터 저장 완료");
    }

    // 불러오기 함수
    public static void LoadData()
    {
        Money = PlayerPrefs.GetInt(MoneyKey, 1000000);

        StatA_Level = PlayerPrefs.GetInt(StatALevelKey, 1);
        StatA_Price = PlayerPrefs.GetInt(StatAPriceKey, 100);

        StatB_Level = PlayerPrefs.GetInt(StatBLevelKey, 1);
        StatB_Price = PlayerPrefs.GetInt(StatBPriceKey, 100);

        StatC_Level = PlayerPrefs.GetInt(StatCLevelKey, 1);
        StatC_Price = PlayerPrefs.GetInt(StatCPriceKey, 100);

        StatD_Level = PlayerPrefs.GetInt(StatDLevelKey, 1);
        StatD_Price = PlayerPrefs.GetInt(StatDPriceKey, 100);

        Debug.Log("SaveManager: 데이터 불러오기 완료");
    }

    // 저장된 데이터 초기화 (테스트용)
    public static void ClearData()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("SaveManager: 데이터 초기화 완료");
    }
}
