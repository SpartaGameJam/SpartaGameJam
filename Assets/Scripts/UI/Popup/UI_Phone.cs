using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Phone : UISelector
{
    #region enum
    enum Images
    {
        Img_Phone
    }
    #endregion

    Image Img_Phone;

    public Transform UpgradePanelBox;
    public Transform AppMenuPanelBox;

    protected override void Awake()
    {
        UpgradePanelBox = ComponentHelper.TryFindChild(this, "UpgradePanelBox");
        AppMenuPanelBox = ComponentHelper.TryFindChild(this, "AppMenuPanelBox");

        BindImages(typeof(Images));

        Img_Phone = GetImage((int)Images.Img_Phone);

        Img_Phone.sprite = Resources.Load<Sprite>("UI_Phone/Img_Phone");
    }
}