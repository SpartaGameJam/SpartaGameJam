using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Coin : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public RectTransform ImageRect => imageRect;
    RectTransform imageRect;
    Image image;

    Vector3 _originalPosition; // 시작 위치 저장
    Canvas _canvas;            // 드래그 시 좌표 변환용
    FrontImage frontImage; // 긁을 대상 (Inspector에서 연결)

    void Start()
    {
        image = GetComponentInChildren<Image>();
        imageRect = image.GetComponent<RectTransform>();

        _canvas = GetComponentInParent<Canvas>();
        _originalPosition = imageRect.localPosition; // 시작 위치 기억
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // 드래그 시작 시 필요하면 효과 넣기
        frontImage = FindAnyObjectByType<FrontImage>();
        if (frontImage != null) 
        {
            frontImage.ResetCoinState();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_canvas == null) return;

        // Overlay Coin 위치 이동
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _canvas.transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out localPos);

        imageRect.localPosition = localPos;

        // FrontImage 긁기
        if (frontImage != null)
        {
            // 1) Overlay Coin → Screen 좌표
            Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(
                _canvas.worldCamera, imageRect.position);

            // 2) FrontImage 영역 안에 들어왔는지 확인
            if (RectTransformUtility.RectangleContainsScreenPoint(
                frontImage.scratchImage.rectTransform,
                screenPos,
                frontImage.scratchImage.canvas.worldCamera))
            {
                // 3) Screen → World 좌표 변환
                Vector3 worldPos;
                RectTransformUtility.ScreenPointToWorldPointInRectangle(
                    frontImage.scratchImage.rectTransform,
                    screenPos,
                    frontImage.scratchImage.canvas.worldCamera,
                    out worldPos);

                // 4) 긁기 실행
                frontImage.ScratchAtWorldPos(worldPos);
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // 드래그 끝나면 원래 자리로 복귀
        imageRect.localPosition = _originalPosition;
    }
}
