using UnityEngine;
using DG.Tweening;

public class DG_BounceMove : MonoBehaviour
{
    public RectTransform target;     // 움직일 UI
    public Vector2 targetPos;        // 목표 위치
    public float duration = 0.6f;    // 이동 시간
    public float overshoot = 1.2f;   // 튕김 강도 (1~2 정도)

    [ContextMenu("Play Bounce Move")]
    public void PlayBounceMove()
    {
        if (target == null) target = GetComponent<RectTransform>();

        // Ease.OutBack : 목표 위치를 살짝 넘었다가 튕기며 멈춤
        target.DOAnchorPos(targetPos, duration)
              .SetEase(Ease.OutBack, overshoot);
    }
}