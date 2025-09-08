using UnityEngine;
using UnityEngine.UI;

public class TDMonster : MonoBehaviour
{
    public int maxHp = 50;
    private int currentHp;

    public Slider hpSlider;   // Inspector에 연결

    private float hideDelay = 2f; // 마지막 피격 후 몇 초 뒤 숨김
    private float hideTimer;
    private bool visible;

    void Start()
    {
        currentHp = maxHp;
        hpSlider.maxValue = maxHp;
        hpSlider.value = maxHp;
        hpSlider.gameObject.SetActive(false); // 시작 시 안 보임
    }

    public GameObject damagePopupPrefab; // Inspector에 프리팹 연결
    public Transform damagePopupCanvas;
    public void TakeDamage(int dmg)
    {
        currentHp -= dmg;
        if (currentHp <= 0)
        {
            Destroy(gameObject);
            return;
        }

        hpSlider.value = currentHp;
        ShowHpBar();

        // ✅ 데미지 팝업 생성
        ShowDamagePopup(dmg);
    }

    void ShowDamagePopup(int dmg)
    {
        GameObject popup = Instantiate(damagePopupPrefab, transform.position + Vector3.up * 1.5f, Quaternion.identity, damagePopupCanvas);
        popup.GetComponent<DamagePopup>().Setup(dmg);
    }

    void Update()
    {
        if (visible)
        {
            hideTimer -= Time.deltaTime;
            if (hideTimer <= 0)
            {
                hpSlider.gameObject.SetActive(false);
                visible = false;
            }
        }

        // 항상 카메라 쪽 바라보게 (UI가 회전하지 않도록)
        hpSlider.transform.forward = Camera.main.transform.forward;
    }

    void ShowHpBar()
    {
        hpSlider.gameObject.SetActive(true);
        visible = true;
        hideTimer = hideDelay;
    }

  
}
