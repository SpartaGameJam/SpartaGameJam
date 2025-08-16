using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//Desk = 80 에서 -520
//부장 = -50 에서 - 480
public class UI_Play : UI_Scene
{
    #region enum
    enum Texts
    {
        Txt_EnemyDialogue
    }

    enum Buttons
    {
        Btn_Phone
    }

    enum Images
    {
        Img_BG01,
        Img_Desk,
        Img_Phone,
        Img_Document,
        Img_Pattern01,

        Img_Enemy
    }

    enum Objects
    {
        Obj_DocumentPanel,
        Obj_WorkInstructionPanel
    }
    #endregion

    Button Btn_Phone;

    Image Img_BG01;
    Image Img_Desk;
    Image Img_Phone;
    Image Img_Document;
    Image Img_Pattern01;

    // 랜덤한 적을 뽑고,
    // 적마다 대사가 다르다고 전제함.
    TextMeshProUGUI Txt_EnemyDialogue;
    Image Img_Enemy;

    // 아래 서류 컨트롤용
    // 그런데 도큐먼트를 별도의 UI로 뺄지 고민 중
    GameObject Obj_DocumentPanel;

    // 컴퓨터 속 화면
    // 기능을 추가한다면 이것을 UI로 잡고, 그 안의 내용을 세부적으로 컨트롤하게 하겠지만,
    // 단순히 이미지를 바꾸거나 오브젝트를 바꾸는 것이라면 이대로 사용
    GameObject Obj_WorkInstructionPanel;


    protected override void Awake()
    {
        BindTexts(typeof(Texts));
        BindButtons(typeof(Buttons));
        BindImages(typeof(Images));
        BindObjects(typeof(Objects));

        Txt_EnemyDialogue = GetText((int)Texts.Txt_EnemyDialogue);

        Btn_Phone = GetButton((int)Buttons.Btn_Phone);

        Img_BG01 = GetImage((int)Images.Img_BG01);
        Img_Desk = GetImage((int)Images.Img_Desk);
        Img_Phone = GetImage((int)Images.Img_Phone);
        Img_Document = GetImage((int)Images.Img_Document);
        Img_Pattern01 = GetImage((int)Images.Img_Pattern01);

        Img_Enemy = GetImage((int)Images.Img_Enemy);

        Obj_DocumentPanel = GetObject((int)Objects.Obj_DocumentPanel);
        Obj_WorkInstructionPanel = GetObject((int)Objects.Obj_WorkInstructionPanel);

        Img_BG01.sprite = Resources.Load<Sprite>("UI_Play/Img_BG01");
        Img_Desk.sprite = Resources.Load<Sprite>("UI_Play/Img_Desk");
        Img_Phone.sprite = Resources.Load<Sprite>("UI_Play/Img_Phone");
        Img_Document.sprite = Resources.Load<Sprite>("UI_Play/Img_Document");
        Img_Pattern01.sprite = Resources.Load<Sprite>("UI_Play/Img_Pattern01");

        Img_Enemy.sprite = Resources.Load<Sprite>("Char/Img_BuJangNormal");

        BindEvent(Btn_Phone.gameObject, OnShowPhone);
    }


    #region Button
    public void OnShowPhone(PointerEventData eventData)
    {
        UIManager.Instance.CloseAllPopupUI();
        UIManager.Instance.ShowPopup<UI_Phone>();
        UIManager.Instance.ShowPopup<UI_AppMenuPanel>(null, UIManager.Instance.FindUIPopup<UI_Phone>()?.AppMenuPanelBox);
    }
    #endregion
}