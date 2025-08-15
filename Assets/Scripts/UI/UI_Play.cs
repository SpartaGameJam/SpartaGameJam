using UnityEngine;

public class UI_Play : UI_Scene
{
    void OnShowPhone()
    {
        UIManager.Instance.ShowPopup<UI_Phone>();
    }
}
