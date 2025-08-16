using UnityEngine;
using DG.Tweening;

public class UI_BlurPanel : MonoBehaviour
{
    public bool IsBlured => isBlured;
    bool isBlured;
    Canvas canvas;
    CanvasGroup canvasGroup;

    void Start()
    {
        canvas = GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;

        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    /// <summary>
    /// 지정한 시간 동안 CanvasGroup alpha를 1로 변경 (보이게)
    /// </summary>
    public void FadeIn(float duration = 0.5f)
    {
        if (isBlured)
            return;

        isBlured = true;
        canvasGroup.DOFade(1f, duration).SetUpdate(true);
    }

    /// <summary>
    /// 지정한 시간 동안 CanvasGroup alpha를 0으로 변경 (숨김)
    /// </summary>
    public void FadeOut(float duration = 0.2f)
    {
        if (!isBlured)
            return;

        isBlured = false;
        canvasGroup.DOFade(0f, duration).SetUpdate(true);
    }
}
