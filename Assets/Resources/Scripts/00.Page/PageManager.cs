using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;

public static class PageManager
{
    // 현재 씬 이름
    public static string CurrentSceneName { get; private set; }

    // 씬 전환 이벤트 (옵션)
    public static event Action<string> OnSceneChanged;

    // 씬 이름 상수
    public static class Scenes
    {
        public const string Splash = "00.Splash";
        public const string Main = "01.Main";
        public const string InGame = "02.InGame";
    }

    // 씬 로드
    public static void LoadScene(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("PageManager: LoadScene에 유효하지 않은 씬 이름이 들어왔습니다.");
            return;
        }

        SceneManager.LoadScene(sceneName);
        CurrentSceneName = sceneName;
        OnSceneChanged?.Invoke(sceneName);
    }

    // 씬 재로드
    public static void ReloadScene()
    {
        if (string.IsNullOrEmpty(CurrentSceneName))
        {
            Debug.LogError("PageManager: 현재 씬이 설정되지 않았습니다.");
            return;
        }

        LoadScene(CurrentSceneName);
    }

    // 다음 씬 로드 (씬 빌드 인덱스 기준)
    public static void LoadNextScene()
    {
        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextIndex < SceneManager.sceneCountInBuildSettings)
        {
            string nextSceneName = System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(nextIndex));
            LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning("PageManager: 다음 씬이 없습니다.");
        }
    }
}
