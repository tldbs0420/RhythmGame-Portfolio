using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncNoteGenerator : MonoBehaviour
{
    static SyncNoteGenerator instance;
    public static SyncNoteGenerator Instance { get { return instance; } }

    NotePool notePool;
    MusicPlayer musicPlayer;

    public readonly float[] linePos = { -6.47f, -4.812f, -3.163f, -1.522f };

    private int currentBar = 3; //현재 마디 : 초반에는 3마디 분량의 노트들을 만들기 위해 3부터 시작
    private int next = 0; //Sheet의 노트 리스트를 하나씩 읽어들이기 위한 변수
    private int prev = 0; // 이전 노트

    private List<NoteObject> toReleaseList = new List<NoteObject>(); // Pool로 반환시킬 노트 리스트

    public Coroutine coGenTimer; // 음악의 1마디 시간마다 생성할 노트 오브젝트 탐색 = 노트 만들기
    public Coroutine coReleaseTimer; // 반마디 시간 주기로 해제할 노트 오브젝트 탐색 = 노트 없애기
    public Coroutine coInterpolate; // 노트 위치 보간!
    public Coroutine coInterpolateWhenCreate; // 노트 생성때 보간!

    readonly float defaultInterval = 0.003f; // 이걸 기본속도로 해보자 제일 적절한 듯
    public float Interval { get; private set; }

    // 나중에 GameManager로 추가
    float speed = 5.2f; // 내 노트속도 ㅎㅎ;
    public float Speed
    {
        get
        {
            return speed;
        }
        set
        {
            speed = Mathf.Clamp(value, 1.0f, 9.9f);
        }
    }

    public float noteSpeed;

    void Awake()
    {
        if ( instance == null )
            instance = this;
    }
    void Start()
    {
        notePool = FindObjectOfType<NotePool>();
        musicPlayer = MusicPlayer.Instance;
    }
    public void StartGen()
    {
        Interval = defaultInterval * Speed;
        noteSpeed = Interval * 1000;
        coGenTimer = StartCoroutine(IEGenTimer(SyncSheetParser.Instance.sheet.BarPerMilliSec * 0.001f));
        coReleaseTimer = StartCoroutine(IEReleaseTimer(SyncSheetParser.Instance.sheet.BarPerMilliSec * 0.001f * 0.5f));
        coInterpolate = StartCoroutine(IEInterpolate(0.1f, 2.5f)); // 시작 2.5초 노트보간 ( 노트 시작 타이밍 맞추기 )
    }

    public void StopGen()
    {
        if (coGenTimer != null)
        {
            StopCoroutine(coGenTimer);
            coGenTimer = null;
        }
        if (coReleaseTimer != null)
        {
            StopCoroutine(coReleaseTimer);
            coReleaseTimer = null;
        }
        ReleaseCompleted();

        toReleaseList.Clear();
        currentBar = 3;
        next = 0;
        prev = 0;
    }

    void GenNote() // 노트 생성
    {
        List<Note> notes = SyncSheetParser.Instance.sheet.notes; // 노트 시트 불러오기
        Debug.Log(notes[0].time);
        List<Note> reconNotes = new List<Note>(); // 생성할 노트들 리스트

        for (; next < notes.Count; next++) // "읽을 만큼의 노트"의 위치 알아내기
        {
            if (notes[next].time > currentBar * SyncSheetParser.Instance.sheet.BarPerMilliSec)
            {
                break;
            }
        }
        for (int j = prev; j < next; j++) // 읽어낸 위치까지 노트들 리스트에 넣기
        {
            reconNotes.Add(notes[j]);
        }
        prev = next;

        float currentTime = musicPlayer.GetMilliSec();
        
        foreach ( Note note in reconNotes )
        {
            NoteObject noteObject = null;

            switch (note.type)
            {
                case (int)NoteType.Short:
                    if ( note.line == 1 )
                        noteObject = notePool.PoolShortL1.Get();
                    else if ( note.line == 2 )
                        noteObject = notePool.PoolShortL2.Get();
                    else if ( note.line == 3 )
                        noteObject = notePool.PoolShortL3.Get();
                    else if ( note.line == 4 )
                        noteObject = notePool.PoolShortL4.Get();
                    
                    noteObject.SetPosition(new Vector3[] {new Vector3(linePos[note.line - 1], (note.time - currentTime) * Interval, 0f) });
                    break;
                default:
                    break;
            }
            noteObject.speed = noteSpeed;
            noteObject.note = note;
            noteObject.isActivated = true;
            noteObject.gameObject.SetActive(true);
            noteObject.Move();
            toReleaseList.Add(noteObject);
        }
    }

    void Release() // 노트 생성 해제
    {
        List<NoteObject> reconNotes = new List<NoteObject>();
        foreach ( NoteObject note in toReleaseList )
        {
            if (!note.isActivated)
            {
                if ( note is NoteShort )
                {
                    if ( note.note.line == 1 )
                        notePool.PoolShortL1.Release(note as NoteShort);
                }
                note.gameObject.SetActive(false);
            }
            else
            {
                reconNotes.Add(note);
            }
        }
        toReleaseList.Clear();
        toReleaseList.AddRange(reconNotes);
    }

    public void PressedNote( int line, int time ) // 노트 눌렀을 때 제거 처리
    {
        foreach ( NoteObject note in toReleaseList )
        {
            if( line == note.note.line && time == note.note.time || line == note.note.line && time == note.note.tail )
            {
                note.gameObject.SetActive( false );
                note.isActivated = false;
                Release();
                break;
            }
        }
    }

    public void ChangeNoteSpeed( float num )
    {
        Speed += num;
        Interval = defaultInterval * Speed;
        noteSpeed = Interval * 1000;

        Interpolate();
    }

    void ReleaseCompleted() // 모든 노트 제거 ( NotePool로 환원 )
    {
        foreach (NoteObject note in toReleaseList)
        {
            note.gameObject.SetActive(false);

            if (note is NoteShort)
            {
                if ( note.note.line == 1 )
                    notePool.PoolShortL1.Release(note as NoteShort);
            }
        }
    }

    public void Interpolate() // 노트 위치 보간 메소드
    {
        if(coInterpolateWhenCreate != null)
            StopCoroutine(coInterpolateWhenCreate);

        coInterpolateWhenCreate = StartCoroutine(IEInterpolate());
    }

    public IEnumerator IEGenTimer(float interval)
    {
        while ( true )
        {
            GenNote();
            Interpolate(); // 생성때 마다 보간 | 안해주면 같은 타이밍에 나온 노트의 y값에 오차가 생김
            yield return new WaitForSeconds(interval);
            currentBar++;
        }
    }

    public IEnumerator IEReleaseTimer(float interval)
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            Release();
        }
    }

    IEnumerator IEInterpolate(float rate = 1f, float duration = 1f) // 노트 위치 보간 코루틴
    {
        float time = 0;
        while (time < duration )
        {
            float milli = musicPlayer.GetMilliSec();

            foreach (NoteObject note in toReleaseList)
            {
                note.speed = noteSpeed;
                note.Interpolate(milli, Interval, 0);
            }
            time += rate;
            yield return new WaitForSeconds(rate);
        }
    }

    private void OnDestroy() {
        instance = null;
    }
}
