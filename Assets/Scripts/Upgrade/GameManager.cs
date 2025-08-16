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
    public float _money;

    public float Money
    {
        get { return _money; }
        set { _money = value; }
    }
    #endregion

}
