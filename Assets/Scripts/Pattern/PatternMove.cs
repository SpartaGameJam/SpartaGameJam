using DG.Tweening;
using UnityEngine;

public class PatternMove : MonoBehaviour
{
    public RectTransform target;
    public Vector2 curPos; // 기본 위치
    public Vector2 targetPos; // 목표 위치
    public float duration = 0.5f;    // 이동 시간

    public bool isUp = false;

    private void Awake()
    {
        target = GetComponent<RectTransform>();

        curPos = transform.localPosition;
    }

    public void Move()
    {
        if(isUp) target.DOMove(targetPos, duration).SetEase(Ease.InOutQuad);
        else target.DOMove(curPos, duration).SetEase(Ease.InOutQuad);
    }
}
