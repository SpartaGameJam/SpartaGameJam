using TMPro;
using UnityEngine;

public class UI_HUD : UI_Scene
{
    #region
    enum Texts
    {
        Txt_Money
    }
    #endregion

    TextMeshProUGUI Txt_Money;

    void OnEnable()
    {

    }

    void OnDisable()
    {

    }

    protected override void Awake()
    {
        BindTexts(typeof(Texts));

        Txt_Money = GetText((int)Texts.Txt_Money);
    }

    void UpdateUI()
    {
        
    }
}