using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class DanceNoteObject : MonoBehaviour
{
    public Vector3 desPos;

    public bool isActivated = false;
    public int moveDirection;
    [SerializeField]
    public Note note = new Note();
    public float speed = 3.0f;

    protected readonly float xInterval = 2.5f;

    public abstract void Move();
    public abstract IEnumerator IEMove();

    public abstract void SetPosition(Vector3[] pos);
    public abstract void Interpolate(float currentTime, float interval);
}



public class DanceNoteShort : DanceNoteObject
{
    public override void Move()
    {
        StartCoroutine(IEMove());
    }

    public override IEnumerator IEMove()
    {
        while ( true )
        {
            transform.position += Vector3.right * speed * Time.deltaTime * moveDirection;
            if ( (transform.position.x > 0f && moveDirection == 1) || (transform.position.x < 0f && moveDirection == -1) )
                isActivated = false;
            
            yield return null;
        }
    }

    public override void SetPosition(Vector3[] pos)
    {
        transform.position = new Vector3(pos[0].x, pos[0].y, pos[0].z);
    }

    public override void Interpolate(float currentTime, float interval) // 노트 위치 보간
    {
        if ( note.line == 1 )
            transform.position = new Vector3(((note.time - currentTime) * interval * -moveDirection) - xInterval, transform.position.y, transform.position.z);
        else if ( note.line == 2 )
            transform.position = new Vector3((note.time - currentTime) * interval * -moveDirection - xInterval, transform.position.y, transform.position.z);
        else if ( note.line == 3 )
            transform.position = new Vector3((note.time - currentTime) * interval + xInterval, transform.position.y, transform.position.z);
        else if ( note.line == 4 )
            transform.position = new Vector3((note.time - currentTime) * interval + xInterval, transform.position.y, transform.position.z);
    }
}

public class DanceNoteLong : DanceNoteObject
{
    LineRenderer lineRenderer;
    public GameObject head;
    public GameObject tail;
    GameObject line;

    public bool isDarked = false;

    void Awake()
    {
        head = transform.GetChild(0).gameObject;
        tail = transform.GetChild(1).gameObject;
        line = transform.GetChild(2).gameObject;
        lineRenderer = line.GetComponent<LineRenderer>();
    }

    public override void Move()
    {
        StartCoroutine(IEMove());
    }

    public override IEnumerator IEMove()
    {
        while ( true )
        {
            transform.position += Vector3.right * speed * Time.deltaTime * moveDirection;

            if ( (tail.transform.position.x > 0f && moveDirection == 1) || (tail.transform.position.x < 0f && moveDirection == -1) )
            {
                isActivated = false;
            }

            yield return null;
        }
    }

    public override void SetPosition(Vector3[] pos)
    {
        transform.position = new Vector3(pos[0].x, pos[0].y, pos[0].z);
        head.transform.position = new Vector3(pos[0].x, pos[0].y, pos[0].z);
        tail.transform.position = new Vector3(pos[1].x, pos[1].y, pos[1].z);
        line.transform.position = head.transform.position;

        Vector3 linePos = tail.transform.position - head.transform.position;
        linePos.y = 0f;
        linePos.z = 0f;
        lineRenderer.SetPosition(1, linePos);
    }

    public override void Interpolate(float currentTime, float interval) // 노트 위치 보간
    {
        if ( note.line == 1 )
        {
            transform.position = new Vector3(((note.time - currentTime) * interval * -moveDirection) - xInterval, head.transform.position.y, head.transform.position.z);
            head.transform.position = new Vector3(((note.time - currentTime) * interval * -moveDirection) - xInterval, head.transform.position.y, head.transform.position.z);
            tail.transform.position = new Vector3(((note.tail - currentTime) * interval * -moveDirection) - xInterval, tail.transform.position.y, tail.transform.position.z);
        }
        else if ( note.line == 2 )
        {
            transform.position = new Vector3((note.time - currentTime) * interval * -moveDirection - xInterval, head.transform.position.y, head.transform.position.z);
            head.transform.position = new Vector3((note.time - currentTime) * interval * -moveDirection - xInterval, head.transform.position.y, head.transform.position.z);
            tail.transform.position = new Vector3((note.tail - currentTime) * interval * -moveDirection - xInterval, tail.transform.position.y, tail.transform.position.z);
        }
        else if ( note.line == 3 )
        {
            transform.position = new Vector3((note.time - currentTime) * interval + xInterval, head.transform.position.y, head.transform.position.z);
            head.transform.position = new Vector3((note.time - currentTime) * interval + xInterval, head.transform.position.y, head.transform.position.z);
            tail.transform.position = new Vector3((note.tail - currentTime) * interval + xInterval, tail.transform.position.y, tail.transform.position.z);
        }
        else if ( note.line == 4 )
        {
            transform.position = new Vector3((note.time - currentTime) * interval + xInterval, head.transform.position.y, head.transform.position.z);
            head.transform.position = new Vector3((note.time - currentTime) * interval + xInterval, head.transform.position.y, head.transform.position.z);
            tail.transform.position = new Vector3((note.tail - currentTime) * interval + xInterval, tail.transform.position.y, tail.transform.position.z);
        }
        line.transform.position = head.transform.position;

        Vector3 linePos = tail.transform.position - head.transform.position;
        linePos.y = 0f;
        linePos.z = 0f;
        lineRenderer.SetPosition(1, linePos);
    }

    public void SetTransparent(float alpha)
    {
        Color color;
        Color lineColor;
        SpriteRenderer sr = head.GetComponent<SpriteRenderer>();
        color = sr.color;
        color *= alpha;
        color.a = 1f;
        sr.color = color;

        sr = tail.GetComponent<SpriteRenderer>();
        sr.color = color;

        lineColor = lineRenderer.material.color;
        lineColor *= alpha;
        lineColor.a = 1f;
        lineRenderer.material.color = lineColor;

        isDarked = !isDarked;
    }
}
