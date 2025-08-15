using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISelector : MonoBehaviour
{
    [Header("Content")]
    [SerializeField] protected GameObject content;

    [SerializeField] protected GameObject defaultButton;

    protected virtual void OnEnable()
    {
        
    }

    public virtual void Show()
    {
        content.SetActive(true);
    }

    public virtual void Hide()
    {
        content.SetActive(false);
    }

    public virtual void Back()
    {
        UIManager.Instance.BackPannel();
    }
}
