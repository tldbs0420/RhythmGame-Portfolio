using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class NoteObject : MonoBehaviour
{
    public bool isActivated = false;

    [SerializeField]
    public Note note = new Note();

    public float speed = 3.0f;
    public float desPosY = -5f;

    public abstract void Move();
    public abstract IEnumerator IEMove();

    public abstract void SetPosition(Vector3[] pos);
    public abstract void Interpolate(float currentTime, float interval, int sync);
}



public class NoteShort : NoteObject
{
    public override void Move()
    {
        StartCoroutine(IEMove());
    }

    public override IEnumerator IEMove()
    {
        while ( true )
        {
            transform.position += Vector3.down * speed * Time.deltaTime;
            if ( transform.position.y < desPosY )
                isActivated = false;
            
            yield return null;
        }
    }

    public override void SetPosition(Vector3[] pos)
    {
        transform.position = new Vector3(pos[0].x, pos[0].y, pos[0].z);
    }

    public override void Interpolate(float currentTime, float interval, int sync) // 노트 위치 보간
    {
        transform.position = new Vector3(transform.position.x, (note.time - currentTime + sync) * interval, transform.position.z);
    }
}

public class NoteLong : NoteObject
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
            transform.position += Vector3.down * speed * Time.deltaTime;

            if ( tail.transform.position.y < desPosY )
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
        linePos.x = 0f;
        linePos.z = 0f;
        lineRenderer.SetPosition(1, linePos);
    }

    public override void Interpolate(float curruntTime, float interval, int sync) // 노트 위치 보간
    {
        transform.position = new Vector3(head.transform.position.x, (note.time - curruntTime + sync) * interval, head.transform.position.z);
        head.transform.position = new Vector3(head.transform.position.x, (note.time - curruntTime + sync) * interval, head.transform.position.z);
        tail.transform.position = new Vector3(tail.transform.position.x, (note.tail - curruntTime + sync) * interval, tail.transform.position.z);
        line.transform.position = head.transform.position;

        Vector3 linePos = tail.transform.position - head.transform.position;
        linePos.x = 0f;
        linePos.z = 0f;
        lineRenderer.SetPosition(1, linePos);
    }

    public void SetTransparent(float gamma)
    {
        Color color;
        Color lineColor;
        SpriteRenderer sr = head.GetComponent<SpriteRenderer>();
        color = sr.color;
        color *= gamma;
        color.a = 1f;
        sr.color = color;

        sr = tail.GetComponent<SpriteRenderer>();
        sr.color = color;

        lineColor = lineRenderer.material.color;
        lineColor *= gamma;
        lineColor.a = 1f;
        lineRenderer.material.color = lineColor;

        isDarked = !isDarked;
    }

    private void OnDestroy() {
        lineRenderer.material = null;
    }
}
