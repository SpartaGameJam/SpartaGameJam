using UnityEngine;
using DG.Tweening;

public class DG_MoveEase : MonoBehaviour
{
    public RectTransform target;     // 움직일 UI
    public Vector2 targetPos;        // 목표 위치
    public float duration = 0.5f;    // 이동 시간

    [ContextMenu("Play Move")]
    public void PlayMove()
    {
        if (target == null) target = GetComponent<RectTransform>();

        // Ease.InOutQuad → 시작과 끝을 부드럽게
        target.DOMove(targetPos, duration).SetEase(Ease.InOutQuad);
    }

    [ContextMenu("Play Local Move")]
    public void PlayLocalMove()
    {
        if (target == null) target = GetComponent<RectTransform>();

        // anchoredPosition 기준으로 움직이고 싶을 때
        target.DOAnchorPos(targetPos, duration).SetEase(Ease.InOutCubic);
    }
}
