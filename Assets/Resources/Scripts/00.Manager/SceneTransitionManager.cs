using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance { get; private set; }

    [SerializeField] private Material transitionMaterial; // 원본 머터리얼(에셋)
    [SerializeField] private Image transitionImage;       // Canvas의 전체 화면 Image
    [SerializeField] private float duration = 1f;

    private Material runtimeMat;      // Image에 실제로 할당해서 사용할 인스턴스
    private Coroutine currentTransition;
    private bool isTransitioning = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        if (transitionMaterial != null)
        {
            runtimeMat = new Material(transitionMaterial);
            transitionImage.material = runtimeMat;

            // 시작은 원본 상태 (픽셀화 X)
            runtimeMat.SetFloat("_Progress", 0f);
        }

        // ✅ 시작할 때 그냥 이미지만 보이도록
        transitionImage.gameObject.SetActive(true);
        transitionImage.raycastTarget = false;

        Canvas c = transitionImage.canvas;
        if (c != null)
        {
            c.overrideSorting = true;
            c.sortingOrder = 10000;
        }

        // ❌ 여기서 InFocus 자동 실행하지 않는다!
        // StartCoroutine(InFocus());
    }


    // -------------------
    // 씬 전환 호출
    // -------------------
    public void TransitionToScene(string sceneName)
    {
        if (isTransitioning) return;
        StartCoroutine(SceneTransitionCoroutine(sceneName));
    }

    IEnumerator SceneTransitionCoroutine(string sceneName)
    {
        isTransitioning = true;

        // OUT: 화면 닫히기
        yield return StartCoroutine(OutFocus());

        // 씬 비동기 로드
        var op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = true;
        while (!op.isDone)
            yield return null;

        // IN: 화면 열리기
        yield return StartCoroutine(InFocus());

        isTransitioning = false;
    }

    // -------------------
    // 수동 호출용
    // -------------------
    public void StartOutFocus()
    {
        if (currentTransition != null) StopCoroutine(currentTransition);
        currentTransition = StartCoroutine(OutFocus());
    }

    public void StartInFocus()
    {
        if (currentTransition != null) StopCoroutine(currentTransition);
        currentTransition = StartCoroutine(InFocus());
    }

    // -------------------
    // 전환 애니메이션
    // -------------------
    IEnumerator OutFocus()
    {
        // Image 켜기
        transitionImage.gameObject.SetActive(true);

        // 0 → 1 (열림 → 닫힘)
        yield return StartCoroutine(AnimateTransition(0f, 1f));
    }

    IEnumerator InFocus()
    {
        // 1 → 0 (닫힘 → 열림)
        yield return StartCoroutine(AnimateTransition(1f, 0f));

        // 다 열리면 Image 끄기
        //transitionImage.gameObject.SetActive(false);
    }

    IEnumerator AnimateTransition(float from, float to)
    {
        if (runtimeMat == null)
        {
            // 안전 처리
            float t = 0f;
            while (t < duration)
            {
                t += Time.deltaTime;
                yield return null;
            }
            yield break;
        }

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsed / duration);
            float value = Mathf.Lerp(from, to, progress);

            runtimeMat.SetFloat("_Progress", value);
            yield return null;
        }

        runtimeMat.SetFloat("_Progress", to);
    }
}
