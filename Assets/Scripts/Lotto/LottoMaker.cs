using UnityEngine;
using System.Collections.Generic;

public enum LottoResult
{
    NoMatch,
    TwoMatch,
    ThreeMatch,
    OneMore,
}

public class LottoMaker : MonoBehaviour
{
    [SerializeField] Transform spawnPoint;
    [SerializeField] Transform targetPoint;
    GameObject lottoPrefab;

    [Header("��������Ʈ")]
    [SerializeField] List<Sprite> normalIcons;
    [SerializeField] List<Sprite> onemoreIcons;

    [Header("��� Ȯ�� �ۼ�Ʈ. �ݵ�� ��� �ۼ�Ʈ���� ���� 100�� �ǵ��� �ؾ��մϴ�.")]
    [Range(0, 100)] public float noMatchPercent = 50f;
    [Range(0, 100)] public float twoMatchPercent = 30f;
    [Range(0, 100)] public float threeMatchPercent = 5f;
    [Range(0, 100)] public float oneMorePercent = 15f;

    private void Start()
    {
        lottoPrefab = Resources.Load<GameObject>("Prefabs/UI_Lotto");
    }

    public void CreateLotto()
    {
        // ��� ����
        LottoResult result = GetRandomResult();

        // ������ ����
        GameObject lottoObj = Instantiate(lottoPrefab, spawnPoint.position, Quaternion.identity);
        UI_Lotto lotto = lottoObj.GetComponent<UI_Lotto>();

        // �ʱ�ȭ
        List<Sprite> backSprites = GetSpritesForResult(result);
        lotto.Init(result, backSprites, targetPoint);
    }

    LottoResult GetRandomResult()
    {
        float r = Random.value * 100f; // 0~100 ����
        float cumulative = 0f;

        if ((cumulative += noMatchPercent) > r) return LottoResult.NoMatch;
        if ((cumulative += twoMatchPercent) > r) return LottoResult.TwoMatch;
        if ((cumulative += threeMatchPercent) > r) return LottoResult.ThreeMatch;
        if ((cumulative += oneMorePercent) > r) return LottoResult.OneMore;

        // ���� 100�� �� �� ��� ���
        return LottoResult.NoMatch;
    }

    List<Sprite> GetSpritesForResult(LottoResult result)
    {
        List<Sprite> resultSprites = new List<Sprite>();

        switch (result)
        {
            case LottoResult.NoMatch:
                {
                    // normalIcons���� �������� 3�� �ٸ� �� �̱�
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
                    // ���� ������ 2�� + �ٸ� ������ 1��
                    Sprite matchSprite = normalIcons[Random.Range(0, normalIcons.Count)];
                    resultSprites.Add(matchSprite);
                    resultSprites.Add(matchSprite);

                    // �ٸ� ������ 1��
                    List<Sprite> pool = new List<Sprite>(normalIcons);
                    pool.Remove(matchSprite);
                    if (pool.Count > 0)
                    {
                        Sprite differentSprite = pool[Random.Range(0, pool.Count)];
                        resultSprites.Add(differentSprite);
                    }

                    // ���� ����
                    for (int i = 0; i < resultSprites.Count; i++)
                    {
                        int rnd = Random.Range(0, resultSprites.Count);
                        (resultSprites[i], resultSprites[rnd]) = (resultSprites[rnd], resultSprites[i]);
                    }
                }
                break;

            case LottoResult.ThreeMatch:
                {
                    // ���� ���� ������
                    Sprite matchSprite = normalIcons[Random.Range(0, normalIcons.Count)];
                    resultSprites.Add(matchSprite);
                    resultSprites.Add(matchSprite);
                    resultSprites.Add(matchSprite);
                }
                break;

            case LottoResult.OneMore:
                return onemoreIcons;
        }

        foreach (Sprite matchSprite in resultSprites)
        {
            Debug.Log($"{matchSprite.name}");
        }

        return resultSprites;
    }

}
