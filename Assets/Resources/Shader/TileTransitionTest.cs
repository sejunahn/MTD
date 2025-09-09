using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TileTransitionTest : MonoBehaviour
{
    [SerializeField] private Material transitionMaterial;
    [SerializeField] private Image transitionImage;
    [SerializeField] private float duration = 1f;
    
    private Coroutine currentTransition; // 현재 실행 중인 코루틴 추적
    private bool isTransitioning = false; // 전환 중인지 체크
    
    void Start()
    {
        transitionMaterial.SetFloat("_Progress", 0f);
    }
    
    // public void StartTransition()
    // {
    //     // 이미 실행 중인 코루틴이 있으면 중단
    //     if (currentTransition != null)
    //     {
    //         StopCoroutine(currentTransition);
    //     }
    //     
    //     currentTransition = StartCoroutine(TransitionCoroutine());
    // }
    
    public void StartOutFocus()
    {
        if (currentTransition != null)
        {
            StopCoroutine(currentTransition);
        }
        
        currentTransition = StartCoroutine(OutFocus());
    }
    public void StartInFocus()
    {
        if (currentTransition != null)
        {
            StopCoroutine(currentTransition);
        }
        
        currentTransition = StartCoroutine(InFocus());
    }
    
    // IEnumerator TransitionCoroutine()
    // {
    //     isTransitioning = true;
    //     
    //     // 페이드 아웃 (0 → 1)
    //     yield return StartCoroutine(AnimateTransition(0f, 1f));
    //     
    //     // 잠시 대기 (선택사항)
    //     yield return new WaitForSeconds(0.1f);
    //     
    //     Debug.Log("씬 전환 지점!");
    //     
    //     // 페이드 인 (1 → 0)  
    //     yield return StartCoroutine(AnimateTransition(1f, 0f));
    //     
    //     // 완료
    //     isTransitioning = false;
    //     currentTransition = null;
    // }

    public IEnumerator OutFocus()
    {
        isTransitioning = true;
        yield return StartCoroutine(AnimateTransition(0f, 1f));
        isTransitioning = false;
        currentTransition = null;
    }

    public IEnumerator InFocus()
    {
        isTransitioning = true;
        yield return StartCoroutine(AnimateTransition(1f, 0f));
        isTransitioning = false;
        currentTransition = null;
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