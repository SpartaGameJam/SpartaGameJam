using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Play : UI_Scene
{
    #region enum
    enum Buttons
    {
        Btn_Phone
    }

    enum Images
    {
        Img_BG01,
        Img_Wall,
        Img_Desk,
        Img_DeskProps
    }
    #endregion

    Button Btn_Phone;


    protected override void Awake()
    {
        BindButtons(typeof(Buttons));

        Btn_Phone = GetButton((int)Buttons.Btn_Phone);

        BindEvent(Btn_Phone.gameObject, OnShowPhone);
    }


    #region Button
    public void OnShowPhone(PointerEventData eventData)
    {
        UIManager.Instance.ShowPopup<UI_Phone>();
    }
    #endregion
}