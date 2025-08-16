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
        EventManager.Instance.AddEvent(EEventType.MoneyChanged, RefreshUI);
    }

    void OnDisable()
    {
        EventManager.Instance.RemoveEvent(EEventType.MoneyChanged, RefreshUI);
    }

    void Start()
    {
        EventManager.Instance.TriggerEvent(EEventType.MoneyChanged);
    }

    public void SetUP()
    {
        gameObject.SetActive(true);
    }

    protected override void Awake()
    {
        BindTexts(typeof(Texts));

        Txt_Money = GetText((int)Texts.Txt_Money);
    }

    void RefreshUI()
    {
        Txt_Money.text = $"{GameManager.Instance.Money}G";
    }

    public void SHOWMETHEMONEY()
    {
        GameManager.Instance.UpdateMoney(22312323);
        EventManager.Instance.TriggerEvent(EEventType.MoneyChanged);

        SoundManager.instance.PlaySFX(SFXSound.GetGold);
    }
}