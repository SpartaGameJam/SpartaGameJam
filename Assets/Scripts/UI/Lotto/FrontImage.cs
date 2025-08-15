using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class FrontImage : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [Header("Scratch Settings")]
    public Image scratchImage;
    public Texture2D brushTexture;
    public float clearThreshold = 70f; // 퍼센트(%)

    [Header("Shine Settings")]
    public Image shineImage;
    public Transform rootTransform;

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
        // 퍼센트 체크
        float percent = (erasedPixels / (float)totalPixels) * 100f;
        if (percent >= clearThreshold)
        {
            if (!isCleared)
            {
                isCleared = true;
                // 진동
                rootTransform.GetComponent<LottoTiltShader>().DoShake();
                // TODO : 여기서 다 긁었을 때 함수 실행시키기
            }
        }

        ExecuteEvents.Execute<IPointerUpHandler>(
            rootTransform.gameObject, eventData, ExecuteEvents.pointerUpHandler
        );
    }

    private void ScratchAt(PointerEventData eventData)
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

            Erase(px, py);
        }
    }

    private void Erase(int cx, int cy)
    {
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
                        }
                    }
                }
            }
        }
        scratchTex.Apply();
    }

    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -180f) angle += 360f;
        if (angle > 180f) angle -= 360f;
        return Mathf.Clamp(angle, min, max);
    }

    private float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return from2 + (value - from1) * (to2 - from2) / (to1 - from1);
    }
}
