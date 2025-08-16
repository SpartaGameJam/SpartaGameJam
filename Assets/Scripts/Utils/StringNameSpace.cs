using UnityEngine;

public class StringNameSpace : MonoBehaviour
{
    public const string CurrencyGain = "CurrencyGain";
    public const string ExtraChanceRate = "ExtraChanceRate";
    public const string FeverTriggerRate = "FeverTriggerRate";
    public const string LotteryWinRate = "LotteryWinRate";
    public const string LotteryDiscountRate = "LotteryDiscountRate";

    public const string NPCDialogue00 = "Scratch your ticket before you head out!";
    public const string NPCDialogue01 = "Give your ticket a scratch before leaving!";
    public const string NPCDialogue02 = "Don't forget to scratch your ticket before you go!";
    public const string NPCDialogue03 = "welcome...";

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
