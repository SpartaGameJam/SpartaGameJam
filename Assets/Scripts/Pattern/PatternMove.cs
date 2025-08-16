using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PatternMove : MonoBehaviour
{
    private RectTransform target;
    private Vector2 curPos; // 기본 위치
    private Vector2 targetPos; // 목표 위치
    public float duration = 0.5f;    // 이동 시간

    public bool isUp = true; // 올라온 상태인지

    private void Awake()
    {
        target = GetComponent<RectTransform>();
        curPos = new Vector2(0, -475);
    }

    public void Move()
    {
        Debug.Log("클릭 체크");

        if(isUp) target.DOMove(targetPos, duration).SetEase(Ease.InOutQuad);
        else target.DOMove(curPos, duration).SetEase(Ease.InOutQuad);
    }

    public void MoveAndBack()
    {
        Debug.Log("클릭 체크");

        if (isUp)
        {
            // 내려가기 : y -200
            target.DOAnchorPos(curPos + new Vector2(0, -500f), duration).SetEase(Ease.InOutQuad);
        }
        else
        {
            // 원래 위치로
            target.DOAnchorPos(curPos, duration).SetEase(Ease.InOutQuad);
        }

        isUp = !isUp; // 상태 토글
    }
}
