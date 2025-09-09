using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SplashController : MonoBehaviour
{
    [Header("Fade ����")]
    public Image fadeImage;          // ��ü ȭ���� ���� ���/������ Image
    public float fadeDuration = 1f;  // ���̵� ��/�ƿ� �ð�
    public float stayDuration = 2f;  // ���̵� �� ���� �ð�

    private void Start()
    {
        // ���� �� �ڷ�ƾ ����
        StartCoroutine(SplashFlow());
    }

    private IEnumerator SplashFlow()
    {
        // 1. ���̵� �� (���� ȭ�� �� ����)
        yield return StartCoroutine(Fade(1f, 0f));

        // 2. ����
        yield return new WaitForSeconds(stayDuration);

        // 3. ���̵� �ƿ� (���� �� ���� ȭ��)
        yield return StartCoroutine(Fade(0f, 1f));

        // 4. Main ������ ��ȯ
        // PageManager.LoadScene(PageManager.Scenes.Main);
        SceneTransitionManager.Instance.TransitionToScene(PageManager.Scenes.InGame);
    }

    private IEnumerator Fade(float fromAlpha, float toAlpha)
    {
        if (fadeImage == null)
        {
            Debug.LogError("SplashController: fadeImage�� �Ҵ���� �ʾҽ��ϴ�.");
            yield break;
        }

        float timer = 0f;
        Color color = fadeImage.color;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(fromAlpha, toAlpha, timer / fadeDuration);
            fadeImage.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        // ���� ���İ� Ȯ���ϰ� ����
        fadeImage.color = new Color(color.r, color.g, color.b, toAlpha);
    }
}
