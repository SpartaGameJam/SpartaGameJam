using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;   // DOTween 네임스페이스 추가

public class LottoButton : MonoBehaviour
{
    [SerializeField] private int price = 1000;       // 로또 가격
    private TMP_Text buttonText;    // 버튼 텍스트

    private Button button;
    private LottoMaker lottoMaker;
    private Image buttonImage;      // 버튼 배경 이미지

    private Color _defaultColor;    // 원래 색 저장용

    void Start()
    {
        lottoMaker = FindAnyObjectByType<LottoMaker>();
        buttonText = GetComponentInChildren<TMP_Text>();
        button = GetComponent<Button>();
        buttonImage = GetComponent<Image>();

        _defaultColor = buttonImage.color;

        button.onClick.AddListener(GetLotto);

        UpdateButtonText();
    }

    void OnEnable()
    {
        EventManager.Instance.AddEvent(EEventType.Upgraded, UpdateButtonText);
        EventManager.Instance.AddEvent(EEventType.MoneyChanged, UpdateButtonText);
        UpdateButtonText();
    }

    void OnDisable()
    {
        EventManager.Instance.RemoveEvent(EEventType.Upgraded, UpdateButtonText);
        EventManager.Instance.RemoveEvent(EEventType.MoneyChanged, UpdateButtonText);
    }


    int GetCurrentPrice()
    {
        float discountRate = GameManager.Instance.GetStatValue(UpgradeType.LotteryDiscountRate);
        float multiplier = Mathf.Clamp01(1f - discountRate / 100f);
        return Mathf.RoundToInt(price * multiplier);
    }

    void GetLotto()
    {
        int currentPrice = GetCurrentPrice();
        if (GameManager.Instance.Money < currentPrice)
        {
            // 애니메이션: 빨갛게 + 좌우 흔들림
            buttonImage.DOColor(Color.red, 0.2f)
                       .OnComplete(() => buttonImage.DOColor(_defaultColor, 0.5f));

            transform.DOShakePosition(0.5f, new Vector3(10f, 0f, 0f), 20, 90, false, true);

            return;
        }
        
        SoundManager.instance.PlaySFX(SFXSound.BuyLotto);

        GameManager.Instance.UpdateLotto();
        GameManager.Instance.UpdateMoney(-currentPrice);
        EventManager.Instance.TriggerEvent(EEventType.MoneyChanged);

        UI_Lotto lotto = lottoMaker.CreateLotto();
        HandleLotto(lotto);

        // 버튼 텍스트 업데이트 (가격이 변동할 수 있다면)
        UpdateButtonText();
    }

    public void HandleLotto(UI_Lotto lotto)
    {
        if (lotto == null) return;

        button.interactable = false;
        lotto.OnLottoDestroyed += () =>
        {
            if (lotto.CurrentResult != LottoResult.OneMore && button != null)
                button.interactable = true;
        };
    }

    void UpdateButtonText()
    {
        if (buttonText != null)
        {
            int currentPrice = GetCurrentPrice();
            buttonText.text = $"복권 구매 \u20A9{currentPrice}";
        }
    }
}
