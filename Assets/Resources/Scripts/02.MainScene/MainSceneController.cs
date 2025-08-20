using UnityEngine;
using UnityEngine.UI;

public class MainSceneController : MonoBehaviour
{
    public void OnStartButtonClicked()
    {
        // InGame 씬으로 전환
        PageManager.LoadScene(PageManager.Scenes.InGame);
    }
}
