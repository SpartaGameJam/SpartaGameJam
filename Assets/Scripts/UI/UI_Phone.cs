using UnityEngine;
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

    protected override void Awake()
    {
        BindImages(typeof(Images));

        Img_Phone = GetImage((int)Images.Img_Phone);

        Img_Phone.sprite = Resources.Load<Sprite>("UI_Phone/Img_Phone");
    }
}