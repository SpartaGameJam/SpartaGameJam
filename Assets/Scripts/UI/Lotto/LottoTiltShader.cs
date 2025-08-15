using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class LottoTiltShader : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public float maxTilt = 10f;           // �Է� �� �ִ� ���� ����
    public float tiltLerpSpeed = 10f;     // ȸ�� ���� �ӵ�

    [Header("Idle Tilt Settings")]
    public float idleTiltAmount = 3f;     // ��� ���¿��� �ִ� ����
    public float idleTiltSpeed = 1f;      // ��� ȸ�� �ӵ�

    [Header("Shake Settings")]
    public Vector3 shakeAngle = new Vector3(5f, -5f, 0f); // ���� ȸ�� ����
    public float shakeStepTime = 0.05f;                   // �� �� �����̴� �� �ɸ��� �ð�
    public int shakeLoops = 6;                            // �ݺ� Ƚ��(�պ� ����)

    Vector2 lastPos;
    Vector3 targetRotation;
    bool isInteracting = false;
    float idleTime;

    public void OnPointerDown(PointerEventData eventData)
    {
        // Ŭ�� �� �θ���
        DoShake();

        isInteracting = true;
        lastPos = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 delta = eventData.position - lastPos;
        lastPos = eventData.position;

        float tiltX = Mathf.Clamp(-delta.y * 0.1f, -maxTilt, maxTilt);
        float tiltY = Mathf.Clamp(delta.x * 0.1f, -maxTilt, maxTilt);
        targetRotation = new Vector3(tiltX, tiltY, 0);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isInteracting = false;
        targetRotation = Vector3.zero;
    }

    void Update()
    {
        if (!isInteracting)
        {
            // ��� ���� ȸ��
            idleTime += Time.deltaTime * idleTiltSpeed;
            float tiltX = Mathf.Sin(idleTime) * idleTiltAmount;
            float tiltY = Mathf.Cos(idleTime * 0.8f) * idleTiltAmount;
            targetRotation = new Vector3(tiltX, tiltY, 0);
        }

        // �巡�׳� ���̵� ƿƮ ����
        transform.localRotation = Quaternion.Lerp(
            transform.localRotation,
            Quaternion.Euler(targetRotation),
            Time.deltaTime * tiltLerpSpeed
        );
    }

    public void DoShake()
    {
        // ���� ���� ���� Ʈ�� �ߴ�
        transform.DOKill();

        // �ʱ�ȭ �� ���� ����
        transform.DOLocalRotate(Vector3.zero, 0f);
        transform.DOLocalRotate(shakeAngle, shakeStepTime)
                 .SetLoops(shakeLoops, LoopType.Yoyo)
                 .SetEase(Ease.InOutSine);
    }
}
