using UnityEngine;
using UnityEngine.UI;

public class MainSceneController : MonoBehaviour
{
    public void OnStartButtonClicked()
    {
        // InGame ������ ��ȯ
        PageManager.LoadScene(PageManager.Scenes.InGame);
    }
}
