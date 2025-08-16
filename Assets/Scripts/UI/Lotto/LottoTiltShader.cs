using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class LottoTiltShader : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public float maxTilt = 10f;           // 입력 시 최대 기울기 각도
    public float tiltLerpSpeed = 10f;     // 회전 보간 속도

    [Header("Idle Tilt Settings")]
    public float idleTiltAmount = 3f;     // 대기 상태에서 최대 기울기
    public float idleTiltSpeed = 1f;      // 대기 회전 속도

    [Header("Shake Settings")]
    public Vector3 shakeAngle = new Vector3(5f, -5f, 0f); // 진동 회전 각도
    public float shakeStepTime = 0.05f;                   // 한 번 움직이는 데 걸리는 시간
    public int shakeLoops = 6;                            // 반복 횟수(왕복 포함)

    Vector2 lastPos;
    Vector3 targetRotation;
    bool isInteracting = false;
    float idleTime;

    bool useExternalTilt = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        // 클릭 시 부르르
        DoShake();

        isInteracting = true;
        lastPos = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (useExternalTilt) return;

        Vector2 delta = eventData.position - lastPos;
        lastPos = eventData.position;

        float tiltX = Mathf.Clamp(-delta.y * 0.1f, -maxTilt, maxTilt);
        float tiltY = Mathf.Clamp(delta.x * 0.1f, -maxTilt, maxTilt);
        targetRotation = new Vector3(tiltX, tiltY, 0);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (useExternalTilt) return;

        isInteracting = false;
        targetRotation = Vector3.zero;
    }

    void Update()
    {
        if (!isInteracting && !useExternalTilt)
        {
            // 대기 상태 회전
            idleTime += Time.deltaTime * idleTiltSpeed;
            float tiltX = Mathf.Sin(idleTime) * idleTiltAmount;
            float tiltY = Mathf.Cos(idleTime * 0.8f) * idleTiltAmount;
            targetRotation = new Vector3(tiltX, tiltY, 0);
        }

        // 드래그나 아이들 틸트 적용
        transform.localRotation = Quaternion.Lerp(
            transform.localRotation,
            Quaternion.Euler(targetRotation),
            Time.deltaTime * tiltLerpSpeed
        );
    }

    public void DoShake()
    {
        // 현재 실행 중인 트윈 중단
        transform.DOKill();

        // 초기화 후 진동 시작
        transform.DOLocalRotate(Vector3.zero, 0f);
        transform.DOLocalRotate(shakeAngle, shakeStepTime)
                 .SetLoops(shakeLoops, LoopType.Yoyo)
                 .SetEase(Ease.InOutSine);
    }

    /// <summary>
    /// 외부에서 기울임 값을 전달하여 회전을 제어합니다.
    /// </summary>
    /// <param name="tilt">x,y 값이 각각 X,Y 회전 각도</param>
    public void SetExternalTilt(Vector2 tilt)
    {
        useExternalTilt = true;
        isInteracting = true;
        targetRotation = new Vector3(
            Mathf.Clamp(tilt.x, -maxTilt, maxTilt),
            Mathf.Clamp(tilt.y, -maxTilt, maxTilt),
            0f);
    }

    /// <summary>
    /// 외부 기울임 제어를 해제합니다.
    /// </summary>
    public void ReleaseExternalTilt()
    {
        useExternalTilt = false;
        isInteracting = false;
        targetRotation = Vector3.zero;
    }
}
