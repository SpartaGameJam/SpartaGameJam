using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Globalization;
using System.Collections.Generic;

public class UI_TextAnimation : MonoBehaviour
{
    CanvasGroup canvasGroup;
    RectTransform rootRect;
    TMP_Text[] texts;

    [Header("Animation Settings")]
    [SerializeField] float moveY = 80f;          // 떠오르는 Y 거리
    [SerializeField] float duration = 1f;        // 전체 애니메이션 시간
    [SerializeField] float punchScale = 0.3f;    // 튀어오르는 효과 크기

    Vector3 startPos;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rootRect = transform.GetChild(0).GetComponent<RectTransform>();
        texts = rootRect.GetComponentsInChildren<TMP_Text>();

        startPos = rootRect.localPosition;
        canvasGroup.alpha = 0f; // 시작은 안보이게
    }

    public void Play(int amount)
    {
        // 숫자를 3자리 콤마 + ₩ 기호로 표시
        string formatted = string.Format(CultureInfo.InvariantCulture, "+{0:N0}\u20A9", amount);
        foreach (TMP_Text text in texts)
            text.text = formatted;

        // 초기화
        rootRect.localPosition = startPos;
        rootRect.localScale = Vector3.one;
        canvasGroup.alpha = 0f;

        // 애니메이션 시퀀스
        Sequence seq = DOTween.Sequence();

        // 알파 페이드 인 + 위로 이동
        seq.Append(canvasGroup.DOFade(1f, 0.2f));
        seq.Join(rootRect.DOLocalMoveY(startPos.y + moveY, duration));

        // 성취감: 살짝 커졌다가 원래대로
        seq.Join(rootRect.DOPunchScale(Vector3.one * punchScale, 0.4f, 6, 0.7f));

        // 끝날 때 서서히 사라지기
        seq.Append(canvasGroup.DOFade(0f, 0.3f));

        seq.OnComplete(() =>
        {
            // 끝나면 다시 초기 상태로
            rootRect.localPosition = startPos;
            rootRect.localScale = Vector3.one;
            canvasGroup.alpha = 0f;
        });
    }
}
