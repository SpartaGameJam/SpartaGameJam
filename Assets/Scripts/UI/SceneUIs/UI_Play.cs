using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

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

        Img_Enemy
    }

    enum Objects
    {
        Obj_Desk,
        Obj_Enemy
    }
    #endregion

    Button Btn_Phone;

    Image Img_BG01;
    Image Img_Desk;
    Image Img_Phone;
    //Image Img_Document;
    //Image Img_Pattern01;



    // 랜덤한 적을 뽑고,
    // 적마다 대사가 다르다고 전제함.
    TextMeshProUGUI Txt_EnemyDialogue;
    Image Img_Enemy;

    // 아래 서류 컨트롤용
    // 그런데 도큐먼트를 별도의 UI로 뺄지 고민 중
    GameObject Obj_Desk;
    GameObject Obj_Enemy;

    // 컴퓨터 속 화면
    // 기능을 추가한다면 이것을 UI로 잡고, 그 안의 내용을 세부적으로 컨트롤하게 하겠지만,
    // 단순히 이미지를 바꾸거나 오브젝트를 바꾸는 것이라면 이대로 사용

    
    [SerializeField] private float workTargetPosY = -900f;
    [SerializeField] private float workMoveDuration = 0.6f;

    private Vector2 _workOriginPos;
    private bool _workIsAtTarget = false;

    [Header("Enemy Auto Move")]
    [SerializeField] private float[] enemyAutoPosY = new float[] { -50f, -480f }; // 예: 부장 -50 ↔ -480
    [SerializeField] private float   enemyAutoInterval = 1.5f;     // 몇 초 간격으로 다음 위치로 이동할지
    [SerializeField] private float   enemyMoveDuration = 0.6f;     // 튕김 이동 시간
    [SerializeField] private float   enemyOvershoot    = 1.2f;     // 튕김 강도

    private Coroutine _coEnemyAutoMove;

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
        //Img_Document = GetImage((int)Images.Img_Document);
        //Img_Pattern01 = GetImage((int)Images.Img_Pattern01);

        Img_Enemy = GetImage((int)Images.Img_Enemy);

        Obj_Desk = GetObject((int)Objects.Obj_Desk);
        Obj_Enemy = GetObject((int)Objects.Obj_Enemy);

        Img_BG01.sprite = Resources.Load<Sprite>("UI_Play/Img_BG01");
        Img_Desk.sprite = Resources.Load<Sprite>("UI_Play/Desks/Img_Monitor01");
        Img_Phone.sprite = Resources.Load<Sprite>("UI_Play/Img_Phone");
        //Img_Document.sprite = Resources.Load<Sprite>("UI_Play/Img_Document");
        //Img_Pattern01.sprite = Resources.Load<Sprite>("UI_Play/Img_Pattern01");

        Img_Enemy.sprite = Resources.Load<Sprite>("Char/Img_BuJangNormal");

        BindEvent(Btn_Phone.gameObject, OnShowPhone);
        BindEvent(Obj_Desk, OnClickWorkInstructionPanel);
        //SoundManager.instance.PlaySFX(SFXSound.Bujang);





        BindEvent(Obj_Enemy, OnClickWorkInstructionPanel);





        _workOriginPos = Obj_Desk.GetComponent<RectTransform>().anchoredPosition;
        workTargetPosY = -720;
    }
    
    
    private void OnEnable()
    {
        // 자동 이동 시작
        if (_coEnemyAutoMove == null)
            _coEnemyAutoMove = StartCoroutine(CoAutoMoveEnemy());
    }

    
    private void OnDisable()
    {
        // 자동 이동 정리
        if (_coEnemyAutoMove != null)
        {
            StopCoroutine(_coEnemyAutoMove);
            _coEnemyAutoMove = null;
        }

        // 트윈 안전 정리
        var rt = Obj_Enemy.GetComponent<RectTransform>();
        DOTween.Kill(rt);
    }

    #region DOTWEEN

    public void OnClickWorkInstructionPanel(PointerEventData _)
    {
        SoundManager.instance.PlaySFX(SFXSound.Bujang01);


        RectTransform rt = Obj_Desk.GetComponent<RectTransform>();

        // DG_MoveEase 없으면 자동 부착
        DG_MoveEase mover = Obj_Desk.GetComponent<DG_MoveEase>();
        if (mover == null) mover = Obj_Desk.AddComponent<DG_MoveEase>();

        // 현재 위치 기준 토글 (X는 유지, Y만 변경)
        Vector2 nextPosAnchored = _workIsAtTarget
            ? _workOriginPos
            : new Vector2(rt.anchoredPosition.x, workTargetPosY);

        mover.target   = rt;
        mover.duration = workMoveDuration;

        mover.targetPos = nextPosAnchored;

        // anchoredPosition 기준 이동을 원하므로 Local 버전 호출
        mover.PlayLocalMove();

        _workIsAtTarget = !_workIsAtTarget;
    }

    
    private System.Collections.IEnumerator CoAutoMoveEnemy()
    {
        var go = Obj_Enemy; // 단일 이미지라면 Img_Enemy.gameObject 로 교체
        var rt = go.GetComponent<RectTransform>();

        // DG_BounceMove 없으면 부착
        var mover = go.GetComponent<DG_BounceMove>();
        if (mover == null) mover = go.AddComponent<DG_BounceMove>();

        mover.target    = rt;
        mover.duration  = enemyMoveDuration;
        mover.overshoot = enemyOvershoot;

        int idx = 0;
        while (true)
        {
            if (enemyAutoPosY == null || enemyAutoPosY.Length == 0)
                yield break;

            // X는 유지, Y만 다음 목표로
            var next = new Vector2(rt.anchoredPosition.x, enemyAutoPosY[idx]);
            mover.targetPos = next;
            mover.PlayBounceMove();

            idx = (idx + 1) % enemyAutoPosY.Length;
            yield return new WaitForSeconds(enemyAutoInterval);
        }
    }

    #endregion


    #region Button
    public void OnShowPhone(PointerEventData eventData)
    {
        SoundManager.instance.PlaySFX(SFXSound.ButtonClick);
        //SoundManager.instance.PlaySFX(SFXSound.Bujang);

        UIManager.Instance.CloseAllPopupUI();
        UIManager.Instance.ShowPopup<UI_Phone>();
        UIManager.Instance.ShowPopup<UI_AppMenuPanel>(null, UIManager.Instance.FindUIPopup<UI_Phone>()?.AppMenuPanelBox);
    }
    #endregion
}