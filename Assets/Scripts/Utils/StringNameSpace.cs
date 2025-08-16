using UnityEngine;

public class StringNameSpace : MonoBehaviour
{
    public const string CurrencyGain = "재화 획득량";
    public const string ExtraChanceRate = "한번더! 확률";
    public const string FeverTriggerRate = "피버 누적량";
    public const string LotteryWinRate = "복권 당첨률";
    public const string LotteryDiscountRate = "복권 할인률";

    public const string NPCDialogue00 = "오늘도 복권 사러 오셨나요?";
    public const string NPCDialogue01 = "복권은 계산대 앞에서\n바로 뽑으실 수 있어요.";
    public const string NPCDialogue02 = "오늘 운세가\n좋으실지도 몰라요!";
    public const string NPCDialogue03 = "복권은 몇 장 필요하세요?";

    public static readonly string[] dialogues =
    {
        StringNameSpace.NPCDialogue00,
        StringNameSpace.NPCDialogue01,
        StringNameSpace.NPCDialogue02,
        StringNameSpace.NPCDialogue03
    };
    
    public string GetRandomDialogue()
    {
        int index = Random.Range(0, dialogues.Length);
        return dialogues[index];
    }
}
