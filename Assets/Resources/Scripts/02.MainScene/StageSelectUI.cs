using UnityEngine;

public class StageSelectUI : MonoBehaviour
{
    public GameObject panel;

    public void Show(int level)
    {
        panel.SetActive(true);
        Debug.Log($"레벨 {level} 클리어! 다음 선택지를 고르세요");
    }

    public void ChooseStage(int option)
    {
        // 버튼에서 호출됨
        Debug.Log($"스테이지 {option} 선택!");
        panel.SetActive(false);

        // GameSceneManager에 선택 전달
        // FindObjectOfType<GameSceneManager>().OnStageChoiceMade();
    }
}