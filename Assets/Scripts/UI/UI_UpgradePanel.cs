using UnityEngine;

public enum UpgradeType
{
    CurrencyGain,
    ExtraChanceRate,
    FeverTriggerRate,
    LotteryWinRate,
    LotteryDiscountRate
}

public class UI_UpgradePanel : UISelector
{
    Transform SlotPanel;
    UpgradeSlot[] slots = new UpgradeSlot[5];

    void Start()
    {
        SlotPanel = ComponentHelper.TryFindChild(this, "SlotPanel");

        for (int i = 0; i < SlotPanel.childCount; i++)
        {
            GameObject go = SlotPanel.GetChild(i).gameObject;
            UpgradeSlot slot = go.GetComponent<UpgradeSlot>();
            slots[i] = slot;
        }
    }
}
