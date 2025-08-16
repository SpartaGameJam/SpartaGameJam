using System.Collections.Generic;
using UnityEngine;

public enum UpgradeType
{
    None,
    CurrencyGain,
    ExtraChanceRate,
    FeverTriggerRate,
    LotteryWinRate,
    LotteryDiscountRate
}

public class UpgradeData
{
    public UpgradeType statType;
    public int level;

    public float baseStatValue;
    public float valueIncrease;

    public float baseCost;
    public float costIncrease;


    public float GetCurStatValue() => baseStatValue + level * valueIncrease;
    public float GetUpgradeCost() => baseCost + level * costIncrease;
}


public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;
    public List<UpgradeData> upgradeData;

    private readonly Dictionary<UpgradeType, UpgradeData> _cache = new Dictionary<UpgradeType, UpgradeData>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        Init();
    }

    void Init()
    {
        BuildCache();
    }

    private void BuildCache()
    {
        _cache.Clear();
        if (upgradeData == null) return;
        foreach (UpgradeData ud in upgradeData)
        {
            if (ud == null) continue;
            _cache[ud.statType] = ud;
            GameManager.Instance.UpdateStat(ud.statType, ud.GetCurStatValue());
        }
    }

    public int GetLevel(UpgradeType stat)
    {
        return _cache.TryGetValue(stat, out var u) ? u.level : 0;
    }


    public float GetCurrentValue(UpgradeType stat)
    {
        if (_cache.TryGetValue(stat, out var u))
            return u.GetCurStatValue();
        return GameManager.Instance.GetStatValue(stat);
    }


    public float GetUpgradeCost(UpgradeType stat)
    {
        return _cache.TryGetValue(stat, out var u) ? u.GetUpgradeCost() : -1f;
    }

    public void TryUpgrade(UpgradeType stat)
    {
        if (GetUpgradeCost(stat) > GameManager.Instance.Money)
        {
            Debug.Log("돈이 모자랍니다");
            return;
        }

        UpgradeData upgradeData = _cache[stat];

        GameManager.Instance.Money -= upgradeData.GetUpgradeCost();

        upgradeData.level++;
        float newValue = upgradeData.GetCurStatValue();
        GameManager.Instance.UpdateStat(stat, newValue);

        EventManager.Instance.TriggerEvent(EEventType.Upgraded);
    }
}