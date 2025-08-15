using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Shop : UI_Scene
{
    #region enum
    enum Texts
    {
        Txt_CashierDialogue
    }

    enum Buttons
    {
        Btn_ReturnToOffice,
        Btn_Lotto
    }

    enum Images
    {
        Img_BG02,
        Img_Counter,

        Img_Cashier
    }

    #endregion

    Button Btn_ReturnToOffice;
    Button Btn_Lotto;

    Image Img_BG02;
    Image Img_Counter;

    TextMeshProUGUI Txt_CashierDialogue;
    Image Img_Cashier;


    protected override void Awake()
    {
        BindTexts(typeof(Texts));
        BindButtons(typeof(Buttons));
        BindImages(typeof(Images));

        Txt_CashierDialogue = GetText((int)Texts.Txt_CashierDialogue);

        Btn_ReturnToOffice = GetButton((int)Buttons.Btn_ReturnToOffice);
        Btn_Lotto = GetButton((int)Buttons.Btn_Lotto);

        Img_BG02 = GetImage((int)Images.Img_BG02);
        Img_Counter = GetImage((int)Images.Img_Counter);

        Img_Cashier = GetImage((int)Images.Img_Cashier);

        Img_BG02.sprite = Resources.Load<Sprite>("UI_Shop/Img_BG02");
        Img_Counter.sprite = Resources.Load<Sprite>("UI_Shop/Img_Counter");

        BindEvent(Btn_Lotto.gameObject, OnClickLotto);
    }

    void OnClickLotto(PointerEventData eventData)
    {
        //void StartScratchTicket()
    }
}
