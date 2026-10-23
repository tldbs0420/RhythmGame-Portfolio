using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EditNoteGenerator : MonoBehaviour
{
    static EditNoteGenerator instance;
    public static EditNoteGenerator Instance { get { return instance; } }

    EditNotePool editNotePool;

    public readonly float[] linePos = { -2.476f, -0.825f, 0.827f, 2.482f };
    public List<EditNoteObject> toReleaseList = new List<EditNoteObject>(); // Pool로 반환시킬 노트 리스트


    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        editNotePool = FindObjectOfType<EditNotePool>();
    }

    public void Gen2() // 노트 모두 생성
    {

        List<Note> notes = MusicListManager.Instance.sheets[GameManager.Instance.title].notes;

        float gridLineInterval = 0.5f; // 그리드 선 간격 | Prefab 참조

        float shortPrevPos = 0;
        int shortPrevTime = 0;

        float headLongPrevPos = 0;
        int headLongPrevTime = 0;

        float tailLongPrevPos = 0;
        int tailLongPrevTime = 0;

        foreach (Note note in notes)
        {
            EditNoteObject noteObject = null;
            switch ( note.type )
            {
                case (int)NoteType.Short:
                    noteObject = editNotePool.EditPoolShort.Get();
                    if ( shortPrevTime == 0 ) // 모든 노트 위치는 이전 노트를 기준으로 하여 상대위치로 계산
                    {
                        int pos = Mathf.RoundToInt((note.time - MusicListManager.Instance.sheets[GameManager.Instance.title].offset) / MusicListManager.Instance.sheets[GameManager.Instance.title].BeatPerMilliSec);
                        shortPrevPos += pos;
                        
                        noteObject.SetPosition(new Vector3[] {new Vector3(linePos[note.line - 1], shortPrevPos * gridLineInterval, 0f)});
                    }
                    else
                    {
                        int pos = Mathf.RoundToInt((note.time - shortPrevTime) / MusicListManager.Instance.sheets[GameManager.Instance.title].BeatPerMilliSec);
                        shortPrevPos += pos;

                        noteObject.SetPosition(new Vector3[] { new Vector3(linePos[note.line - 1], shortPrevPos * gridLineInterval, 0f) });
                    }

                    shortPrevTime = note.time;
                    break;

                case (int)NoteType.Long:
                    noteObject = editNotePool.EditPoolLong.Get();
                    if ( headLongPrevTime == 0 )
                    {
                        int pos = Mathf.RoundToInt((note.time - MusicListManager.Instance.sheets[GameManager.Instance.title].offset) / MusicListManager.Instance.sheets[GameManager.Instance.title].BeatPerMilliSec);
                        headLongPrevPos += pos;

                        int pos2 = Mathf.RoundToInt((note.tail - MusicListManager.Instance.sheets[GameManager.Instance.title].offset) / MusicListManager.Instance.sheets[GameManager.Instance.title].BeatPerMilliSec);
                        tailLongPrevPos += pos2;

                        noteObject.SetPosition(new Vector3[]
                        {
                            new Vector3(linePos[note.line - 1], headLongPrevPos * gridLineInterval, 0f),
                            new Vector3(linePos[note.line - 1], tailLongPrevPos * gridLineInterval, 0f)
                        });
                    }
                    else
                    {
                        int pos = Mathf.RoundToInt((note.time - headLongPrevTime) / MusicListManager.Instance.sheets[GameManager.Instance.title].BeatPerMilliSec);
                        headLongPrevPos += pos;

                        int pos2 = Mathf.RoundToInt((note.tail - tailLongPrevTime) / MusicListManager.Instance.sheets[GameManager.Instance.title].BeatPerMilliSec);
                        tailLongPrevPos += pos2;

                        noteObject.SetPosition(new Vector3[]
                        {
                            new Vector3(linePos[note.line - 1], headLongPrevPos * gridLineInterval, 0f),
                            new Vector3(linePos[note.line - 1], tailLongPrevPos * gridLineInterval, 0f)
                        });
                    }
                    headLongPrevTime = note.time;
                    tailLongPrevTime = note.tail;
                    break;

                default:
                    break;
            }

            noteObject.note = note;
            noteObject.isActivated = true;
            noteObject.gameObject.SetActive(true);
            toReleaseList.Add(noteObject);
        }
    }

    public void DisposeNoteShort(Vector3 pos) // 숏노트 배치
    {
        EditNoteObject noteObject = editNotePool.EditPoolShort.Get();
        noteObject.SetPosition(new Vector3[] { pos });
        noteObject.gameObject.SetActive( true );
        noteObject.SetCollider();
        toReleaseList.Add(noteObject);
    }

    EditNoteObject longTemp;
    public void DisposeNoteLong(int makingCount, Vector3[] pos) // 롱노트 배치
    {
        if ( makingCount == 0 )
        {
            longTemp = editNotePool.EditPoolLong.Get();
            longTemp.SetPosition(new Vector3[] { pos[0], pos[1] });
            longTemp.gameObject.SetActive(true);
        }
        else if ( makingCount == 1 )
        {
            longTemp.SetPosition(new Vector3[] { pos[0], pos[1] });
            longTemp.SetCollider();
            toReleaseList.Add(longTemp);
        }
    }
}
