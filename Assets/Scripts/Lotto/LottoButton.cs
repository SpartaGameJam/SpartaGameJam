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
        // TODO : 돈을 받고 로또를 사야 합니다
        lottoMaker.CreateLotto();
    }
}
