using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    public TextMeshPro textMesh;

    private Color textColor;
    private float moveSpeed = 1f;
    private float fadeSpeed = 2f;

    public void Setup(int damage)
    {
        textMesh.text = "-" + damage;
        textColor = Color.red;
        textColor.a = 1f;
        textMesh.color = textColor;
    }

    void Update()
    {
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;

        textColor.a -= fadeSpeed * Time.deltaTime;
        textMesh.color = textColor;

        if (textColor.a <= 0f)
            Destroy(gameObject);
    }
}
