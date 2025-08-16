using TMPro;
using UnityEngine;

public class EndingResult : MonoBehaviour
{
    public TextMeshProUGUI lottoText;
    public TextMeshProUGUI failText;
    public TextMeshProUGUI fiverText;
    public TextMeshProUGUI workText;
    public TextMeshProUGUI upgradeText;
    public TextMeshProUGUI useMoneyText;

    private void Start()
    {
        lottoText.text = "복권 긁은 횟수: " + GameManager.Instance.useLottoCount.ToString();
        failText.text = "꽝 나온 횟수: " + GameManager.Instance.failCount.ToString();
        fiverText.text = "피버 횟수: " + GameManager.Instance.fiverCount.ToString();
        workText.text = "처리한 업무 수: " + GameManager.Instance.completeWork.ToString();
        upgradeText.text = "업그레이드 수: " + GameManager.Instance.upgradeCount.ToString();
        useMoneyText.text = "사용한 돈: " + GameManager.Instance.useMoney.ToString();
    }
}
