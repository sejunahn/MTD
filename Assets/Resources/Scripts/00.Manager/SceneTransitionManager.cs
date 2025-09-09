using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance { get; private set; }
    
    [SerializeField] private Material transitionMaterial;
    [SerializeField] private Image transitionImage;
    [SerializeField] private float duration = 1f;
    
    private Coroutine currentTransition;
    private bool isTransitioning = false;
    
    void Awake()
    {
        // Singleton 패턴
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
        transitionMaterial.SetFloat("_Progress", 0f);
    }
    
    // 씬 전환 (가장 많이 사용할 메서드)
    public void TransitionToScene(string sceneName)
    {
        if (isTransitioning) return;
        
        StartCoroutine(SceneTransitionCoroutine(sceneName));
    }
    
    IEnumerator SceneTransitionCoroutine(string sceneName)
    {
        isTransitioning = true;
        
        // 1. OUT (화면 가리기)
        yield return StartCoroutine(OutFocus());
        
        // 2. 씬 로드
        SceneManager.LoadScene(sceneName);
        
        // 3. IN (화면 보이기) 
        yield return StartCoroutine(InFocus());
        
        isTransitioning = false;
    }
    
    // 개별 호출용 (특수한 경우)
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
    
    IEnumerator OutFocus()
    {
        yield return StartCoroutine(AnimateTransition(0f, 1f));
    }
    
    IEnumerator InFocus()
    {
        yield return StartCoroutine(AnimateTransition(1f, 0f));
    }
    
    IEnumerator AnimateTransition(float from, float to)
    {
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / duration;
            float value = Mathf.Lerp(from, to, progress);
            
            transitionMaterial.SetFloat("_Progress", value);
            yield return null;
        }
        
        transitionMaterial.SetFloat("_Progress", to);
    }
}