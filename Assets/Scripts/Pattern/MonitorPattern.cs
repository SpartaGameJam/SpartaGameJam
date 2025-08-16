using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    // 세 점이 같은 행/열/대각선(↘ 또는 ↙) 위에 있는지
    private bool IsStraightLine(int a, int b, int c)
    {
        IdToRC(a, out int ar, out int ac);
        IdToRC(b, out int br, out int bc);
        IdToRC(c, out int cr, out int cc);

        bool sameRow = (ar == br) && (br == cr);
        bool sameCol = (ac == bc) && (bc == cc);

        bool mainDiag = (ar - ac) == (br - bc) && (br - bc) == (cr - cc); // ↘ 대각(r-c 일정)
        bool antiDiag = (ar + ac) == (br + bc) && (br + bc) == (cr + cc); // ↙ 대각(r+c 일정)

        return sameRow || sameCol || mainDiag || antiDiag;
    }

    // 정답 열쇠
    private HashSet<string> answerSet = new HashSet<string>();

    private void RebuildAnswerSet()
    {
        int first = -1; // 첫 연속 행렬
        int last = -1; // 마지막 연속 행렬

        for(int i=0; i< patternSequence.Count-2; i++)
        {
            if (IsStraightLine(patternSequence[i], patternSequence[i+1], patternSequence[i+2]))
            {
                if (first == -1) first = i; // 첫번째 위치 선택 ex) 0 1 2 라면 0을 선택
                else last = i+1; // 마지막에서 2번째 위치 ex) 6 7 8 이면 7의 인덱스가 선택
            }
        }

        string tmp1 = ""; // 그대로
        string tmp2 = ""; // 앞만 뒤집
        string tmp3 = ""; // 뒤만 뒤집
        string tmp4 = ""; // 둘다 뒤집
        string reverse = ""; // 아예 역순

        for(int i=0; i<patternSequence.Count; i++)
        {
            tmp1 += patternSequence[i].ToString();

            if (first == i) // 앞에 뒤집을 순간
            {
                tmp1 += patternSequence[i + 1].ToString(); // 건너뜀 방지
                tmp2 += patternSequence[i+1].ToString() + patternSequence[i].ToString();
                tmp3 += patternSequence[i].ToString() + patternSequence[i + 1].ToString();
                tmp4 += patternSequence[i+1].ToString() + patternSequence[i].ToString();
                i++;
                continue;
            }
            else if (last == i) // 뒤에 뒤집을 순간
            {
                tmp1 += patternSequence[i + 1].ToString();
                tmp2 += patternSequence[i].ToString() + patternSequence[i + 1].ToString();
                tmp3 += patternSequence[i + 1].ToString() + patternSequence[i].ToString();
                tmp4 += patternSequence[i + 1].ToString() + patternSequence[i].ToString();

                i++;
                continue;
            }
            else // 아무것도 아닐 때
            {
                tmp2 += patternSequence[i].ToString();
                tmp3 += patternSequence[i].ToString();
                tmp4 += patternSequence[i].ToString();
            }
        }

        for (int i = tmp1.Length - 1; i >= 0; i--)
            reverse += tmp1[i];

        answerSet.Add(reverse);

        answerSet.Add(tmp1);
        if(first != -1) answerSet.Add(tmp2);
        if(last != -1) answerSet.Add(tmp3);
        if(first != -1 && last != -1) answerSet.Add(tmp4);

        Debug.Log(tmp1);
        Debug.Log(tmp2);
        Debug.Log(tmp3);
        Debug.Log(tmp4);
        Debug.Log(reverse);
    }

    public bool CheckPattern(List<int> pointlist)
    {
        /*var setA = new HashSet<int>(patternSequence);
        var setB = new HashSet<int>(pointlist);

        return setA.SetEquals(setB);*/
        answerSet.Clear();

        RebuildAnswerSet();

        string result = "";
        foreach (var point in pointlist) {
            result += point.ToString();
        }

        Debug.Log(result);
        //Debug.Log(answerSet.Contains(result));
        //Debug.Log(answerSet.Contains("23458"));

        return answerSet.Contains(result);

        //return answerSet.Contains(ToKey(pointlist));
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
        //SetPatternSquence();

        patternSequence.Add(3);
        patternSequence.Add(4);
        patternSequence.Add(5);
        patternSequence.Add(2);
        patternSequence.Add(8);

        SetLine();
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
