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
        // TODO : ȸ�� ������ ���ư��ϴ�
        Debug.Log("ȸ�� ������ ���ư��ϴ�");
    }
}
