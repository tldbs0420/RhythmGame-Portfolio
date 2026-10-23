using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class NotePool : MonoBehaviour
{
    public GameObject[] notePrefabs;
    public GameObject parent;
    public Material lineRendererMaterial;

    public Color[] colors = new Color[4] {
        new Color(1, 1, 1, 1),                              //new Color(1, 1, 1, 1)
        new Color(0.6745098f, 0.9058824f, 0.9803922f, 1),   //new Color(0.7215686f, 0.4862745f, 0.8588236f, 1)
        new Color(0.6745098f, 0.9058824f, 0.9803922f, 1),   //new Color(1, 0, 0, 1),
        new Color(1, 1, 1, 1)                               // new Color(0.6745098f, 0.9058824f, 0.9803922f, 1)
    };

    public void SetNotesAndColors(Sprite[] newNotes, Color[] newColors)
    {
        SetNotes(newNotes);
        SetColors(newColors);
    }

    public void SetNotes(Sprite[] newNotes)
    {
        for (int i = 0; i < 4; i++)
            notePrefabs[i].GetComponent<SpriteRenderer>().sprite = newNotes[i];
    }

    public void SetColors(Color[] newColors)
    {
        for (int i = 0; i < 4; i++)
            colors[i] = newColors[i];
    }



// ================================= Short Note ===================================

    IObjectPool<NoteShort> poolShortL1;
    public IObjectPool<NoteShort> PoolShortL1
    {
        get
        {
            if ( poolShortL1 == null )
            {
                poolShortL1 = new ObjectPool<NoteShort>(CreatePooledShortL1, defaultCapacity: 64);
            }
            return poolShortL1;
        }
    }

    NoteShort CreatePooledShortL1()
    {
        GameObject note = Instantiate(notePrefabs[0], parent.transform);
        note.AddComponent<NoteShort>();
        return note.GetComponent<NoteShort>();
    }

    IObjectPool<NoteShort> poolShortL2;
    public IObjectPool<NoteShort> PoolShortL2
    {
        get
        {
            if ( poolShortL2 == null )
            {
                poolShortL2 = new ObjectPool<NoteShort>(CreatePooledShortL2, defaultCapacity: 64);
            }
            return poolShortL2;
        }
    }

    NoteShort CreatePooledShortL2()
    {
        GameObject note = Instantiate(notePrefabs[1], parent.transform);
        note.AddComponent<NoteShort>();
        return note.GetComponent<NoteShort>();
    }

    IObjectPool<NoteShort> poolShortL3;
    public IObjectPool<NoteShort> PoolShortL3
    {
        get
        {
            if ( poolShortL3 == null )
            {
                poolShortL3 = new ObjectPool<NoteShort>(CreatePooledShortL3, defaultCapacity: 64);
            }
            return poolShortL3;
        }
    }

    NoteShort CreatePooledShortL3()
    {
        GameObject note = Instantiate(notePrefabs[2], parent.transform);
        note.AddComponent<NoteShort>();
        return note.GetComponent<NoteShort>();
    }

    IObjectPool<NoteShort> poolShortL4;
    public IObjectPool<NoteShort> PoolShortL4
    {
        get
        {
            if ( poolShortL4 == null )
            {
                poolShortL4 = new ObjectPool<NoteShort>(CreatePooledShortL4, defaultCapacity: 64);
            }
            return poolShortL4;
        }
    }

    NoteShort CreatePooledShortL4()
    {
        GameObject note = Instantiate(notePrefabs[3], parent.transform);
        note.AddComponent<NoteShort>();
        return note.GetComponent<NoteShort>();
    }




// ================================ Long Note =================================



    IObjectPool<NoteLong> poolLongL1;
    public IObjectPool<NoteLong> PoolLongL1
    {
        get
        {
            if ( poolLongL1 == null )
                poolLongL1 = new ObjectPool<NoteLong>(CreatePooledLongL1, defaultCapacity: 16);
            return poolLongL1;
        }
    }

    NoteLong CreatePooledLongL1()
    {
        GameObject note = new GameObject("NoteLong");
        note.transform.parent = parent.transform;

        GameObject head = Instantiate(notePrefabs[0]);
        head.transform.parent = note.transform;
        head.name = "head";

        GameObject tail = Instantiate(notePrefabs[0]);
        tail.transform.parent = note.transform;
        tail.name = "tail";

        GameObject line = new GameObject("line");
        line.transform.parent = note.transform;

        line.AddComponent<LineRenderer>();
        LineRenderer lineRenderer = line.GetComponent<LineRenderer>();
        lineRenderer.material = lineRendererMaterial;
        lineRenderer.material.color = colors[0];
        lineRenderer.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
        lineRenderer.sortingOrder = -1;
        lineRenderer.widthMultiplier = 1.2f;
        lineRenderer.positionCount = 2;
        lineRenderer.useWorldSpace = false;

        note.AddComponent<NoteLong>();
        return note.GetComponent<NoteLong>();
    }

    IObjectPool<NoteLong> poolLongL2;
    public IObjectPool<NoteLong> PoolLongL2
    {
        get
        {
            if ( poolLongL2 == null )
                poolLongL2 = new ObjectPool<NoteLong>(CreatePooledLongL2, defaultCapacity: 16);
            return poolLongL2;
        }
    }

    NoteLong CreatePooledLongL2()
    {
        GameObject note = new GameObject("NoteLong");
        note.transform.parent = parent.transform;

        GameObject head = Instantiate(notePrefabs[1]);
        head.transform.parent = note.transform;
        head.name = "head";

        GameObject tail = Instantiate(notePrefabs[1]);
        tail.transform.parent = note.transform;
        tail.name = "tail";

        GameObject line = new GameObject("line");
        line.transform.parent = note.transform;

        line.AddComponent<LineRenderer>();
        LineRenderer lineRenderer = line.GetComponent<LineRenderer>();
        lineRenderer.material = lineRendererMaterial;
        lineRenderer.material.color = colors[1];
        lineRenderer.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
        lineRenderer.sortingOrder = -1;
        lineRenderer.widthMultiplier = 1.2f;
        lineRenderer.positionCount = 2;
        lineRenderer.useWorldSpace = false;

        note.AddComponent<NoteLong>();
        return note.GetComponent<NoteLong>();
    }

    IObjectPool<NoteLong> poolLongL3;
    public IObjectPool<NoteLong> PoolLongL3
    {
        get
        {
            if ( poolLongL3 == null )
                poolLongL3 = new ObjectPool<NoteLong>(CreatePooledLongL3, defaultCapacity: 16);
            return poolLongL3;
        }
    }

    NoteLong CreatePooledLongL3()
    {
        GameObject note = new GameObject("NoteLong");
        note.transform.parent = parent.transform;

        GameObject head = Instantiate(notePrefabs[2]);
        head.transform.parent = note.transform;
        head.name = "head";

        GameObject tail = Instantiate(notePrefabs[2]);
        tail.transform.parent = note.transform;
        tail.name = "tail";

        GameObject line = new GameObject("line");
        line.transform.parent = note.transform;

        line.AddComponent<LineRenderer>();
        LineRenderer lineRenderer = line.GetComponent<LineRenderer>();
        lineRenderer.material = lineRendererMaterial;
        lineRenderer.material.color = colors[2];
        lineRenderer.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
        lineRenderer.sortingOrder = -1;
        lineRenderer.widthMultiplier = 1.2f;
        lineRenderer.positionCount = 2;
        lineRenderer.useWorldSpace = false;

        note.AddComponent<NoteLong>();
        return note.GetComponent<NoteLong>();
    }

    IObjectPool<NoteLong> poolLongL4;
    public IObjectPool<NoteLong> PoolLongL4
    {
        get
        {
            if ( poolLongL4 == null )
                poolLongL4 = new ObjectPool<NoteLong>(CreatePooledLongL4, defaultCapacity: 16);
            return poolLongL4;
        }
    }

    NoteLong CreatePooledLongL4()
    {
        GameObject note = new GameObject("NoteLong");
        note.transform.parent = parent.transform;

        GameObject head = Instantiate(notePrefabs[3]);
        head.transform.parent = note.transform;
        head.name = "head";

        GameObject tail = Instantiate(notePrefabs[3]);
        tail.transform.parent = note.transform;
        tail.name = "tail";

        GameObject line = new GameObject("line");
        line.transform.parent = note.transform;

        line.AddComponent<LineRenderer>();
        LineRenderer lineRenderer = line.GetComponent<LineRenderer>();
        lineRenderer.material = lineRendererMaterial;
        lineRenderer.material.color = colors[3];
        lineRenderer.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
        lineRenderer.sortingOrder = -1;
        lineRenderer.widthMultiplier = 1.2f;
        lineRenderer.positionCount = 2;
        lineRenderer.useWorldSpace = false;

        note.AddComponent<NoteLong>();
        return note.GetComponent<NoteLong>();
    }

    

}
