using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Phone : UISelector
{
    #region enum
    enum Buttons
    {
        Btn_Back,
        Btn_Home
    }

    enum Images
    {
        Img_Phone,
        Img_SafeAreaBottom
    }
    #endregion

    Button Btn_Back;
    Button Btn_Home;
    Image Img_Phone;
    Image Img_SafeAreaBottom;

    public Transform UpgradePanelBox;
    public Transform AppMenuPanelBox;

    protected override void Awake()
    {
        UpgradePanelBox = ComponentHelper.TryFindChild(this, "UpgradePanelBox");
        AppMenuPanelBox = ComponentHelper.TryFindChild(this, "AppMenuPanelBox");

        BindButtons(typeof(Buttons));
        BindImages(typeof(Images));

        Btn_Back = GetButton((int)Buttons.Btn_Back);
        Btn_Home = GetButton((int)Buttons.Btn_Home);
        Img_Phone = GetImage((int)Images.Img_Phone);
        Img_SafeAreaBottom = GetImage((int)Images.Img_SafeAreaBottom);

        Img_Phone.sprite = Resources.Load<Sprite>("UI_Phone/Img_Phone");
        Img_SafeAreaBottom.sprite = Resources.Load<Sprite>("UI_Phone/Img_SafeAreaBottom");

        BindEvent(Btn_Back.gameObject, OnBackPannel);
        BindEvent(Btn_Home.gameObject, OnHomeButton);
    }

    #region Button
    void OnBackPannel(PointerEventData eventData)
    {
        SoundManager.instance.PlaySFX(SFXSound.ButtonClick);

        if (UIManager.Instance.PeekCurPopup() is UI_UpgradePanel)
        {
            ClosePopupUI();
            UIManager.Instance.ShowPopup<UI_AppMenuPanel>(null, AppMenuPanelBox);
        }
        else
        {
            UIManager.Instance.CloseAllPopupUI();
        }
    }

    void OnHomeButton(PointerEventData eventData)
    {
        SoundManager.instance.PlaySFX(SFXSound.ButtonClick);
        
        UIManager.Instance.CloseAllPopupUI();
    }
    #endregion
}