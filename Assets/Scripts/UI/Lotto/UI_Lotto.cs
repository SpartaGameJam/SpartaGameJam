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

    public void SetCamera(Camera cam)
    {
        canvas = GetComponent<Canvas>();
        canvas.sortingOrder = 5;
        canvas.worldCamera = cam;
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
            officeBtn.Btn.interactable = true;

            OnLottoDestroyed?.Invoke();
            OnLottoDestroyed = null;
        }
    }
}