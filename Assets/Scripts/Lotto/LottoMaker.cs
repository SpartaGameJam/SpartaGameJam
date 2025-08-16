using UnityEngine;
using System.Collections.Generic;

public enum LottoResult
{
    NoMatch,
    TwoMatch,
    ThreeMatch,
    OneMore,
    Fever,
}

public class LottoMaker : MonoBehaviour
{
    [SerializeField] Transform spawnPoint;
    [SerializeField] Transform targetPoint;
    GameObject lottoPrefab;

    [Header("스프라이트")]
    [SerializeField] List<Sprite> normalIcons;
    [SerializeField] List<Sprite> onemoreIcons;
    [SerializeField] Sprite feverIcon;

    [Header("결과 확률 퍼센트. 반드시 합계가 100이 되도록 해야합니다.")]
    [Range(0, 100)] public float noMatchPercent = 40f;
    [Range(0, 100)] public float twoMatchPercent = 25f;
    [Range(0, 100)] public float threeMatchPercent = 5f;
    [Range(0, 100)] public float oneMorePercent = 20f;
    [Range(0, 100)] public float feverPercent = 10f;

    private void Start()
    {
        lottoPrefab = Resources.Load<GameObject>("Prefabs/UI_Lotto");
    }

    public void CreateLotto()
    {
        // 결과 결정
        LottoResult result = GetRandomResult();

        // 프리팹 생성
        GameObject lottoObj = Instantiate(lottoPrefab, spawnPoint.position, Quaternion.identity);
        UI_Lotto lotto = lottoObj.GetComponent<UI_Lotto>();

        // 초기화
        List<Sprite> backSprites = GetSpritesForResult(result);
        lotto.Init(result, backSprites, targetPoint);
    }

    LottoResult GetRandomResult()
    {
        float r = Random.value * 100f; // 0~100 사이
        float cumulative = 0f;

        if ((cumulative += noMatchPercent) > r) return LottoResult.NoMatch;
        if ((cumulative += twoMatchPercent) > r) return LottoResult.TwoMatch;
        if ((cumulative += threeMatchPercent) > r) return LottoResult.ThreeMatch;
        if ((cumulative += oneMorePercent) > r) return LottoResult.OneMore;
        if ((cumulative += feverPercent) > r) return LottoResult.Fever;

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
                    // normalIcons + feverIcon 포함해서 랜덤 3개 다른 거 뽑기
                    List<Sprite> pool = new List<Sprite>(normalIcons);
                    if (feverIcon != null) pool.Add(feverIcon);

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
                    // 같은 아이콘 2개 (feverIcon 포함 가능)
                    List<Sprite> pool = new List<Sprite>(normalIcons);
                    if (feverIcon != null) pool.Add(feverIcon);

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
                    // 전부 같은 normal 아이콘 (feverIcon은 제외)
                    Sprite matchSprite = normalIcons[Random.Range(0, normalIcons.Count)];
                    resultSprites.Add(matchSprite);
                    resultSprites.Add(matchSprite);
                    resultSprites.Add(matchSprite);
                }
                break;

            case LottoResult.OneMore:
                // oneMore는 지정된 아이콘 사용
                return new List<Sprite>(onemoreIcons);

            case LottoResult.Fever:
                {
                    // 전부 fever 아이콘
                    resultSprites.Add(feverIcon);
                    resultSprites.Add(feverIcon);
                    resultSprites.Add(feverIcon);
                }
                break;
        }

        return resultSprites;
    }
}
