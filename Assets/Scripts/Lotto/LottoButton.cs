using UnityEngine;
using UnityEngine.UI;
using TMPro; // TMP �ؽ�Ʈ�� ������ �ʿ�

public class LottoButton : MonoBehaviour
{
    [SerializeField] private int price = 1000;       // �ζ� ����
    private TMP_Text buttonText;    // ��ư �ؽ�Ʈ

    private Button button;
    private LottoMaker lottoMaker;

    void Start()
    {
        lottoMaker = FindAnyObjectByType<LottoMaker>();
        buttonText = GetComponentInChildren<TMP_Text>();
        button = GetComponent<Button>();
        button.onClick.AddListener(GetLotto);

        UpdateButtonText();
    }

    void GetLotto()
    {
        // TODO : ���� �����ϴ� ���� �ʿ�
        lottoMaker.CreateLotto();

        // ��ư �ؽ�Ʈ ������Ʈ (������ ������ �� �ִٸ�)
        UpdateButtonText();
    }

    void UpdateButtonText()
    {
        if (buttonText != null)
        {
            buttonText.text = $"�߰� ����\n\u20A9{price}";
        }
    }
}
