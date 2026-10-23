using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EditNotePool : MonoBehaviour
{
    public GameObject notePrefab;
    public GameObject parent;
    public Material lineRendererMaterial;

    IObjectPool<EditNoteShort> editPoolShort;
    public IObjectPool<EditNoteShort> EditPoolShort
    {
        get
        {
            if ( editPoolShort == null )
                editPoolShort = new ObjectPool<EditNoteShort>(CreatePooledShort, defaultCapacity: 256);
            return editPoolShort;
        }
    }

    EditNoteShort CreatePooledShort()
    {
        GameObject note = Instantiate(notePrefab, parent.transform);
        note.AddComponent<EditNoteShort>();
        return note.GetComponent<EditNoteShort>();
    }

    IObjectPool<EditNoteLong> editPoolLong;
    public IObjectPool<EditNoteLong> EditPoolLong
    {
        get
        {
            if ( editPoolLong == null )
                editPoolLong = new ObjectPool<EditNoteLong>(CreatePooledLong, defaultCapacity: 64);
            return editPoolLong;
        }
    }

    EditNoteLong CreatePooledLong()
    {
        GameObject note = new GameObject("NoteLong");
        note.transform.parent = parent.transform;

        GameObject head = Instantiate(notePrefab);
        head.transform.parent = note.transform;
        head.name = "head";
        head.tag = "LongNote";

        GameObject tail = Instantiate(notePrefab);
        tail.transform.parent = note.transform;
        tail.name = "tail";
        tail.tag = "LongNote";

        GameObject line = new GameObject("line");
        line.transform.parent = note.transform;
        line.tag = "LongNote";

        line.AddComponent<LineRenderer>();
        LineRenderer lineRenderer = line.GetComponent<LineRenderer>();
        lineRenderer.material = lineRendererMaterial;
        lineRenderer.sortingOrder = 3;
        lineRenderer.widthMultiplier = 1.2f;
        lineRenderer.positionCount = 2;
        lineRenderer.useWorldSpace = false;

        note.AddComponent<EditNoteLong>();
        return note.GetComponent<EditNoteLong>();
    }


}
