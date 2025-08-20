using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;

public static class PageManager
{
    // ���� �� �̸�
    public static string CurrentSceneName { get; private set; }

    // �� ��ȯ �̺�Ʈ (�ɼ�)
    public static event Action<string> OnSceneChanged;

    // �� �̸� ���
    public static class Scenes
    {
        public const string Splash = "00.Splash";
        public const string Main = "01.Main";
        public const string InGame = "02.InGame";
    }

    // �� �ε�
    public static void LoadScene(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("PageManager: LoadScene�� ��ȿ���� ���� �� �̸��� ���Խ��ϴ�.");
            return;
        }

        SceneManager.LoadScene(sceneName);
        CurrentSceneName = sceneName;
        OnSceneChanged?.Invoke(sceneName);
    }

    // �� ��ε�
    public static void ReloadScene()
    {
        if (string.IsNullOrEmpty(CurrentSceneName))
        {
            Debug.LogError("PageManager: ���� ���� �������� �ʾҽ��ϴ�.");
            return;
        }

        LoadScene(CurrentSceneName);
    }

    // ���� �� �ε� (�� ���� �ε��� ����)
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
            Debug.LogWarning("PageManager: ���� ���� �����ϴ�.");
        }
    }
}
