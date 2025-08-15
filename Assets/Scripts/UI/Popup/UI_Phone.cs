using UnityEngine;
using UnityEngine.UI;

public class UI_Phone : UISelector
{
    #region enum
    enum Buttons
    {
        Btn_Icon00_Pressed,
        Btn_Icon00_Normal,
        Btn_Icon02_Pressed,
        Btn_Icon02_Normal,
    }

    enum Images
    {
        Img_Phone
    }
    #endregion

    Image Img_Phone;

    Button Btn_Icon00_Pressed;
    Button Btn_Icon00_Normal;
    Button Btn_Icon02_Pressed;
    Button Btn_Icon02_Normal;


    protected override void Awake()
    {
        BindImages(typeof(Images));
        BindButtons(typeof(Buttons));

        Img_Phone = GetImage((int)Images.Img_Phone);

        Btn_Icon00_Pressed = GetButton((int)Buttons.Btn_Icon00_Pressed);
        Btn_Icon00_Normal = GetButton((int)Buttons.Btn_Icon00_Normal);
        Btn_Icon02_Pressed = GetButton((int)Buttons.Btn_Icon02_Pressed);
        Btn_Icon02_Normal = GetButton((int)Buttons.Btn_Icon02_Normal);

        Img_Phone.sprite = Resources.Load<Sprite>("UI_Phone/Img_Phone");
    }
}