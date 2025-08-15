using TMPro;
using UnityEngine;

public class UpgradeSlot : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI Txt_UpgradeValue;
    [SerializeField] TextMeshProUGUI Txt_UpgradePrice;

    public void SetupByUpgradeType(UpgradeType upgradeType)
    {
        switch (upgradeType)
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
}
