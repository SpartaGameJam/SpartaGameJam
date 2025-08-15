using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonitorPattern : MonoBehaviour
{
    public List<GameObject> pointers;
    public List<int> patternSequence; // 포인트 배열 순서
    public List<GameObject> lines;

    public GameObject linePrefab;
    public Canvas canvas; // 하이어라키창 스폿

    private int gridSize = 3;

    //public Vector3 offsetPos = new Vector3(0, 450f, 0); // Monitor의 Rect Transform 값
    public float yOffset = 350f;

    public bool CheckPattern(List<int> pointlist)
    {
        var setA = new HashSet<int>(patternSequence);
        var setB = new HashSet<int>(pointlist);

        return setA.SetEquals(setB);
    }

    private void Start()
    {
        pointers = new List<GameObject>();
        patternSequence = new List<int>();
        lines = new List<GameObject>();

        for (int i = 0; i < transform.childCount; i++)
        {
            pointers.Add(transform.GetChild(i).gameObject);
        }

        //SetPatternSquence();

        StartCoroutine(test());
    }

    private IEnumerator test()
    {
        yield return null;
        SetPatternSquence();
    }

    public void SetPatternSquence()
    {
        Release();

        int cnt = Random.Range(2, pointers.Count); // 선은 점 2개부터 시작이니
        int check = 0; // 현재 들어온 값을 카운트

        patternSequence.Clear();

        patternSequence.Add(Random.Range(0, pointers.Count)); // 첫 인자값 추가
        check++;

        while(check < cnt)
        {
            int num = Random.Range(0, pointers.Count);
            if (patternSequence.Contains(num)) continue;

            int mid = GetMiddleId(num, patternSequence[patternSequence.Count - 1]); // 현재 뽑은값과 이전 값의 사이 노드 체크

            if(mid != -1 && !patternSequence.Contains(mid)) // 만약 사잇값이 존재하고 이미 추가가 된 상태가 아니라면
            {
                if(check+1 == cnt) // 남은 자리수가 1개라면
                {
                    continue; // 다시 뽑기
                }

                patternSequence.Add(mid); // 사이 노드 추가
                check++;
            }

            check++;
            patternSequence.Add(num);
        }

        SetLine();
    }

    private void SetLine()
    {
        RectTransform lineRect;

        for(int i=0; i<patternSequence.Count-1; i++) // 마지막 라인은 생성 제한
        {
            var line = GameObject.Instantiate(linePrefab, canvas.transform);
            line.name = "Grade " + line.name;
            line.transform.localPosition = pointers[patternSequence[i]].transform.localPosition;
            lineRect = line.GetComponent<RectTransform>();

            lineRect.sizeDelta = new Vector2(lineRect.sizeDelta.x / 2, Vector3.
                Distance(pointers[patternSequence[i+1]].transform.localPosition, pointers[patternSequence[i]].transform.localPosition));

            lineRect.rotation = Quaternion.FromToRotation(
                Vector3.up, (pointers[patternSequence[i+1]].transform.localPosition - pointers[patternSequence[i]].transform.localPosition)
                .normalized);

            lineRect.anchoredPosition += new Vector2(0,yOffset);


            lines.Add(line);
        }
    }

    private void Release() // 패턴 제거
    {
        foreach (var line in lines)
        {
            Destroy(line.gameObject);
        }

        lines.Clear();
    }

    private void IdToRC(int id, out int r, out int c)
    {
        int z = id;
        r = z / gridSize; // 행
        c = z % gridSize; // 열
    }

    int GetMiddleId(int a, int b)
    {
        IdToRC(a, out int ar, out int ac);
        IdToRC(b, out int br, out int bc);

        int dr = Mathf.Abs(br - ar), dc = Mathf.Abs(bc - ac);

        if (ar == br && dc == 2) return (a + b) / 2; // 같은 행, 열이 2 차이 (0,2)
        if (ac == bc && dr == 2) return (a + b) / 2; // 같은 열, 행이 2 차이 (0,6)
        if (dc == dr && dc + dr == 4) return (a + b) / 2; // 대각선

        return -1;
    }
}
