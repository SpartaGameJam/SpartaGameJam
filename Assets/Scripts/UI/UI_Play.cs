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
        Img_Phone,
        Img_Document,
        Img_Pattern01,

        Img_Enemy
    }

    enum Objects
    {
        Obj_DocumentPanel,
        Obj_WorkInstructionPanel
    }
    #endregion

    Button Btn_Phone;

    Image Img_BG01;
    Image Img_Desk;
    Image Img_Phone;
    Image Img_Document;
    Image Img_Pattern01;

    Image Img_Enemy;
    GameObject Obj_DocumentPanel;
    GameObject Obj_WorkInstructionPanel;


    
    protected override void Awake()
    {
        BindButtons(typeof(Buttons));
        BindImages(typeof(Images));
        BindObjects(typeof(Objects));

        Btn_Phone = GetButton((int)Buttons.Btn_Phone);

        Img_BG01 = GetImage((int)Images.Img_BG01);
        Img_Desk = GetImage((int)Images.Img_Desk);
        Img_Phone = GetImage((int)Images.Img_Phone);
        Img_Document = GetImage((int)Images.Img_Document);
        Img_Pattern01 = GetImage((int)Images.Img_Pattern01);

        Img_Enemy = GetImage((int)Images.Img_Enemy);

        Obj_DocumentPanel = GetObject((int)Objects.Obj_DocumentPanel);
        Obj_WorkInstructionPanel = GetObject((int)Objects.Obj_WorkInstructionPanel);

        Img_BG01.sprite = Resources.Load<Sprite>("Play/Img_BG01");
        Img_Desk.sprite = Resources.Load<Sprite>("Play/Img_Desk");
        Img_Phone.sprite = Resources.Load<Sprite>("Play/Img_Phone");
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