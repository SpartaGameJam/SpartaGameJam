using UnityEngine;
using UnityEngine.UI;

public class StopButton : MonoBehaviour
{
    Button button;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(Stop);
    }

    void Stop()
    {
        UIManager.Instance.ChangeSceneUI<UI_Play>();
        // TODO : 회사 씬으로 돌아갑니다
        Debug.Log("회사 씬으로 돌아갑니다");
    }
}
