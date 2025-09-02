using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainSceneController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text_money;

    [SerializeField] private TextMeshProUGUI text_stat_A_level;
    [SerializeField] private TextMeshProUGUI text_stat_A_price;

    [SerializeField] private TextMeshProUGUI text_stat_B_level;
    [SerializeField] private TextMeshProUGUI text_stat_B_price;

    [SerializeField] private TextMeshProUGUI text_stat_C_level;
    [SerializeField] private TextMeshProUGUI text_stat_C_price;

    [SerializeField] private TextMeshProUGUI text_stat_D_level;
    [SerializeField] private TextMeshProUGUI text_stat_D_price;

    // 게임 시작 시 불러오기
    void Start()
    {
        LoadData();

        SetUI();
    }

    private void LoadData()
    {
        SaveManager.LoadData();
    }

    private void SetUI()
    {
        text_money.text = SaveManager.Money.ToString();

        text_stat_A_level.text = SaveManager.StatA.Level.ToString();
        text_stat_A_price.text = SaveManager.StatA.Price.ToString();

        text_stat_B_level.text = SaveManager.StatB.Level.ToString();
        text_stat_B_price.text = SaveManager.StatB.Price.ToString();

        text_stat_C_level.text = SaveManager.StatC.Level.ToString();
        text_stat_C_price.text = SaveManager.StatC.Price.ToString();

        text_stat_D_level.text = SaveManager.StatD.Level.ToString();
        text_stat_D_price.text = SaveManager.StatD.Price.ToString();
    }

    public void Upgrade(string stat)
    {
        switch (stat)
        {
            case "A":
                SaveManager.Money -= SaveManager.StatA.Price;

                SaveManager.StatA.Level += 1;
                SaveManager.StatA.Price += 100;
                break;
            case "B":
                SaveManager.Money -= SaveManager.StatB.Price;

                SaveManager.StatB.Level += 1;
                SaveManager.StatB.Price += 100;
                break;
            case "C":
                SaveManager.Money -= SaveManager.StatC.Price;

                SaveManager.StatC.Level += 1;
                SaveManager.StatC.Price += 100;
                break;
            case "D":
                SaveManager.Money -= SaveManager.StatD.Price;

                SaveManager.StatD.Level += 1;
                SaveManager.StatD.Price += 100;
                break;
        }

        SetUI();
    }

    public void OnStartButtonClicked()
    {
        SaveManager.SaveData();
        // InGame 씬으로 전환
        PageManager.LoadScene(PageManager.Scenes.InGame);
    }
}
