using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockPattern : MonoBehaviour
{
    public GameObject linePrefab;
    public Canvas canvas;
    public Transform spawnPoint;
    public MonitorPattern monitorPattern;
    public Sprite[] pointerSprites; // 0 기본 , 1 현재, 2 확정

    public Vector3 offsetPos = new Vector3(0, -450f, 0); // PatternContent의 Rect Transform 값
    private Vector3 offY = new Vector3(0, 150, 0);

    private Dictionary<int, PatternPointer> pointers;

    public List<PatternPointer> lines; // 라인이 어떤 Circle로 부터 생성된지를 파악

    private GameObject lineOnEdit; // 마우스의 포인터를 따라갈 Line
    private RectTransform lineOnEditRect; // 마우스 위치를 저장
    private PatternPointer pointerOnEdit; // 포인터를 저장

    private bool unlocking;

    private new bool enabled = true;

    public int gridSize = 3;

    private float waitTime = 1f; // 패턴 애니메이션 시간

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
        if (dc == dr && dc + dr == 4) // 대각선 일 경우 (0,8)
        {
            return (a + b) / 2;
        }
        return -1;
    }

    private void Start()
    {
        pointers = new Dictionary<int, PatternPointer>();

        for(int i=0; i<transform.childCount; i++)
        {
            var point = transform.GetChild(i);
            var identifier = point.GetComponent<PatternPointer>();

            identifier.id = i;

            pointers.Add(i, identifier);
        }
    }

    private void Update()
    {
        if(enabled == false)
        {
            return;
        }

        if(unlocking)
        {
            Vector3 mousePos = canvas.transform.InverseTransformPoint(Input.mousePosition);
            mousePos += offsetPos + offY; // 마우스 위치 동기화

            lineOnEditRect.sizeDelta = new Vector2(lineOnEditRect.sizeDelta.x, 
                Vector3.Distance(mousePos, pointerOnEdit.transform.localPosition));

            lineOnEditRect.rotation = Quaternion.FromToRotation(
                Vector3.up, (mousePos - pointerOnEdit.transform.localPosition).normalized);
        }
    }

    private IEnumerator Release() // 패턴 제거
    {
        enabled = false;
        monitorPattern.SetPatternSquence();

        yield return new WaitForSeconds(waitTime);

        foreach(var point in pointers)
        {
            //point.Value.GetComponent<Image>().color = Color.white;
            point.Value.GetComponent<Animator>().enabled = false;
            point.Value.image.color = Color.white;
            point.Value.image.sprite = pointerSprites[0];
        }

        foreach (var line in lines)
        {
            Destroy(line.gameObject);
        }

        lines.Clear();

        lineOnEdit = null;
        lineOnEditRect = null;
        pointerOnEdit = null;

        enabled = true;
    }

    /// <summary>
    /// 라인을 포인터의 위치에 맞게 생성
    /// </summary>
    /// <param name="pos">포인터 위치</param>
    /// <param name="id">포인터 id</param>
    private GameObject CreateLine(Vector3 pos, int id)
    {
        var line = GameObject.Instantiate(linePrefab, spawnPoint);

        line.transform.localPosition = pos;

        var linePointer = line.AddComponent<PatternPointer>();

        linePointer.id = id;

        lines.Add(linePointer);

        return line;
    }

    /// <summary>
    /// 포인터를 고려하여 라인을 생성
    /// </summary>
    /// <param name="pp"></param>
    private void TrySetLineEdit(PatternPointer pp) 
    {
        foreach(var line in lines)
        {
            if(line.id == pp.id) // 이미 해당 포인터에 라인이 생성됨
            {
                return;
            }
        }

        if(pointerOnEdit != null) pointerOnEdit.image.sprite = pointerSprites[2]; 
        pp.image.sprite = pointerSprites[1];
        lineOnEdit = CreateLine(pp.transform.localPosition + offsetPos , pp.id); // 첫 생성 위치 동기화
        lineOnEditRect = lineOnEdit.GetComponent<RectTransform>();
        pointerOnEdit = pp;
    }

    private void EnableColorFade(Animator anim, bool isPass = true)
    {
        anim.enabled = true;
        anim.Rebind();

        if (isPass) anim.SetTrigger("IsPass");
        else anim.SetTrigger("IsFail");

        //anim.Rebind();
    }

    public void MouseEnter(PatternPointer pp)
    {
        if (enabled == false)
        {
            return;
        }

        foreach (var line in lines)
        {
            if (line.id == pp.id) // 이미 해당 포인터에 라인이 생성됨
            {
                return;
            }
        }

        if (unlocking) // 버튼을 이미 하나라도 해제했다면 다음 버튼 연결
        {
            int mid = GetMiddleId(pointerOnEdit.id, pp.id); // 현재 저장된 id와 지금 들어온 점의 id

            if (mid != -1) // 중간에 점이 있다면
            {
                lineOnEditRect.sizeDelta = new Vector2(lineOnEditRect.sizeDelta.x, Vector3.
                Distance(pointers[mid].transform.localPosition, pointerOnEdit.transform.localPosition));
                lineOnEditRect.rotation = Quaternion.FromToRotation(
                    Vector3.up, (pointers[mid].transform.localPosition - pointerOnEdit.transform.localPosition)
                    .normalized);

                TrySetLineEdit(pointers[mid]);
            }

            lineOnEditRect.sizeDelta = new Vector2(lineOnEditRect.sizeDelta.x, Vector3.
                Distance(pp.transform.localPosition, pointerOnEdit.transform.localPosition));
            lineOnEditRect.rotation = Quaternion.FromToRotation(
                Vector3.up, (pp.transform.localPosition - pointerOnEdit.transform.localPosition)
                .normalized);

            TrySetLineEdit(pp);
        }
    }

    public void MouseExit(PatternPointer pp)
    {

        if (enabled == false)
        {
            return;
        }

        //Debug.Log("Exit : " + pp.id);
    }

    public void MouseDown(PatternPointer pp)
    {
        //Debug.Log("Down : " + pp.id);

        if (enabled == false)
        {
            return;
        }


        unlocking = true;

        TrySetLineEdit(pp);
    }

    public void MouseUp(PatternPointer pp)
    {
        //Debug.Log("Up : " + pp.id);

        if (enabled == false)
        {
            return;
        }


        if (unlocking) // 패턴 그리기 완료
        {
            List<int> ids = new List<int>();

            foreach (var line in lines) ids.Add(line.id);

            bool checkResult = monitorPattern.CheckPattern(ids);
            Debug.Log("결과값 받아옴:" + checkResult);

            foreach (var line in lines)
            {
                EnableColorFade(pointers[line.id].gameObject.GetComponent<Animator>(), checkResult);
            }


            Destroy(lines[lines.Count - 1].gameObject);
            lines.RemoveAt(lines.Count - 1);

            foreach(var line in lines)
            {
                EnableColorFade(line.GetComponent<Animator>(), checkResult);
            }

            StartCoroutine(Release());
        }

        unlocking = false;

        

    }
}
