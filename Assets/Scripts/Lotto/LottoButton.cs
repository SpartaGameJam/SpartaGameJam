using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LottoButton : MonoBehaviour
{
    [SerializeField] private int price = 1000;       // 로또 가격
    private TMP_Text buttonText;    // 버튼 텍스트

    private Button button;
    private LottoMaker lottoMaker;

    void Start()
    {
        lottoMaker = FindAnyObjectByType<LottoMaker>();
        buttonText = GetComponentInChildren<TMP_Text>();
        button = GetComponent<Button>();
        button.onClick.AddListener(GetLotto);
        button.onClick.AddListener(CloseShopScene);

        UpdateButtonText();
    }

    void CloseShopScene()
    {
        UIManager.Instance.FindUIScene<UI_Shop>().gameObject.SetActive(false);
    }

    void GetLotto()
    {
        // TODO : 돈을 차감하는 로직 필요
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
            if (lotto.CurrentResult != LottoResult.OneMore)
                button.interactable = true;
        };
    }


    void UpdateButtonText()
    {
        if (buttonText != null)
        {
            buttonText.text = $"추가 구매\n\u20A9{price}";
        }
    }
}
