using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SettingOption
{
    Gameplay, Graphic, Sound, KeyBindings
}

public class SettingMenu : UISelector
{
    [Header("버튼")]
    [SerializeField] private Button gameplayButton;

    [Header("OptionView")]
    [SerializeField] private GameObject[] optionViews;

    [SerializeField] private SettingOption curOption = SettingOption.Gameplay;

    public void Start()
    {
        ChangeView(curOption);
    }

    public void GameplayButton()
    {
        Debug.Log("게임 플레이 클릭");
        ChangeView(SettingOption.Gameplay);
        
        SoundManager.instance.PlaySFX(SFXSound.ButtonClick);
    }

    public void GraphicButton()
    {
        Debug.Log("언어");
        ChangeView(SettingOption.Graphic);
        
        SoundManager.instance.PlaySFX(SFXSound.ButtonClick);
    }

    public void SoundButton()
    {
        Debug.Log("사운드");
        ChangeView(SettingOption.Sound);
        
        SoundManager.instance.PlaySFX(SFXSound.ButtonClick);
    }

    public void KeyBinidingButton()
    {
        Debug.Log("키바인딩");
        ChangeView(SettingOption.KeyBindings);
        
        SoundManager.instance.PlaySFX(SFXSound.ButtonClick);
    }

    public void ChangeView(SettingOption options)
    {
        optionViews[(int)curOption].SetActive(false); // ?는 나중에 오브젝트 전부 할당되면 삭제
        curOption = options;
        optionViews[(int)curOption].SetActive(true);
    }

    public override void Back()
    {
        ChangeView(SettingOption.Gameplay);
        MainMenuUIManager.Instance.BackPannel();
        
        SoundManager.instance.PlaySFX(SFXSound.ButtonClick);
    }
}
