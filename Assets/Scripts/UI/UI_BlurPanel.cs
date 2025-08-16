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
    /// ������ �ð� ���� CanvasGroup alpha�� 1�� ���� (���̰�)
    /// </summary>
    public void FadeIn(float duration = 0.5f)
    {
        if (isBlured)
            return;

        isBlured = true;
        canvasGroup.DOFade(1f, duration).SetUpdate(true);
    }

    /// <summary>
    /// ������ �ð� ���� CanvasGroup alpha�� 0���� ���� (����)
    /// </summary>
    public void FadeOut(float duration = 0.2f)
    {
        if (!isBlured)
            return;

        isBlured = false;
        canvasGroup.DOFade(0f, duration).SetUpdate(true);
    }
}
