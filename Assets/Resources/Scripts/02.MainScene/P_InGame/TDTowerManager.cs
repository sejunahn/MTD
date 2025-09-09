using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TDTowerManager : MonoBehaviour
{
    [Header("Tower Settings")]
    public GameObject towerPrefab;
    public Transform towerParent;
    [SerializeField] private List<TowerData> l_towerData;

    [Header("Refs")]
    public TDMonsterSpawner spawner;   // 💰 몬스터 스포너 참조
    private TDMapGenerator mapGen;

    [Header("UI")]
    public TMP_Text nextPriceText;     // 다음 타워 가격 표시

    private int currentTowerCount = 0;

    // 가격 테이블
    private int[] towerPrices = { 0, 100, 300, 600, 1000, 1500 };

    public int atk_ref = 1;
    public int range_ref = 1;
    public int spd_ref = 1;

    [SerializeField] List<Tower> towers = new List<Tower>();

    private int ATKUPPrice = 100;
    private int RangeUPPrice = 500;
    private int SPDUPPrice = 1000;

    [SerializeField] private TextMeshProUGUI ATKUPText;
    [SerializeField] private TextMeshProUGUI RANGEUPText;
    [SerializeField] private TextMeshProUGUI SPDUPText;

    
    void Start()
    {
        mapGen = FindObjectOfType<TDMapGenerator>();
        if (spawner == null) spawner = FindObjectOfType<TDMonsterSpawner>();
        UpdatePriceUI();
        UpdateUpgradeUI();
    }

    void Update()
    {
        UpdatePriceUI();
    }

    public void SpawnTower()
    {
        if (mapGen == null || spawner == null) return;

        int price = GetNextTowerPrice();

        // 돈 부족
        if (!spawner.TrySpend(price))
        {
            Debug.Log("돈이 부족합니다!");
            return;
        }

        // 빈 타일 찾아서 생성
        for (int r = 1; r < mapGen.rows - 1; r++)
        {
            for (int c = 1; c < mapGen.cols - 1; c++)
            {
                TDTileData tile = mapGen.tiles[r, c];
                if (!tile.isOccupied)
                {
                    currentTowerCount++;

                    GameObject tower = Instantiate(towerPrefab, tile.transform.position, Quaternion.identity, towerParent);

                    // 드래그 + 타일 초기화
                    TDTowerDrag drag = tower.GetComponent<TDTowerDrag>();
                    drag.mapGen = mapGen;
                    drag.InitTile(tile);

                    // 랜덤 타워 데이터 적용
                    Tower temp = tower.GetComponent<Tower>();
                    if (temp != null && l_towerData.Count > 0)
                    {
                        int rand = Random.Range(0, l_towerData.Count);
                        temp.Init(this,l_towerData[rand]);
                    }
                    towers.Add(temp);

                    Debug.Log($"타워 소환! 현재 {currentTowerCount}개, 다음 가격 {GetNextTowerPrice()}");
                    return;
                }
            }
        }
        Debug.Log("빈 타일이 없습니다!");
    }

    private int GetNextTowerPrice()
    {
        if (currentTowerCount < towerPrices.Length)
            return towerPrices[currentTowerCount];
        else
            return towerPrices[towerPrices.Length - 1] + (currentTowerCount - (towerPrices.Length - 1)) * 500;
    }

    private void UpdatePriceUI()
    {
        if (nextPriceText != null && spawner != null)
        {
            int price = GetNextTowerPrice();
            nextPriceText.text = $"Next Price: {price}";

            if (spawner.CurrentMoney >= price)
                nextPriceText.color = Color.green; // 가능
            else
                nextPriceText.color = Color.red;   // 부족
        }
    }

    private void UpdateUpgradeUI()
    {
        ATKUPText.text = (atk_ref * ATKUPPrice).ToString();
        RANGEUPText.text = (range_ref * RangeUPPrice).ToString();
        SPDUPText.text = (spd_ref * SPDUPPrice).ToString();
    }

    public void ATKUpgrade()
    {
        if (spawner.CurrentMoney <= atk_ref * ATKUPPrice)
            return;
        foreach (var item in towers)
        {
            item.UpgradeTower(UpgradeType.Damage);
        }

        atk_ref++;

        UpdateUpgradeUI();
    }
    public void RangeUpgrade()
    {
        if (spawner.CurrentMoney <= range_ref * RangeUPPrice)
            return;
        foreach (var item in towers)
        {
            item.UpgradeTower(UpgradeType.Range);
        }

        range_ref++;
        UpdateUpgradeUI();
    }
    public void SPDUpgrade()
    {
        if (spawner.CurrentMoney <= spd_ref * SPDUPPrice)
            return;
        foreach (var item in towers)
        {
            item.UpgradeTower(UpgradeType.Cooltime);
        }

        spd_ref++;
        UpdateUpgradeUI();
    }
}
