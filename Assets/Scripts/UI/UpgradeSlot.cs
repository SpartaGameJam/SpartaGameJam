using TMPro;
using UnityEngine;

public class UpgradeSlot : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI Txt_UpgradeValue;
    [SerializeField] TextMeshProUGUI Txt_UpgradePrice;

    public UpgradeType UpgradeType;
    public int Index;

    public void SetupByUpgradeType()
    {
        if (UpgradeType == UpgradeType.None)
            return;

        switch (UpgradeType)
        {
            case UpgradeType.CurrencyGain:
                Txt_UpgradeValue.text = $"{StringNameSpace.CurrencyGain}(Lv.0)+88%";
                Txt_UpgradePrice.text = $"8888";
                break;
            case UpgradeType.ExtraChanceRate:
                Txt_UpgradeValue.text = $"{StringNameSpace.ExtraChanceRate}(Lv.0)+88%";
                Txt_UpgradePrice.text = $"8888";
                break;
            case UpgradeType.FeverTriggerRate:
                Txt_UpgradeValue.text = $"{StringNameSpace.FeverTriggerRate}(Lv.0)+88%";
                Txt_UpgradePrice.text = $"8888";
                break;
            case UpgradeType.LotteryWinRate:
                Txt_UpgradeValue.text = $"{StringNameSpace.LotteryWinRate}(Lv.0)+88%";
                Txt_UpgradePrice.text = $"8888";
                break;
            case UpgradeType.LotteryDiscountRate:
                Txt_UpgradeValue.text = $"{StringNameSpace.LotteryDiscountRate}(Lv.0)+88%";
                Txt_UpgradePrice.text = $"8888";
                break;
        }
    }
    
    public void OnSlotClicked()
    {
        UIManager.Instance.FindUIPopup<UI_UpgradePanel>()?.OnSlotClicked(Index);
    }
}