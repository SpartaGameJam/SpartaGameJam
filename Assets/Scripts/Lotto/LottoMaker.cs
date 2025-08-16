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

    [Header("��������Ʈ")]
    [SerializeField] List<Sprite> normalIcons;
    [SerializeField] List<Sprite> onemoreIcons;
    [SerializeField] Sprite feverIcon;

    [Header("��� Ȯ�� �ۼ�Ʈ. �ݵ�� �հ谡 100�� �ǵ��� �ؾ��մϴ�.")]
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
        if ((cumulative += feverPercent) > r) return LottoResult.Fever;

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
                    // normalIcons + feverIcon �����ؼ� ���� 3�� �ٸ� �� �̱�
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
                    // ���� ������ 2�� (feverIcon ���� ����)
                    List<Sprite> pool = new List<Sprite>(normalIcons);
                    if (feverIcon != null) pool.Add(feverIcon);

                    Sprite matchSprite = pool[Random.Range(0, pool.Count)];
                    resultSprites.Add(matchSprite);
                    resultSprites.Add(matchSprite);

                    // �ٸ� ������ 1��
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
                    // ���� ���� normal ������ (feverIcon�� ����)
                    Sprite matchSprite = normalIcons[Random.Range(0, normalIcons.Count)];
                    resultSprites.Add(matchSprite);
                    resultSprites.Add(matchSprite);
                    resultSprites.Add(matchSprite);
                }
                break;

            case LottoResult.OneMore:
                // oneMore�� ������ ������ ���
                return new List<Sprite>(onemoreIcons);

            case LottoResult.Fever:
                {
                    // ���� fever ������
                    resultSprites.Add(feverIcon);
                    resultSprites.Add(feverIcon);
                    resultSprites.Add(feverIcon);
                }
                break;
        }

        return resultSprites;
    }
}
