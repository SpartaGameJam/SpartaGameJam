using UnityEngine;
using UnityEngine.UI;

public class Btn_ReturnToOffice : MonoBehaviour
{
    public Button Btn => btn;
    Button btn;

    private void Start()
    {
        btn = GetComponent<Button>();
        btn.interactable = true;
    }
}
