using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Coin : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public RectTransform ImageRect => imageRect;
    RectTransform imageRect;
    Image image;

    Vector3 _originalPosition; // ���� ��ġ ����
    Canvas _canvas;            // �巡�� �� ��ǥ ��ȯ��
    FrontImage frontImage; // ���� ��� (Inspector���� ����)

    void Start()
    {
        image = GetComponentInChildren<Image>();
        imageRect = image.GetComponent<RectTransform>();

        _canvas = GetComponentInParent<Canvas>();
        _originalPosition = imageRect.localPosition; // ���� ��ġ ���
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // �巡�� ���� �� �ʿ��ϸ� ȿ�� �ֱ�
        frontImage = FindAnyObjectByType<FrontImage>();
        if (frontImage != null) 
        {
            frontImage.ResetCoinState();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_canvas == null) return;

        // Overlay Coin ��ġ �̵�
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _canvas.transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out localPos);

        imageRect.localPosition = localPos;

        // FrontImage �ܱ�
        if (frontImage != null)
        {
            // 1) Overlay Coin �� Screen ��ǥ
            Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(
                _canvas.worldCamera, imageRect.position);

            // 2) FrontImage ���� �ȿ� ���Դ��� Ȯ��
            if (RectTransformUtility.RectangleContainsScreenPoint(
                frontImage.scratchImage.rectTransform,
                screenPos,
                frontImage.scratchImage.canvas.worldCamera))
            {
                // 3) Screen �� World ��ǥ ��ȯ
                Vector3 worldPos;
                RectTransformUtility.ScreenPointToWorldPointInRectangle(
                    frontImage.scratchImage.rectTransform,
                    screenPos,
                    frontImage.scratchImage.canvas.worldCamera,
                    out worldPos);

                // 4) �ܱ� ����
                frontImage.ScratchAtWorldPos(worldPos);
            }
        }
        
        SoundManager.instance.PlaySFX(SFXSound.GetGold);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // �巡�� ������ ���� �ڸ��� ����
        imageRect.localPosition = _originalPosition;
    }
}
