using UnityEngine;
using TMPro;

public class TDMonster : MonoBehaviour
{
    [Header("Stats")]
    public int maxHp = 50;
    private int currentHp;

    [Header("Health Bar (SpriteRenderer)")]
    public Transform healthBarRoot;    // 빈 막대 (Empty)
    public Transform healthFill;       // 채워지는 부분 (Fill)

    private Vector3 originalFillScale;
    private float hideDelay = 2f;
    private float hideTimer;
    private bool visible;

    [Header("Damage Popup")]
    public GameObject damagePopupPrefab; // TextMeshPro 3D prefab
    public Transform popupParent;        // DamagePopup이 들어갈 부모 (없으면 null)

    void Start()
    {
        currentHp = maxHp;
        if (healthFill != null)
            originalFillScale = healthFill.localScale;

        // 시작 시 체력바 숨김
        if (healthBarRoot != null)
            healthBarRoot.gameObject.SetActive(false);
    }

    public void TakeDamage(int dmg)
    {
        currentHp -= dmg;
        if (currentHp <= 0)
        {
            Die();
            return;
        }

        UpdateHealthBar();
        ShowHealthBar();

        ShowDamagePopup(dmg);
    }

    void UpdateHealthBar()
    {
        if (healthFill == null) return;

        float ratio = (float)currentHp / maxHp;
        Vector3 scale = originalFillScale;
        scale.x *= ratio;
        healthFill.localScale = scale;

        // ✅ 왼쪽 고정, 오른쪽만 줄어들도록 위치 보정
        float offsetX = (originalFillScale.x - scale.x) * 0.5f;
        healthFill.localPosition = new Vector3(-offsetX, healthFill.localPosition.y, healthFill.localPosition.z);
    }

    void ShowHealthBar()
    {
        if (healthBarRoot == null) return;

        healthBarRoot.gameObject.SetActive(true);
        visible = true;
        hideTimer = hideDelay;
    }

    void ShowDamagePopup(int dmg)
    {
        if (damagePopupPrefab == null) return;

        Vector3 spawnPos = transform.position + Vector3.up * 1.5f;
        GameObject popup = Instantiate(damagePopupPrefab, spawnPos, Quaternion.identity, popupParent);
        popup.GetComponent<DamagePopup>().Setup(dmg);
    }

    void Update()
    {
        if (visible)
        {
            hideTimer -= Time.deltaTime;
            if (hideTimer <= 0)
            {
                healthBarRoot.gameObject.SetActive(false);
                visible = false;
            }
        }

        // 체력바 항상 카메라 쪽을 바라보게 (Billboard)
        if (healthBarRoot != null)
            healthBarRoot.forward = Camera.main.transform.forward;
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
