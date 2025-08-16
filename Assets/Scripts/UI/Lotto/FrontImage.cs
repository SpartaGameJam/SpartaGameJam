using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FrontImage : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [Header("Scratch Settings")]
    public Image scratchImage;
    public Texture2D brushTexture;
    public float clearThreshold = 60f; // 퍼센트(%)

    [Header("Shine Settings")]
    public Image shineImage;
    public Transform rootTransform;

    [Header("Animation Settings")]
    [SerializeField] private float forwardMoveZ = -100f;
    [SerializeField] private float forwardMoveDuration = 0.1f;
    [SerializeField] private float moveXOffset = 300f;
    [SerializeField] private float moveXDuration = 1f;

    [Header("Dust Settings")]
    [SerializeField] private ParticleSystem scratchDustPrefab;
    [SerializeField] private float particleSpacing = 10f;
    private Vector3 lastDustPos;
    private bool firstDust = true;

    Texture2D scratchTex;
    Material shineMat;
    int totalPixels;
    int erasedPixels;
    bool isCleared;

    // 코인 진입 여부
    bool coinInside = false;

    void Start()
    {
        Texture2D original = scratchImage.sprite.texture;
        scratchTex = new Texture2D(original.width, original.height, TextureFormat.RGBA32, false);
        scratchTex.SetPixels(original.GetPixels());
        scratchTex.Apply();

        scratchImage.sprite = Sprite.Create(
            scratchTex,
            new Rect(0, 0, scratchTex.width, scratchTex.height),
            new Vector2(0.5f, 0.5f)
        );

        totalPixels = scratchTex.width * scratchTex.height;
        erasedPixels = 0;
        isCleared = false;

        if (shineImage != null)
        {
            shineMat = new Material(shineImage.material);
            shineImage.material = shineMat;
        }

        if (rootTransform == null)
            rootTransform = transform.root;
    }

    void Update()
    {
        // 광택 연출용 셰이더 파라미터
        if (shineMat != null && rootTransform != null)
        {
            Vector3 euler = rootTransform.localRotation.eulerAngles;
            float xAngle = ClampAngle(euler.x, -90f, 90f);
            float yAngle = ClampAngle(euler.y, -90f, 90f);

            float remapX = Remap(xAngle, -20, 20, -0.5f, 0.5f);
            float remapY = Remap(yAngle, -20, 20, -0.5f, 0.5f);

            shineMat.SetVector("_Rotation", new Vector2(remapX, remapY));
        }
    }

    // 마우스로 긁기 (지우지 않고, dust도 없음, 부모 이벤트만)
    public void OnPointerDown(PointerEventData eventData)
    {
        ExecuteEvents.Execute<IPointerDownHandler>(
            rootTransform.gameObject, eventData, ExecuteEvents.pointerDownHandler
        );
    }

    public void OnDrag(PointerEventData eventData)
    {
        ExecuteEvents.Execute<IDragHandler>(
            rootTransform.gameObject, eventData, ExecuteEvents.dragHandler
        );
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ExecuteEvents.Execute<IPointerUpHandler>(
            rootTransform.gameObject, eventData, ExecuteEvents.pointerUpHandler
        );
    }

    // 코인 긁기 (지우기 + Dust + 부모 이벤트 + 회전)
    public void ScratchAtWorldPos(Vector3 worldPos)
    {
        if (isCleared) return;

        Vector2 localPos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            scratchImage.rectTransform,
            worldPos,
            null,
            out localPos))
        {
            Rect rect = scratchImage.rectTransform.rect;
            float u = (localPos.x - rect.x) / rect.width;
            float v = (localPos.y - rect.y) / rect.height;

            int px = Mathf.RoundToInt(u * scratchTex.width);
            int py = Mathf.RoundToInt(v * scratchTex.height);

            bool erased = Erase(px, py);

            if (erased)
            {
                CheckClear();
                if (isCleared) return;
                EmitScratchDust(localPos);

                // 코인이 처음 닿았을 때 PointerDown 이벤트 실행
                if (!coinInside)
                {
                    coinInside = true;
                    var downData = new PointerEventData(EventSystem.current) { position = worldPos };
                    ExecuteEvents.Execute<IPointerDownHandler>(rootTransform.gameObject, downData, ExecuteEvents.pointerDownHandler);
                }

                // 긁는 동안 Drag 이벤트 실행
                var dragData = new PointerEventData(EventSystem.current) { position = worldPos };
                ExecuteEvents.Execute<IDragHandler>(rootTransform.gameObject, dragData, ExecuteEvents.dragHandler);

                // 회전 처리 (코인 방향 따라)
                Vector3 dir = (worldPos - rootTransform.position).normalized;
                float tiltX = Mathf.Clamp(-dir.y * 10f, -15f, 15f);
                float tiltY = Mathf.Clamp(dir.x * 10f, -15f, 15f);

                LottoTiltShader tilt = rootTransform.GetComponent<LottoTiltShader>();
                if (tilt != null)
                    tilt.SetExternalTilt(new Vector2(tiltX, tiltY));
            }
        }
    }


    // 코인 드래그 시작할 때 Reset (UI_Coin에서 호출해줘야 함)
    public void ResetCoinState()
    {
        coinInside = false;

        LottoTiltShader tilt = rootTransform.GetComponent<LottoTiltShader>();
        if (tilt != null)
            tilt.ReleaseExternalTilt();
    }

    // 실제 스크래치 지우기
    bool Erase(int cx, int cy)
    {
        bool erasedAny = false;
        int bw = brushTexture.width;
        int bh = brushTexture.height;

        for (int x = 0; x < bw; x++)
        {
            for (int y = 0; y < bh; y++)
            {
                Color bc = brushTexture.GetPixel(x, y);
                if (bc.a > 0f)
                {
                    int px = cx + x - bw / 2;
                    int py = cy + y - bh / 2;
                    if (px >= 0 && px < scratchTex.width && py >= 0 && py < scratchTex.height)
                    {
                        Color current = scratchTex.GetPixel(px, py);
                        if (current.a > 0f)
                        {
                            erasedPixels++;
                            scratchTex.SetPixel(px, py, new Color(0, 0, 0, 0));
                            erasedAny = true;
                        }
                    }
                }
            }
        }
        if (erasedAny) scratchTex.Apply();
        return erasedAny;
    }

    // Dust 생성
    void EmitScratchDust(Vector2 localPos)
    {
        if (scratchDustPrefab == null) return;

        Vector3 worldPos = scratchImage.rectTransform.TransformPoint(localPos);

        if (firstDust)
        {
            EmitDust(worldPos);
            lastDustPos = worldPos;
            firstDust = false;
        }
        else
        {
            float dist = Vector3.Distance(lastDustPos, worldPos);
            if (dist > particleSpacing)
            {
                int steps = Mathf.FloorToInt(dist / particleSpacing);
                for (int i = 1; i <= steps; i++)
                {
                    Vector3 stepPos = Vector3.Lerp(lastDustPos, worldPos, i / (float)steps);
                    EmitDust(stepPos);
                }
                lastDustPos = worldPos;
            }
        }
    }

    void EmitDust(Vector3 worldPos)
    {
        ParticleSystem ps = Instantiate(scratchDustPrefab, worldPos, Quaternion.identity, scratchImage.canvas.transform);

        var renderer = ps.GetComponent<ParticleSystemRenderer>();
        renderer.sortingOrder = 999;

        var main = ps.main;
        main.stopAction = ParticleSystemStopAction.Destroy;

        ps.Emit(Random.Range(3, 6));
    }

    void CheckClear()
    {
        if (isCleared) return;

        float percent = (erasedPixels / (float)totalPixels) * 100f;
        if (percent >= clearThreshold)
        {
            isCleared = true;
            ResetCoinState();
            ClearAllScratch();
            PlayResultAnimation();
        }
    }

    void ClearAllScratch()
    {
        Color[] colors = scratchTex.GetPixels();
        Color clear = new Color(0, 0, 0, 0);
        for (int i = 0; i < colors.Length; i++)
            colors[i] = clear;
        scratchTex.SetPixels(colors);
        scratchTex.Apply();
    }

    LottoResult GetLottoResult()
    {
        UI_Lotto lotto = GetComponentInParent<UI_Lotto>();
        if (lotto == null) return LottoResult.NoMatch;
        return lotto.CurrentResult;
    }

    void PlayResultAnimation()
    {
        LottoResult result = GetLottoResult();
        float moveX = rootTransform.position.x + moveXOffset;
        float longWait = 1f;
        float middleWait = 0.5f;
        float shortWait = 0.25f;
        float waitAfterTilt = shortWait;

        switch (result)
        {
            case LottoResult.NoMatch:
            case LottoResult.TwoMatch:
                waitAfterTilt = shortWait;
                break;
            case LottoResult.ThreeCarrot:
            case LottoResult.ThreeRabbit:
            case LottoResult.ThreeRadish:
            case LottoResult.ThreeScoop:
                waitAfterTilt = longWait;
                break;
            case LottoResult.OneMore:
                waitAfterTilt = middleWait;
                break;
            default:
                waitAfterTilt = shortWait;
                break;
        }

        Sequence seq = DOTween.Sequence();
        LottoTiltShader tilt = rootTransform.GetComponent<LottoTiltShader>();

        if (result == LottoResult.ThreeCarrot || result == LottoResult.ThreeRabbit || result == LottoResult.ThreeRadish || result == LottoResult.ThreeScoop || result == LottoResult.OneMore)
        {
            seq.Append(rootTransform.DOLocalMoveZ(rootTransform.localPosition.z + forwardMoveZ, forwardMoveDuration));
            if (tilt != null)
            {
                seq.AppendCallback(() => tilt.DoShake());
                float shakeTime = tilt.shakeStepTime * tilt.shakeLoops * 2f;
                seq.AppendInterval(shakeTime);
                seq.AppendInterval(waitAfterTilt);
            }
            seq.Append(rootTransform.DOMoveX(moveX, moveXDuration));
        }
        else if (result == LottoResult.TwoMatch || result == LottoResult.NoMatch)
        {
            if (tilt != null)
            {
                seq.AppendCallback(() => tilt.DoShake());
                float shakeTime = tilt.shakeStepTime * tilt.shakeLoops * 2f;
                seq.AppendInterval(shakeTime);
                seq.AppendInterval(waitAfterTilt);
            }
            seq.Append(rootTransform.DOMoveX(moveX, moveXDuration));
        }

        // 여기서 결과에 따라 디버그 로그 출력
        seq.OnComplete(() =>
        {
            switch (result)
            {
                case LottoResult.NoMatch:
                    Debug.Log("결과: 꽝!");
                    break;
                case LottoResult.TwoMatch:
                    Debug.Log("결과: 2개 일치");
                    break;
                case LottoResult.ThreeCarrot:
                    Debug.Log("결과: 당근 3개! -> 엔딩씬으로 이동");
                    LoadSceneManager.Instance.ChangeScene(SceneName.Ending, LoadSceneManager.Instance.curSceneName);

                    break;
                case LottoResult.ThreeRabbit:
                    Debug.Log("결과: 토끼 3개! 100000G 획득");
                    GameManager.Instance.Money += 100000;
                    EventManager.Instance.TriggerEvent(EEventType.MoneyChanged);
                    break;
                case LottoResult.ThreeRadish:
                    Debug.Log("결과: 무 3개! 50000G 획득");
                    GameManager.Instance.Money += 50000;
                    EventManager.Instance.TriggerEvent(EEventType.MoneyChanged);
                    break;
                case LottoResult.ThreeScoop:
                    Debug.Log("결과: 국자 3개! 30000G 획득");
                    GameManager.Instance.Money += 30000;
                    EventManager.Instance.TriggerEvent(EEventType.MoneyChanged);
                    break;
                case LottoResult.OneMore:
                    LottoMaker maker = FindAnyObjectByType<LottoMaker>();
                    LottoButton lottoButton = FindAnyObjectByType<LottoButton>();
                    if (maker != null)
                    {
                        UI_Lotto autoLotto = maker.CreateLotto();
                        if (lottoButton != null)
                            lottoButton.HandleLotto(autoLotto);
                    }
                    break;
                default:
                    break;
            }
            Destroy(rootTransform.parent.gameObject);
        });
    }


    float ClampAngle(float angle, float min, float max)
    {
        if (angle < -180f) angle += 360f;
        if (angle > 180f) angle -= 360f;
        return Mathf.Clamp(angle, min, max);
    }

    float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return from2 + (value - from1) * (to2 - from2) / (to1 - from1);
    }
}
