using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EditNoteObject : MonoBehaviour
{
    public bool isActivated = false;

    protected int gridInterval = 32; // 그리드 높이 (Prefab 참조)

    public Note note = new Note();

    public abstract void SetPosition(Vector3[] pos);

    public abstract void SetCollider();
    public abstract IEnumerator IECheckCollider();
}

public class EditNoteShort : EditNoteObject
{
    public override void SetPosition(Vector3[] pos)
    {
        transform.position = new Vector3(pos[0].x, pos[0].y, pos[0].z);
    }

    public override void SetCollider() // 충돌 설정
    {
        StartCoroutine(IECheckCollider());
    }

    public override IEnumerator IECheckCollider()
    {
        WaitForSeconds wait = new WaitForSeconds(0.1f);
        while ( true )
        {
            int time = (int)transform.localPosition.y;
            int currentBar = Editor.Instance.currentBar;
            if ( time >= (currentBar - 3) * gridInterval && time <= (currentBar + 3) * gridInterval) // 자신의 마디가 왔을 때만 충돌 허용 ( 배치 허용 )
                GetComponent<BoxCollider2D>().enabled = true;
            else
                GetComponent<BoxCollider2D>().enabled = false;

            yield return wait;
        }
    }
}

public class EditNoteLong : EditNoteObject
{
    LineRenderer lineRenderer;
    public GameObject head;
    public GameObject tail;
    GameObject line;

    void Awake()
    {
        head = transform.GetChild(0).gameObject;
        tail = transform.GetChild(1).gameObject;
        line = transform.GetChild(2).gameObject;
        lineRenderer = line.GetComponent<LineRenderer>();
    }
    public override void SetPosition(Vector3[] pos)
    {
        transform.position = new Vector3(pos[0].x, pos[0].y, pos[0].z);
        head.transform.position = new Vector3(pos[0].x, pos[0].y, pos[0].z);
        tail.transform.position = new Vector3(pos[1].x, pos[1].y, pos[1].z);
        line.transform.position = head.transform.position;

        Vector3 linePos = tail.transform.position - head.transform.position;
        linePos.x = 0f;
        linePos.z = 0f;
        lineRenderer.SetPosition(1, linePos);
    }

    public override void SetCollider() // 충돌 설정
    {
        StartCoroutine(IECheckCollider());
    }

    public override IEnumerator IECheckCollider() // 자신의 마디가 왔을 때만 충돌 허용 ( 배치 허용 )
    {
        WaitForSeconds wait = new WaitForSeconds(0.1f);
        while (true)
        {
            int time = (int)transform.localPosition.y;
            int currentBar = Editor.Instance.currentBar;
            if (time >= (currentBar - 3) * gridInterval && time <= (currentBar + 3) * gridInterval )
            {
                head.GetComponent<BoxCollider2D>().enabled = true;
                tail.GetComponent<BoxCollider2D>().enabled = true;
            }
            else
            {
                head.GetComponent<BoxCollider2D>().enabled = false;
                tail.GetComponent<BoxCollider2D>().enabled = false;
            }

            yield return wait;
        }
    }
}
