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
        // TODO : »∏ªÁ æ¿¿∏∑Œ µπæ∆∞©¥œ¥Ÿ
        Debug.Log("»∏ªÁ æ¿¿∏∑Œ µπæ∆∞©¥œ¥Ÿ");
    }
}
