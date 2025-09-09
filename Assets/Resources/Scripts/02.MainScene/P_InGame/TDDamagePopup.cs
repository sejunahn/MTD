using UnityEngine;
using TMPro;

public class TDDamagePopup : MonoBehaviour
{
    public float moveUpSpeed = 1f;
    public float fadeSpeed = 2f;

    private TMP_Text textMesh;
    private Color textColor;

    void Awake()
    {
        textMesh = GetComponent<TMP_Text>();
        textColor = textMesh.color;
    }

    public void Setup(float damage)
    {
        textMesh.text = "-" + damage;
        textColor.a = 1f;
        textMesh.color = textColor;
    }

    void Update()
    {
        // ���� �̵�
        transform.position += new Vector3(0, moveUpSpeed * Time.deltaTime, 0);

        // ���� ��������
        textColor.a -= fadeSpeed * Time.deltaTime;
        textMesh.color = textColor;

        if (textColor.a <= 0)
            Destroy(gameObject);
    }
}
