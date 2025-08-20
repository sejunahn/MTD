using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SplashController : MonoBehaviour
{
    public Image fadeImage;      // 페이드용 UI (Canvas에 전체화면 검은색 Image)
    public float fadeDuration = 1f; // 페이드 인/아웃 시간
    public float stayDuration = 2f; // 화면 유지 시간

    private void Start()
    {
        StartCoroutine(SplashFlow());
    }

    private IEnumerator SplashFlow()
    {
        // 1. 페이드 인 (검은 화면 → 투명)
        yield return StartCoroutine(Fade(1f, 0f));

        // 2. 2초간 유지
        yield return new WaitForSeconds(stayDuration);

        // 3. 페이드 아웃 (투명 → 검은 화면)
        yield return StartCoroutine(Fade(0f, 1f));

        // 4. 메인 씬으로 이동
        PageManager.Instance.GoToMain();
    }

    /// <summary>
    /// 알파 값을 from → to로 서서히 바꿔줌
    /// </summary>
    private IEnumerator Fade(float from, float to)
    {
        float t = 0f;
        Color c = fadeImage.color;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float normalized = Mathf.Clamp01(t / fadeDuration);
            c.a = Mathf.Lerp(from, to, normalized);
            fadeImage.color = c;
            yield return null;
        }
    }
}
