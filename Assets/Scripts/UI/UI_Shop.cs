using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Shop : UI_Scene
{
    #region enum
    enum Texts
    {
        Txt_CashierDialogue
    }

    enum Buttons
    {
        Btn_ReturnToOffice,
        //Btn_Lotto
    }

    enum Images
    {
        Img_BG02,
        Img_Counter,
        Img_ReturnToOffice,

        Img_Cashier
    }

    enum Objects
    {
        Obj_Cashier,
    }

    #endregion

    Button Btn_ReturnToOffice;
    //Button Btn_Lotto;

    Image Img_BG02;
    Image Img_Counter;
    Image Img_ReturnToOffice;

    TextMeshProUGUI Txt_CashierDialogue;
    Image Img_Cashier;

    GameObject Obj_Cashier;


    protected override void Awake()
    {
        BindTexts(typeof(Texts));
        BindButtons(typeof(Buttons));
        BindImages(typeof(Images));
        BindObjects(typeof(Objects));

        Txt_CashierDialogue = GetText((int)Texts.Txt_CashierDialogue);

        Btn_ReturnToOffice = GetButton((int)Buttons.Btn_ReturnToOffice);
        //Btn_Lotto = GetButton((int)Buttons.Btn_Lotto);

        Img_BG02 = GetImage((int)Images.Img_BG02);
        Img_Counter = GetImage((int)Images.Img_Counter);
        Img_ReturnToOffice = GetImage((int)Images.Img_ReturnToOffice);

        Img_Cashier = GetImage((int)Images.Img_Cashier);

        Obj_Cashier = GetObject((int)Objects.Obj_Cashier);

        Img_BG02.sprite = Resources.Load<Sprite>("UI_Shop/Img_BG02");
        Img_Counter.sprite = Resources.Load<Sprite>("UI_Shop/Img_Counter");
        Img_ReturnToOffice.sprite = Resources.Load<Sprite>("UI_Shop/Img_ReturnToOffice");
        Img_Cashier.sprite = Resources.Load<Sprite>("Char/Img_Juno002");

        Btn_ReturnToOffice.onClick.AddListener(OnReturnToOffice);

        BindEvent(Obj_Cashier, CashierClick);

        _workOriginPos = Obj_Cashier.GetComponent<RectTransform>().anchoredPosition;
    }

    
    public string GetRandomDialogue()
    {
        int index = Random.Range(0, StringNameSpace.dialogues.Length);
        return StringNameSpace.dialogues[index];
    }

    public Sprite LoadEmotion(int emotionId)
    {
        string emotionName = "Img_Juno" + emotionId.ToString("000");
        Sprite emotion = Resources.Load<Sprite>($"Char/{emotionName}");

        return emotion;
    }

    void CashierClick(PointerEventData pointerEventData)
    {
        OnClickWorkInstructionPanel();

        int randomIndex = UnityEngine.Random.Range(0, 4);
        Img_Cashier.sprite = LoadEmotion(randomIndex);

        Txt_CashierDialogue.text = GetRandomDialogue().ToString();
    }


    [SerializeField] private float workTargetPosY = 180f;
    [SerializeField] private float workMoveDuration = 0.6f;

    private Vector2 _workOriginPos;
    private bool _workIsAtTarget = false;


    public void OnClickWorkInstructionPanel()
    {
        RectTransform rt = Obj_Cashier.GetComponent<RectTransform>();

        // DG_MoveEase 없으면 자동 부착
        DG_MoveEase mover = Obj_Cashier.GetComponent<DG_MoveEase>();
        if (mover == null) mover = Obj_Cashier.AddComponent<DG_MoveEase>();

        // 현재 위치 기준 토글 (X는 유지, Y만 변경)
        Vector2 nextPosAnchored = _workIsAtTarget ? _workOriginPos : new Vector2(rt.anchoredPosition.x, 180);

        mover.target = rt;
        mover.duration = workMoveDuration;

        mover.targetPos = nextPosAnchored;

        // anchoredPosition 기준 이동을 원하므로 Local 버전 호출
        mover.PlayLocalMove();

        _workIsAtTarget = !_workIsAtTarget;
    }



    #region Button
    void OnReturnToOffice()
    {
        FindAnyObjectByType<LottoSystem>(FindObjectsInactive.Include).gameObject.SetActive(false);

        UIManager.Instance.CloseAllPopupUI();
        UIManager.Instance.ChangeSceneUI<UI_Play>();
    }
    #endregion
}
