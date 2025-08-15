using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Init();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [Header("Panels")]
    //[SerializeField] private UISelector pausePanel;
    [SerializeField] private UISelector settingPanel;
    //public UISelector PausePanel => pausePanel;
    public UISelector SettingPanel => settingPanel;

    private Stack<UISelector> panelStack = new Stack<UISelector>();
    
    public Dictionary<string, UI_Base> UIs = new();

    public void Init()
    {
        RegisterAllUIs();
    }

    void RegisterAllUIs()
    {
        UIs.Clear();

        UI_Base[] list = GetComponentsInChildren<UI_Base>(true);

        foreach (UI_Base ui in list)
        {
            string key = ui.gameObject.name;

            if (UIs.ContainsKey(key))
            {
                Debug.LogWarning($"Duplicate popup detected: : {key}");
                continue;
            }

            UIs.Add(key, ui);
        }
    }

    public void ShowPopup<T>(Action<UISelector> callback = null, Transform parent = null) where T : UISelector
    {
        String key = typeof(T).Name;

        if (UIs.TryGetValue(key, out UI_Base ui) == false)
        {
            Debug.LogError($"Popup not registered: {key}");
        }
        
        T popuupUI = ui as T;


        panelStack.Push(popuupUI);

        if (panelStack.Count <= 0) return;

        UISelector top = panelStack.Peek();
        top.Show();

        callback?.Invoke(top);

        if (parent != null)
            top.transform.SetParent(parent);
    }


    public void CloseAllPopupUI()
    {
        while (panelStack.Count > 0)
            ClosePopupUI();
    }


    public void ClosePopupUI()
    {
        if (panelStack.Count == 0) return;
        panelStack.Pop().Hide();

        if (panelStack.Count > 0)
            panelStack.Peek().Show();
    }

    /// <summary>
    /// panelStack에 현재 패널을 추가합니다.
    /// </summary>
    public void AddPanel(UISelector newSelector)
    {
        panelStack.Push(newSelector);
    }

    /// <summary>
    /// 패널 카운트 반환
    /// </summary>
    /// <returns></returns>
    public int PannelCount()
    {
        return panelStack.Count;
    }

    /// <summary>
    ///  최상단 패널 활성화
    /// </summary>
    public void TopPanelShow()
    {
        if (panelStack.Count <= 0) return;
        panelStack.Peek().Show();
    }

    /// <summary>
    ///  최상단 패널 비활성화
    /// </summary>
    public void TopPaneHide()
    {
        if (panelStack.Count != 1) return;
        panelStack.Peek().Hide();
    }

    /// <summary>
    /// 현재 패널을 숨기고 새로운 패널을 활성화
    /// </summary>
    /// <param name="newSelector">새로운 패널</param>

    public void ChangePanel(UISelector newSelector)
    {
        /*postUISelector?.Hide();
        postUISelector = curUISelector;
        curUISelector = newSelector;
        curUISelector?.Show();*/

        if (panelStack.Count > 0) panelStack?.Peek().Hide();
        AddPanel(newSelector);
        panelStack.Peek().Show();
    }

    
    /// <summary>
    /// 현재 패널을 숨기고 새로운 패널을 활성화
    /// Action은 UISetUp 혹은 DisplayResultData
    /// panret는 Panel타입의 UI컴포넌트들을 UI안에 집어넣을 때 사용
    /// </summary>
    public void ChangePanel(UISelector newSelector, Action<UISelector> callback = null, Transform parent = null)
    {
        if (panelStack.Count > 0) panelStack?.Peek().Hide();

        AddPanel(newSelector);
        UISelector top = panelStack.Peek();
        panelStack.Peek().Show();

        callback?.Invoke(top);

        if (parent != null)
            top.transform.SetParent(parent);
    }
    

    /// <summary>
    /// 현재 패널을 숨기고 이전 패널을 활성화
    /// </summary>
    public void BackPannel()
    {
        if (panelStack.Count == 0) return; // 현재 활성화된 패널이 없으면 실행 x
        panelStack.Pop().Hide();
        if (panelStack.Count > 0)
            panelStack.Peek().Show();
    }

    /// <summary>
    /// 패널을 전부 초기화
    /// </summary>
    public void InitializePannel()
    {
        while(panelStack.Count > 0)  
            panelStack.Pop().Hide();
    }
}
