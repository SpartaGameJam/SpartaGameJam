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
        Img_Desk,
        Img_Document,
        Img_Pattern01
    }

    enum Objects
    {
        Obj_DocumentPanel
    }
    #endregion

    Button Btn_Phone;

    Image Img_BG01;
    Image Img_Desk;
    Image Img_Document;
    Image Img_Pattern01;

    
    protected override void Awake()
    {
        BindButtons(typeof(Buttons));
        BindImages(typeof(Images));
        BindObjects(typeof(Objects));

        Btn_Phone = GetButton((int)Buttons.Btn_Phone);

        Img_BG01 = GetImage((int)Images.Img_BG01);
        Img_Desk = GetImage((int)Images.Img_Desk);
        Img_Document = GetImage((int)Images.Img_Document);
        Img_Pattern01 = GetImage((int)Images.Img_Pattern01);

        Img_BG01.sprite = Resources.Load<Sprite>("Play/Img_BG01");
        Img_Desk.sprite = Resources.Load<Sprite>("Play/Img_Desk");
        Img_Document.sprite = Resources.Load<Sprite>("Document/Img_Document");
        Img_Pattern01.sprite = Resources.Load<Sprite>("Document/Img_Pattern01");

        BindEvent(Btn_Phone.gameObject, OnShowPhone);
    }


    #region Button
    public void OnShowPhone(PointerEventData eventData)
    {
        UIManager.Instance.ShowPopup<UI_Phone>();
    }
    #endregion
}