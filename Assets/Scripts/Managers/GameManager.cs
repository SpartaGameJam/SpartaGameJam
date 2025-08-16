using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    #region Upgrade
    private readonly Dictionary<UpgradeType, float> _stats = new();

    public float GetStatValue(UpgradeType type)
    {
        if (_stats.TryGetValue(type, out float value))
        {
            return value;
        }
        else
        {
            return 0f;
        }
    }

    public void UpdateStat(UpgradeType type, float value)
    {
        if (_stats.ContainsKey(type))
        {
            _stats[type] = value;
        }
        else
        {
            _stats.Add(type, value);
        }
    }
    #endregion

    #region Money
    public long _money;

    public long Money
    {
        get { return _money; }
        //set { _money = value; }
    }
    #endregion

    public long useMoney { get; private set; } // 전체 사용 돈

    public int failCount { get; private set; } // 로또 실패 카운드
    public int useLottoCount { get; private set; } // 복권 사용 수
    public int completeWork { get; private set; } // 작업 완료 횟수
    public int fiverCount { get; private set; } // 피버카운트

    public int upgradeCount { get; private set; } // 업그레이드 횟수

    public float GetGainPer()
    {
        return UpgradeManager.Instance.GetCurrentValue(UpgradeType.CurrencyGain);
    }


    public void UpdateMoney(long money)
    {
        if (money < 0)
        {
            useMoney -= money; // 음수니 빼서 양수로
        }

        _money += money;
    }

    public void FailLotto()
    {
        failCount++;
    }

    public void UpdateLotto()
    {
        useLottoCount++;
    }

    public void CompleteWork()
    {
        completeWork++;
    }

    public void UpdateFiver()
    {
        fiverCount++;
    }

    public void UpdateUpgrade()
    {
        upgradeCount++;
    }

    public bool IsFeverTime = false;

    public void ClearData()
    {
        _stats.Clear();

        _money = 0;
        failCount = 0;
        useLottoCount = 0;
        completeWork = 0;
        fiverCount = 0;
        upgradeCount = 0;

    }
}
