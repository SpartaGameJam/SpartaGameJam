using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

public class UI_Lotto : MonoBehaviour
{
    Canvas canvas;
    LottoResult currentResult;

    [SerializeField] List<Image> images;

    void Start()
    {
        canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
    }

    public void Init(LottoResult result, List<Sprite> sprites, Transform targetPosition)
    {
        currentResult = result;

        for (int i = 0; i < images.Count; i++)
        {
            if (sprites != null && i < sprites.Count)
                images[i].sprite = sprites[i];
        }

        // targetPosition으로 이동 애니메이션
        MoveToTarget(targetPosition);
    }

    private void MoveToTarget(Transform targetPosition)
    {
        float moveDuration = 0.5f; // 이동 시간

        // 시작할 때 랜덤 회전
        transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(-50, 50f));

        // 이동과 회전을 동시에
        transform.DOMove(targetPosition.position, moveDuration)
                 .SetEase(Ease.OutCubic);

        transform.DORotate(Vector3.zero, moveDuration)
                 .SetEase(Ease.OutCubic);
    }

}
