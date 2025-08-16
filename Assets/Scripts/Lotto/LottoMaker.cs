using UnityEngine;
using System.Collections.Generic;

public enum LottoResult
{
    NoMatch,
    TwoMatch,
    ThreeMatch,
    ThreeCarrot,
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
    [Range(0, 100)] public float threeMatchPercent;
    [Range(0, 100)] public float threeCarrotPercent;
    [Range(0, 100)] public float oneMorePercent;

    private void Start()
    {
        lottoPrefab = Resources.Load<GameObject>("Prefabs/UI_Lotto");
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
        if ((cumulative += threeMatchPercent) > r) return LottoResult.ThreeMatch;
        if ((cumulative += threeCarrotPercent) > r) return LottoResult.ThreeCarrot; // 🥕 추가
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

            case LottoResult.ThreeMatch:
                {
                    // 전부 같은 normal 아이콘
                    Sprite matchSprite = normalIcons[Random.Range(0, normalIcons.Count)];
                    resultSprites.Add(matchSprite);
                    resultSprites.Add(matchSprite);
                    resultSprites.Add(matchSprite);
                }
                break;

            case LottoResult.ThreeCarrot:
                {
                    // 전부 당근 아이콘
                    Sprite carrot = normalIcons[0];
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
