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
    [SerializeField] private float forwardMoveZ = -50f; // 앞으로 튀어나오기 z 변화량
    [SerializeField] private float forwardMoveDuration = 0.1f; // 튀어나오기 시간
    [SerializeField] private float moveXOffset = 300f; // 오른쪽으로 이동할 X 거리
    [SerializeField] private float moveXDuration = 1f; // 오른쪽 이동 시간

    [Header("Dust Settings")]
    [SerializeField] private ParticleSystem scratchDustPrefab;
    [SerializeField] private float particleSpacing = 10f; // 가루 간격(px)
    private Vector3 lastDustPos;
    private bool firstDust = true;

    Texture2D scratchTex;
    Material shineMat;
    int totalPixels;
    int erasedPixels;
    bool isCleared;

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

    public void OnPointerDown(PointerEventData eventData)
    {
        ScratchAt(eventData);
        firstDust = true; // 첫 긁기 시 첫 파티클 보장

        ExecuteEvents.Execute<IPointerDownHandler>(
            rootTransform.gameObject, eventData, ExecuteEvents.pointerDownHandler
        );
    }

    public void OnDrag(PointerEventData eventData)
    {
        ScratchAt(eventData);

        ExecuteEvents.Execute<IDragHandler>(
            rootTransform.gameObject, eventData, ExecuteEvents.dragHandler
        );
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        CheckClear();
        ExecuteEvents.Execute<IPointerUpHandler>(
            rootTransform.gameObject, eventData, ExecuteEvents.pointerUpHandler
        );
    }

    void CheckClear()
    {
        if (isCleared) return;

        float percent = (erasedPixels / (float)totalPixels) * 100f;
        if (percent >= clearThreshold)
        {
            isCleared = true;
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
        Debug.Log($"{lotto.CurrentResult}");
        if (lotto == null) return LottoResult.NoMatch;
        return lotto.CurrentResult;
    }

    void PlayResultAnimation()
    {
        LottoResult result = GetLottoResult();
        float moveX = rootTransform.position.x + moveXOffset;

        Sequence seq = DOTween.Sequence();
        LottoTiltShader tilt = rootTransform.GetComponent<LottoTiltShader>();

        if (result == LottoResult.ThreeMatch || result == LottoResult.OneMore)
        {
            seq.Append(rootTransform.DOLocalMoveZ(rootTransform.localPosition.z + forwardMoveZ, forwardMoveDuration));
            if (tilt != null)
            {
                seq.AppendCallback(() => tilt.DoShake());
                float shakeTime = tilt.shakeStepTime * tilt.shakeLoops * 2f;
                seq.AppendInterval(shakeTime);
            }
            seq.Append(rootTransform.DOMoveX(moveX, moveXDuration));
        }
        else if (result == LottoResult.TwoMatch)
        {
            if (tilt != null)
            {
                seq.AppendCallback(() => tilt.DoShake());
                float shakeTime = tilt.shakeStepTime * tilt.shakeLoops * 2f;
                seq.AppendInterval(shakeTime);
            }
            seq.Append(rootTransform.DOMoveX(moveX, moveXDuration));
        }
        else
        {
            seq.Append(rootTransform.DOMoveX(moveX, moveXDuration));
        }

        seq.OnComplete(() => Destroy(rootTransform.parent.gameObject));
    }

    void ScratchAt(PointerEventData eventData)
    {
        Vector2 localPos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            scratchImage.rectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out localPos))
        {
            Rect rect = scratchImage.rectTransform.rect;
            float u = (localPos.x - rect.x) / rect.width;
            float v = (localPos.y - rect.y) / rect.height;

            int px = Mathf.RoundToInt(u * scratchTex.width);
            int py = Mathf.RoundToInt(v * scratchTex.height);

            // Erase에서 긁힌 여부 체크
            bool erasedSomething = Erase(px, py);
            if (!erasedSomething) return; // 아무것도 안 지워졌으면 파티클 안 만듦

            CheckClear();

            Vector3 worldPos = scratchImage.rectTransform.TransformPoint(localPos);

            // 파티클 경로 생성
            if (scratchDustPrefab != null)
            {
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
        }
    }

    void EmitDust(Vector3 worldPos)
    {
        ParticleSystem ps = Instantiate(scratchDustPrefab, worldPos, Quaternion.identity, scratchImage.canvas.transform);

        // Lotto 종이보다 위에 표시
        var renderer = ps.GetComponent<ParticleSystemRenderer>();
        renderer.sortingOrder = 999;

        var main = ps.main;
        main.stopAction = ParticleSystemStopAction.Destroy;

        ps.Emit(Random.Range(3, 6));
    }


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
                        if (current.a > 0f) // 아직 지워지지 않은 픽셀만
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
