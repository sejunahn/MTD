using UnityEngine;

[System.Serializable]
public class StatData
{
    private string levelKey;
    private string priceKey;

    public int Level { get; set; }
    public int Price { get; set; }

    public StatData(string keyPrefix, int defaultLevel = 1, int defaultPrice = 100)
    {
        levelKey = keyPrefix + "_Level";
        priceKey = keyPrefix + "_Price";
        Load(defaultLevel, defaultPrice);
    }

    public void Save()
    {
        PlayerPrefs.SetInt(levelKey, Level);
        PlayerPrefs.SetInt(priceKey, Price);
    }

    public void Load(int defaultLevel = 1, int defaultPrice = 100)
    {
        Level = PlayerPrefs.GetInt(levelKey, defaultLevel);
        Price = PlayerPrefs.GetInt(priceKey, defaultPrice);
    }
}

public static class SaveManager
{
    private const string MoneyKey = "Money";

    public static int Money { get; set; }

    // Stat 묶음 관리
    public static StatData StatA { get; private set; } = new StatData("StatA");
    public static StatData StatB { get; private set; } = new StatData("StatB");
    public static StatData StatC { get; private set; } = new StatData("StatC");
    public static StatData StatD { get; private set; } = new StatData("StatD");

    public static void SaveData()
    {
        PlayerPrefs.SetInt(MoneyKey, Money);

        StatA.Save();
        StatB.Save();
        StatC.Save();
        StatD.Save();

        PlayerPrefs.Save();
        Debug.Log("SaveManager: 데이터 저장 완료");
    }

    public static void LoadData()
    {
        Money = PlayerPrefs.GetInt(MoneyKey, 0);

        StatA.Load();
        StatB.Load();
        StatC.Load();
        StatD.Load();

        Debug.Log("SaveManager: 데이터 불러오기 완료");
    }

    public static void ClearData()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("SaveManager: 데이터 초기화 완료");
    }
}
