using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum Level
{
    level1=1, level2=4, level3=8, level4=13,
    level5=19, level6=26
}

public class MonitorPattern : MonoBehaviour
{
    public List<PatternPointer> pointers;
    public List<int> patternSequence; // 포인트 배열 순서
    public List<GameObject> lines;

    public GameObject linePrefab;
    public Canvas canvas; // 하이어라키창 스폿
    public Sprite[] pointerSprites; // 0 기본 , 1 현재, 2 확정
    public Transform spawnPoint;

    private readonly int[] levelList = new int[] { 1, 4, 8, 13, 19, 26 };
    private int level = 3; // 레벨 (디폴트 3으로 설정 2~2)
    private int clearCount = 0; // 클리어 횟수

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
                if (first != -1) last = i+1; // 마지막에서 2번째 위치 ex) 6 7 8 이면 7의 인덱스가 선택
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
        }

        for (int i = 0; i < patternSequence.Count; i++)
        {
            if (first == i) tmp2 += patternSequence[i + 1].ToString() + patternSequence[i++].ToString();
            else tmp2 += patternSequence[i].ToString();
        }

        for (int i = 0; i < patternSequence.Count; i++)
        {
            if (last == i) tmp3 += patternSequence[i + 1].ToString() + patternSequence[i++].ToString();
            else tmp3 += patternSequence[i].ToString();
        }

        for (int i = 0; i < patternSequence.Count; i++)
        {
            if (first == i) tmp4 += patternSequence[i + 1].ToString() + patternSequence[i++].ToString();
            else if (i < first) tmp4 = patternSequence[i].ToString();

            if (last == i)
            {
                if (first == i - 1) // 이전이 first 증가 였다면
                {
                    if (last + 2 < patternSequence.Count) //여유 공간 체크
                    {
                        tmp4 += patternSequence[++i + 1].ToString() + patternSequence[i].ToString();
                    }
                    else tmp4 += patternSequence[i + 1].ToString(); // 그냥 다음 추가
                }
                else
                {
                    tmp4 += patternSequence[i + 1].ToString() + patternSequence[i].ToString();
                }
            }
            else if (i < last) tmp4 = patternSequence[i].ToString();
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

        first = -1;
        last = -1;
        for (int i = 0; i < reverse.Length - 2; i++)
        {
            if (IsStraightLine(reverse[i], reverse[i + 1], reverse[i + 2]))
            {
                if (first == -1) first = i; // 첫번째 위치 선택 ex) 0 1 2 라면 0을 선택
                if (first != -1) last = i + 1; // 마지막에서 2번째 위치 ex) 6 7 8 이면 7의 인덱스가 선택
            }
        }

        Debug.Log("리버스 후 first와 라스트 " + first + " / " + last);

        tmp2 = ""; // 앞만 뒤집
        tmp3 = ""; // 뒤만 뒤집
        tmp4 = ""; // 둘다 뒤집

        for (int i = 0; i < patternSequence.Count; i++)
        {
            if (first == i) tmp2 += reverse[i + 1].ToString() + reverse[i++].ToString();
            else tmp2 += reverse[i].ToString();
        }

        for (int i = 0; i < patternSequence.Count; i++)
        {
            if (last == i) tmp3 += reverse[i + 1].ToString() + reverse[i++].ToString();
            else tmp3 += reverse[i].ToString();
        }

        for (int i = 0; i < patternSequence.Count; i++)
        {
            if (first == i) tmp4 += reverse[i + 1].ToString() + reverse[i++].ToString();
            else if(i < first) tmp4 = reverse[i].ToString();

            if (last == i)
            {
                if(first == i -1) // 이전이 first 증가 였다면
                {
                    if (last + 2 < patternSequence.Count) //여유 공간 체크
                    {
                        tmp4 += reverse[++i + 1].ToString() + reverse[i].ToString();
                    }
                    else tmp4 += reverse[i+1].ToString(); // 그냥 다음 추가
                }
                else
                {
                    tmp4 += reverse[i + 1].ToString() + reverse[i].ToString();
                }
            }
            else if (i < last) tmp4 = reverse[i].ToString();
        }


        Debug.Log("리버스 계산 완료");

        Debug.Log(tmp2);
        Debug.Log(tmp3);
        Debug.Log(tmp4);

        if (first != -1) answerSet.Add(tmp2);
        if (last != -1) answerSet.Add(tmp3);
        if (first != -1 && last != -1) answerSet.Add(tmp4);
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
        pointers = new List<PatternPointer>();
        patternSequence = new List<int>();
        lines = new List<GameObject>();

        for (int i = 0; i < transform.childCount; i++)
        {
            PatternPointer pp =  transform.GetChild(i).gameObject.GetComponent<PatternPointer>();
            pp.id = i;
            pointers.Add(pp);
        }

        //SetPatternSquence();

        StartCoroutine(test());
    }

    private IEnumerator test()
    {
        yield return null;
        //SetPatternSquence();

        /*patternSequence.Add(5);
        patternSequence.Add(8);
        patternSequence.Add(2);
        patternSequence.Add(1);
        patternSequence.Add(0);*/


        /*patternSequence.Add(3);
        patternSequence.Add(4);
        patternSequence.Add(5);
        patternSequence.Add(2);
        patternSequence.Add(8);*/

        patternSequence.Add(2);
        patternSequence.Add(4);
        patternSequence.Add(8);
        patternSequence.Add(0);

        SetLine();
    }

    public void SetPatternSquence()
    {
        Release();

        //int cnt = Random.Range(2, pointers.Count); // 선은 점 2개부터 시작이니
        int cnt = Random.Range(2, level);
        int check = 0; // 현재 들어온 값을 카운트

        foreach (var point in pointers)
        {
            point.image.sprite = pointerSprites[0];
        }

        patternSequence.Clear();

        int ran = Random.Range(0, pointers.Count);
        pointers[ran].image.sprite = pointerSprites[2];
        patternSequence.Add(ran); // 첫 인자값 추가
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

                pointers[mid].image.sprite = pointerSprites[2];
                patternSequence.Add(mid); // 사이 노드 추가
                check++;
            }

            check++;
            pointers[num].image.sprite = pointerSprites[2];
            patternSequence.Add(num);
        }

        SetLine();
    }

    private void SetLine()
    {
        RectTransform lineRect;

        for(int i=0; i<patternSequence.Count-1; i++) // 마지막 라인은 생성 제한
        {
            var line = GameObject.Instantiate(linePrefab, spawnPoint);
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

    public void UpdateClearCount()
    {
        clearCount++;

        if (level > 6) return;
        for(int i=0; i< levelList.Length; i++)
        {
            if(clearCount == levelList[i])
            {
                level++;
                break;
            }
        }
    }
}
