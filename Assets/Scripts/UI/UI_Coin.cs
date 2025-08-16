using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Coin : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public RectTransform ImageRect => imageRect;
    RectTransform imageRect;
    Image image;
    ParticleSystem coinParticle;

    Vector3 _originalPosition;
    Canvas _canvas;
    FrontImage frontImage;

    void Start()
    {
        image = GetComponentInChildren<Image>();
        imageRect = image.GetComponent<RectTransform>();
        coinParticle = GetComponentInChildren<ParticleSystem>();

        _canvas = GetComponentInParent<Canvas>();
        _originalPosition = imageRect.localPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        frontImage = FindAnyObjectByType<FrontImage>();
        if (frontImage != null)
        {
            frontImage.ResetCoinState();
        }

        if (coinParticle != null)
        {
            coinParticle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_canvas == null) return;

        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _canvas.transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out localPos);

        imageRect.localPosition = localPos;

        if (frontImage != null)
        {
            Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(
                _canvas.worldCamera, imageRect.position);

            if (RectTransformUtility.RectangleContainsScreenPoint(
                frontImage.scratchImage.rectTransform,
                screenPos,
                frontImage.scratchImage.canvas.worldCamera))
            {
                Vector3 worldPos;
                RectTransformUtility.ScreenPointToWorldPointInRectangle(
                    frontImage.scratchImage.rectTransform,
                    screenPos,
                    frontImage.scratchImage.canvas.worldCamera,
                    out worldPos);

                frontImage.ScratchAtWorldPos(worldPos);
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        imageRect.localPosition = _originalPosition;

        if (coinParticle != null)
        {
            coinParticle.Play();
        }
    }
}