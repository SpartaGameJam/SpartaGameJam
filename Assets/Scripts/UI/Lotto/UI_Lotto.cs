using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using DG.Tweening;

public class UI_Lotto : MonoBehaviour
{
    public LottoResult CurrentResult => currentResult;
    LottoResult currentResult;

    Canvas canvas;
    UI_BlurPanel blurPanel;
    Btn_ReturnToOffice officeBtn;

    [SerializeField] List<Image> images;

    public event Action OnLottoDestroyed;

    public void Init(LottoResult result, List<Sprite> sprites, Transform targetPosition)
    {
        currentResult = result;

        blurPanel = FindAnyObjectByType<UI_BlurPanel>(FindObjectsInactive.Include);
        blurPanel.FadeIn();
        officeBtn = FindAnyObjectByType<Btn_ReturnToOffice>(FindObjectsInactive.Include);
        officeBtn.Btn.interactable = false;

        for (int i = 0; i < images.Count; i++)
        {
            if (sprites != null && i < sprites.Count)
                images[i].sprite = sprites[i];
        }

        // targetPosition���� �̵� �ִϸ��̼�
        MoveToTarget(targetPosition);
    }

    RectTransform target;
    
    void Start()
    {
        if (!target)
            target = GetComponent<RectTransform>();
    }

    public void SetCamera(Camera cam)
    {
        canvas = GetComponent<Canvas>();
        canvas.sortingOrder = 5;
        canvas.worldCamera = cam;
    }

    public void PlaySpin(RectTransform RectTransform)
    {
        float rpm = 120f;
        Tween spinT;

        float duration = 60f / rpm * 4;
        spinT = RectTransform.DOBlendableLocalRotateBy(new Vector3(0, 360f * 3.5f, 0), duration, RotateMode.FastBeyond360)
                      .SetEase(Ease.OutCubic)
                      .OnComplete(() => spinT = null);
    }


    private void MoveToTarget(Transform targetPosition)
    {
        float moveDuration = 0.5f; // �̵� �ð�

        // ������ �� ���� ȸ��
        transform.rotation = Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(-50, 50f));

        // �̵��� ȸ���� ���ÿ�
        transform.DOMove(targetPosition.position, moveDuration)
                 .SetEase(Ease.OutCubic);

        transform.DORotate(Vector3.zero, moveDuration)
                 .SetEase(Ease.OutCubic);
    }

    private void OnDestroy()
    {
        if (currentResult != LottoResult.OneMore)
        {
            blurPanel.FadeOut();
            if (officeBtn != null && officeBtn.Btn != null)
            {
                officeBtn.Btn.interactable = true;
            }

            OnLottoDestroyed?.Invoke();
            OnLottoDestroyed = null;
        }
    }
}