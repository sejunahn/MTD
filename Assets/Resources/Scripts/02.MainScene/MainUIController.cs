using UnityEngine;

public class MainUIController : MonoBehaviour
{
    /// <summary>
    /// UI 버튼 OnClick에 연결해서 호출
    /// </summary>
    public void OnClickStartGame()
    {
        PageManager.Instance.GoToGame();
    }
}
