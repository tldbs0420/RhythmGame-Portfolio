using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class DanceNotePool : MonoBehaviour
{
    public GameObject[] notePrefabs;
    public GameObject parent;
    public Material lineRendererMaterial;
    float lineWidth = 0.3f;

    Color[] colors = new Color[4] {
        new Color(1f, 0.1556604f, 0.1556604f, 1),                              //new Color(1, 1, 1, 1)
        new Color(0.2216981f, 0.663664f,1f, 1),   //new Color(0.7215686f, 0.4862745f, 0.8588236f, 1)
        new Color(0.4661676f, 1f, 0.2235294f, 1),   //new Color(1, 0, 0, 1),
        new Color(1, 0.2235294f, 0.640916f, 1)                               // new Color(0.6745098f, 0.9058824f, 0.9803922f, 1)
    };



// ================================= Short Note ===================================

    IObjectPool<DanceNoteShort> poolShortL1;
    public IObjectPool<DanceNoteShort> PoolShortL1
    {
        get
        {
            if ( poolShortL1 == null )
            {
                poolShortL1 = new ObjectPool<DanceNoteShort>(CreatePooledShortL1, defaultCapacity: 64);
            }
            return poolShortL1;
        }
    }

    DanceNoteShort CreatePooledShortL1()
    {
        GameObject note = Instantiate(notePrefabs[0], parent.transform);
        note.AddComponent<DanceNoteShort>();
        return note.GetComponent<DanceNoteShort>();
    }

    IObjectPool<DanceNoteShort> poolShortL2;
    public IObjectPool<DanceNoteShort> PoolShortL2
    {
        get
        {
            if ( poolShortL2 == null )
            {
                poolShortL2 = new ObjectPool<DanceNoteShort>(CreatePooledShortL2, defaultCapacity: 64);
            }
            return poolShortL2;
        }
    }

    DanceNoteShort CreatePooledShortL2()
    {
        GameObject note = Instantiate(notePrefabs[1], parent.transform);
        note.AddComponent<DanceNoteShort>();
        return note.GetComponent<DanceNoteShort>();
    }

    IObjectPool<DanceNoteShort> poolShortL3;
    public IObjectPool<DanceNoteShort> PoolShortL3
    {
        get
        {
            if ( poolShortL3 == null )
            {
                poolShortL3 = new ObjectPool<DanceNoteShort>(CreatePooledShortL3, defaultCapacity: 64);
            }
            return poolShortL3;
        }
    }

    DanceNoteShort CreatePooledShortL3()
    {
        GameObject note = Instantiate(notePrefabs[2], parent.transform);
        note.AddComponent<DanceNoteShort>();
        return note.GetComponent<DanceNoteShort>();
    }

    IObjectPool<DanceNoteShort> poolShortL4;
    public IObjectPool<DanceNoteShort> PoolShortL4
    {
        get
        {
            if ( poolShortL4 == null )
            {
                poolShortL4 = new ObjectPool<DanceNoteShort>(CreatePooledShortL4, defaultCapacity: 64);
            }
            return poolShortL4;
        }
    }

    DanceNoteShort CreatePooledShortL4()
    {
        GameObject note = Instantiate(notePrefabs[3], parent.transform);
        note.AddComponent<DanceNoteShort>();
        return note.GetComponent<DanceNoteShort>();
    }




// ================================ Long Note =================================



    IObjectPool<DanceNoteLong> poolLongL1;
    public IObjectPool<DanceNoteLong> PoolLongL1
    {
        get
        {
            if ( poolLongL1 == null )
                poolLongL1 = new ObjectPool<DanceNoteLong>(CreatePooledLongL1, defaultCapacity: 16);
            return poolLongL1;
        }
    }

    DanceNoteLong CreatePooledLongL1()
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
        lineRenderer.sortingOrder = 1;
        lineRenderer.widthMultiplier = lineWidth;
        lineRenderer.positionCount = 2;
        lineRenderer.useWorldSpace = false;

        note.AddComponent<DanceNoteLong>();
        return note.GetComponent<DanceNoteLong>();
    }

    IObjectPool<DanceNoteLong> poolLongL2;
    public IObjectPool<DanceNoteLong> PoolLongL2
    {
        get
        {
            if ( poolLongL2 == null )
                poolLongL2 = new ObjectPool<DanceNoteLong>(CreatePooledLongL2, defaultCapacity: 16);
            return poolLongL2;
        }
    }

    DanceNoteLong CreatePooledLongL2()
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
        lineRenderer.sortingOrder = 1;
        lineRenderer.widthMultiplier = lineWidth;
        lineRenderer.positionCount = 2;
        lineRenderer.useWorldSpace = false;

        note.AddComponent<DanceNoteLong>();
        return note.GetComponent<DanceNoteLong>();
    }

    IObjectPool<DanceNoteLong> poolLongL3;
    public IObjectPool<DanceNoteLong> PoolLongL3
    {
        get
        {
            if ( poolLongL3 == null )
                poolLongL3 = new ObjectPool<DanceNoteLong>(CreatePooledLongL3, defaultCapacity: 16);
            return poolLongL3;
        }
    }

    DanceNoteLong CreatePooledLongL3()
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
        lineRenderer.sortingOrder = 1;
        lineRenderer.widthMultiplier = lineWidth;
        lineRenderer.positionCount = 2;
        lineRenderer.useWorldSpace = false;

        note.AddComponent<DanceNoteLong>();
        return note.GetComponent<DanceNoteLong>();
    }

    IObjectPool<DanceNoteLong> poolLongL4;
    public IObjectPool<DanceNoteLong> PoolLongL4
    {
        get
        {
            if ( poolLongL4 == null )
                poolLongL4 = new ObjectPool<DanceNoteLong>(CreatePooledLongL4, defaultCapacity: 16);
            return poolLongL4;
        }
    }

    DanceNoteLong CreatePooledLongL4()
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
        lineRenderer.sortingOrder = 1;
        lineRenderer.widthMultiplier = lineWidth;
        lineRenderer.positionCount = 2;
        lineRenderer.useWorldSpace = false;

        note.AddComponent<DanceNoteLong>();
        return note.GetComponent<DanceNoteLong>();
    }

}
