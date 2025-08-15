using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISelector : UI_Base
{
    [Header("Content")]
    [SerializeField] protected GameObject content;

    [SerializeField] protected GameObject defaultButton;
    
    public virtual void ClosePopupUI()
    {
        UIManager.Instance.ClosePopupUI();
    }

    public virtual void Back()
    {
        UIManager.Instance.BackPannel();
    }
}
