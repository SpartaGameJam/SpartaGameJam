using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISelector : UI_Base
{
    [Header("Content")]
    [SerializeField] protected GameObject content;

    [SerializeField] protected GameObject defaultButton;

    protected override void Awake()
    {
        base.Awake();
    }


    protected virtual void OnEnable()
    {

    }
    
    public virtual void ClosePopupUI()
    {
        UIManager.Instance.ClosePopupUI();
    }

    public virtual void Back()
    {
        UIManager.Instance.BackPannel();
    }
}
