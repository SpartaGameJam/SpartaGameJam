using UnityEngine;
using System.Collections.Generic;

public enum LottoResult
{
    NoMatch,
    TwoMatch,
    ThreeCarrot,
    ThreeRabbit,
    ThreeRadish,
    ThreeScoop,
    OneMore,
}

public class LottoMaker : MonoBehaviour
{
    [SerializeField] Transform spawnPoint;
    [SerializeField] Transform targetPoint;
    GameObject lottoPrefab;

    [Header("스프라이트")]
    [SerializeField] List<Sprite> normalIcons;
    [SerializeField] List<Sprite> onemoreIcons;

    [Header("결과 확률 퍼센트. 반드시 합계가 100이 되도록 해야합니다.")]
    [Range(0, 100)] public float noMatchPercent;
    [Range(0, 100)] public float twoMatchPercent;
    [Range(0, 100)] public float threeCarrotPercent;
    [Range(0, 100)] public float threeRabbitPercent;
    [Range(0, 100)] public float threeRadishPercent;
    [Range(0, 100)] public float threeScoopPercent;
    [Range(0, 100)] public float oneMorePercent;

    float baseNoMatchPercent;
    float baseOneMorePercent;

    private void Awake()
    {
        baseNoMatchPercent = noMatchPercent;
        baseOneMorePercent = oneMorePercent;
    }

    private void OnEnable()
    {
        EventManager.Instance.AddEvent(EEventType.Upgraded, ApplyExtraChance);
        ApplyExtraChance();
    }

    private void OnDisable()
    {
        EventManager.Instance.RemoveEvent(EEventType.Upgraded, ApplyExtraChance);
    }
    private void Start()
    {
        lottoPrefab = Resources.Load<GameObject>("Prefabs/UI_Lotto");
    }

    void ApplyExtraChance()
    {
        float extra = GameManager.Instance.GetStatValue(UpgradeType.ExtraChanceRate);
        oneMorePercent = baseOneMorePercent + extra;
        noMatchPercent = Mathf.Max(0f, baseNoMatchPercent - extra);
    }


    public UI_Lotto CreateLotto()
    {
        // 결과 결정
        LottoResult result = GetRandomResult();

        // 프리팹 생성
        GameObject lottoObj = Instantiate(lottoPrefab, spawnPoint.position, Quaternion.identity);
        UI_Lotto lotto = lottoObj.GetComponent<UI_Lotto>();

        // 초기화
        List<Sprite> backSprites = GetSpritesForResult(result);
        lotto.Init(result, backSprites, targetPoint);

        return lotto;
    }

    LottoResult GetRandomResult()
    {
        float r = Random.value * 100f; // 0~100 사이
        float cumulative = 0f;

        if ((cumulative += noMatchPercent) > r) return LottoResult.NoMatch;
        if ((cumulative += twoMatchPercent) > r) return LottoResult.TwoMatch;
        if ((cumulative += threeCarrotPercent) > r) return LottoResult.ThreeCarrot;
        if ((cumulative += threeRabbitPercent) > r) return LottoResult.ThreeRabbit;
        if ((cumulative += threeRadishPercent) > r) return LottoResult.ThreeRadish;
        if ((cumulative += threeScoopPercent) > r) return LottoResult.ThreeScoop;
        if ((cumulative += oneMorePercent) > r) return LottoResult.OneMore;

        // 합이 100이 안 될 경우 대비
        return LottoResult.NoMatch;
    }

    List<Sprite> GetSpritesForResult(LottoResult result)
    {
        List<Sprite> resultSprites = new List<Sprite>();

        switch (result)
        {
            case LottoResult.NoMatch:
                {
                    // normalIcons 중 랜덤 3개 뽑기 (중복X)
                    List<Sprite> pool = new List<Sprite>(normalIcons);

                    for (int i = 0; i < 3 && pool.Count > 0; i++)
                    {
                        int idx = Random.Range(0, pool.Count);
                        resultSprites.Add(pool[idx]);
                        pool.RemoveAt(idx);
                    }
                }
                break;

            case LottoResult.TwoMatch:
                {
                    // 같은 아이콘 2개
                    List<Sprite> pool = new List<Sprite>(normalIcons);

                    Sprite matchSprite = pool[Random.Range(0, pool.Count)];
                    resultSprites.Add(matchSprite);
                    resultSprites.Add(matchSprite);

                    // 다른 아이콘 1개
                    pool.Remove(matchSprite);
                    if (pool.Count > 0)
                    {
                        Sprite differentSprite = pool[Random.Range(0, pool.Count)];
                        resultSprites.Add(differentSprite);
                    }

                    // 랜덤 섞기
                    for (int i = 0; i < resultSprites.Count; i++)
                    {
                        int rnd = Random.Range(0, resultSprites.Count);
                        (resultSprites[i], resultSprites[rnd]) = (resultSprites[rnd], resultSprites[i]);
                    }
                }
                break;
            case LottoResult.ThreeCarrot:
                {
                    Sprite carrot = normalIcons[0];
                    resultSprites.Add(carrot);
                    resultSprites.Add(carrot);
                    resultSprites.Add(carrot);
                }
                break;
            case LottoResult.ThreeRabbit:
                {
                    Sprite carrot = normalIcons[1];
                    resultSprites.Add(carrot);
                    resultSprites.Add(carrot);
                    resultSprites.Add(carrot);
                }
                break;
            case LottoResult.ThreeRadish:
                {
                    Sprite carrot = normalIcons[2];
                    resultSprites.Add(carrot);
                    resultSprites.Add(carrot);
                    resultSprites.Add(carrot);
                }
                break;
            case LottoResult.ThreeScoop:
                {
                    Sprite carrot = normalIcons[3];
                    resultSprites.Add(carrot);
                    resultSprites.Add(carrot);
                    resultSprites.Add(carrot);
                }
                break;
            case LottoResult.OneMore:
                // oneMore는 지정된 아이콘 사용
                return new List<Sprite>(onemoreIcons);
        }

        return resultSprites;
    }
}
