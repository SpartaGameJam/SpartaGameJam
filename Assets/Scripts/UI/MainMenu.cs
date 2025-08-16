using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : UISelector
{
    [Header("버튼")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button optionButton;

    public event EventHandler OnIngameEnter;

    public GameObject backGroundImg;
    public GameObject titleObject;

    private void Start()
    {
        MainMenuUIManager.Instance.AddPanel(this);
    }

    public void Init()
    {
        LoadSceneManager.Instance.UnLoadScene(SceneName.Ending);

        titleObject.SetActive(true);
        backGroundImg.SetActive(true);
        SpineController.Instance.StartTitle(false, 1);

        GameManager.Instance.ClearData();

        MainMenuUIManager.Instance.ChangePanel(this);
    }


    public void NewStartButton()
    {
        OnIngameEnter?.Invoke(this, EventArgs.Empty);
        MainMenuUIManager.Instance.InitializePannel();

        LoadSceneManager.Instance.LoadScene(SceneName.Ingame);
        SoundManager.instance.PlayBGM(GameplaySound.Main);

        Debug.Log("NewStartButton");
    }

    public void ContinueButton()
    {
        // LoadSceneManager.Instance.LoadScene();
        //UIManager.Instance.BackPannel(); // 현재 MainMenuPanel 제거 (실제 인게임 진입 시 주석 제거)
        Debug.Log("ContinueButton");
    }

    public void SettingButton()
    {
        MainMenuUIManager.Instance.ChangePanel(MainMenuUIManager.Instance.SettingPanel);

        SoundManager.instance.PlaySFX(SFXSound.ButtonClick);
    }

    public void ExitButton()
    {
        Application.Quit();
        Debug.Log("게임 종료");

        SoundManager.instance.PlaySFX(SFXSound.ButtonClick);
    }

    public override void Back()
    {
        Debug.Log("Back 지원 안함");

        SoundManager.instance.PlaySFX(SFXSound.ButtonClick);
    }
}
