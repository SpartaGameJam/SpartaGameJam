using UnityEngine;
using UnityEngine.UI;

public class LottoButton : MonoBehaviour
{
    Button button;
    LottoMaker lottoMaker;

    void Start()
    {
        lottoMaker = FindAnyObjectByType<LottoMaker>();
        button = GetComponent<Button>();
        button.onClick.AddListener(GetLotto);
    }

    void GetLotto()
    {
        // TODO : ���� �ް� �ζǸ� ��� �մϴ�
        lottoMaker.CreateLotto();
    }
}
