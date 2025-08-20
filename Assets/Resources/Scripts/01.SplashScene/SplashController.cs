using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SplashController : MonoBehaviour
{
    [Header("Fade 설정")]
    public Image fadeImage;          // 전체 화면을 덮는 흰색/검은색 Image
    public float fadeDuration = 1f;  // 페이드 인/아웃 시간
    public float stayDuration = 2f;  // 페이드 후 유지 시간

    private void Start()
    {
        // 시작 시 코루틴 실행
        StartCoroutine(SplashFlow());
    }

    private IEnumerator SplashFlow()
    {
        // 1. 페이드 인 (검은 화면 → 투명)
        yield return StartCoroutine(Fade(1f, 0f));

        // 2. 유지
        yield return new WaitForSeconds(stayDuration);

        // 3. 페이드 아웃 (투명 → 검은 화면)
        yield return StartCoroutine(Fade(0f, 1f));

        // 4. Main 씬으로 전환
        PageManager.LoadScene(PageManager.Scenes.Main);
    }

    private IEnumerator Fade(float fromAlpha, float toAlpha)
    {
        if (fadeImage == null)
        {
            Debug.LogError("SplashController: fadeImage가 할당되지 않았습니다.");
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

        // 최종 알파값 확실하게 적용
        fadeImage.color = new Color(color.r, color.g, color.b, toAlpha);
    }
}
