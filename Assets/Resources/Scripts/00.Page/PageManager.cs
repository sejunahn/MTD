using UnityEngine;
using UnityEngine.SceneManagement;

public class PageManager : MonoBehaviour
{
    public static PageManager Instance;

    private void Awake()
    {
        // 싱글톤 처리
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 넘어가도 유지
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 씬 이름으로 이동
    /// </summary>
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// Splash → Main 씬 이동
    /// </summary>
    public void GoToMain()
    {
        LoadScene("01.Main");
    }

    /// <summary>
    /// Main → Game 씬 이동
    /// </summary>
    public void GoToGame()
    {
        LoadScene("02.InGame");
    }
}
