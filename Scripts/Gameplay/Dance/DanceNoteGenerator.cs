using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class DanceNoteGenerator : MonoBehaviour
{
    static DanceNoteGenerator instance;
    public static DanceNoteGenerator Instance { get { return instance; } }


    DanceNotePool notePool;
    MusicPlayer musicPlayer;
    GameManager gameManager;

    private int currentBar = 3; //현재 마디 : 초반에는 3마디 분량의 노트들을 만들기 위해 3부터 시작
    private int next = 0; //Sheet의 노트 리스트를 하나씩 읽어들이기 위한 변수
    private int prev = 0; // 이전 노트

    private List<DanceNoteObject> toReleaseList = new List<DanceNoteObject>();

    Coroutine coGenTimer; // 음악의 1마디 시간마다 생성할 노트 오브젝트 탐색 = 노트 만들기
    Coroutine coReleaseTimer; // 반마디 시간 주기로 해제할 노트 오브젝트 탐색 = 노트 없애기
    Coroutine coInterpolate; // 노트 위치 보간!
    Coroutine coInterpolateWhenCreate; // 노트 생성때 보간!

    private readonly float xInterval = 2.5f;
    private readonly float[] yInterval = new float[2] { 1f, 2.5f };

    readonly float defaultInterval = 0.003f; // 이걸 기본속도로 해보자 제일 적절한 듯
    public float Interval { get; private set; }

    private int sync;

    public float Speed
    {
        get
        {
            return GameManager.Instance.DanceSpeed;
        }
        set
        {
            GameManager.Instance.DanceSpeed = value;
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
        notePool = FindObjectOfType<DanceNotePool>();
        musicPlayer = MusicPlayer.Instance;
        gameManager = GameManager.Instance;
    }
    public void StartGen()
    {
        sync = PlayerPrefs.GetInt("Sync", 0);
        Interval = defaultInterval * Speed;
        noteSpeed = Interval * 1000;
        coGenTimer = StartCoroutine(IEGenTimer(MusicListManager.Instance.sheets[GameManager.Instance.title].BarPerMilliSec * 0.001f));
        coReleaseTimer = StartCoroutine(IEReleaseTimer(MusicListManager.Instance.sheets[GameManager.Instance.title].BarPerMilliSec * 0.001f * 0.5f));
        coInterpolate = StartCoroutine(IEInterpolate(0.1f, 2.5f)); // 시작 2.5초 노트보간 ( 노트 시작 타이밍 맞추기 )
    }

    void GenNote()
    {
        List<Note> notes = MusicListManager.Instance.sheets[GameManager.Instance.title].notes; // 노트 시트 불러오기
        List<Note> reconNotes = new List<Note>(); // 생성할 노트들 리스트

        for (; next < notes.Count; next++) // "읽을 만큼의 노트"의 위치 알아내기
        {
            if (notes[next].time > currentBar * MusicListManager.Instance.sheets[GameManager.Instance.title].BarPerMilliSec)
            {
                break;
            }
        }
        for (int j = prev; j < next; j++) // 읽어낸 위치까지 노트들 리스트에 넣기
        {
            reconNotes.Add(notes[j]);
        }
        prev = next;

        float currentTime = musicPlayer.GetMilliSec() - sync;
        
        foreach ( Note note in reconNotes )
        {
            DanceNoteObject noteObject = null;

            switch (note.type)
            {
                case (int)NoteType.Short:
                    // 왼쪽 (1, 2번 줄)에서 오는 노트는 이동 방향 오른쪽 (moveDirection = 1)
                    if ( note.line == 1 )
                    {
                        noteObject = notePool.PoolShortL1.Get();
                        noteObject.SetPosition(new Vector3[] {new Vector3(((note.time - currentTime) * Interval * (-1)) - xInterval, yInterval[1], 0f) });
                        noteObject.moveDirection = 1;
                    }
                    else if ( note.line == 2 )
                    {
                        noteObject = notePool.PoolShortL2.Get();
                        noteObject.SetPosition(new Vector3[] {new Vector3((note.time - currentTime) * Interval * (-1) - xInterval, yInterval[0], 0f) });
                        noteObject.moveDirection = 1;
                    }
                    // 오른쪽 (3, 4번 줄)에서 오는 노트는 이동 방향 왼쪽 (moveDirection = -1)
                    else if ( note.line == 3 )
                    {
                        noteObject = notePool.PoolShortL3.Get();
                        noteObject.SetPosition(new Vector3[] {new Vector3((note.time - currentTime) * Interval + xInterval, -yInterval[0], 0f) });
                        noteObject.moveDirection = -1;
                    }
                    else if ( note.line == 4 )
                    {
                        noteObject = notePool.PoolShortL4.Get();
                        noteObject.SetPosition(new Vector3[] {new Vector3(((note.time - currentTime) * Interval) + xInterval, -yInterval[1], 0f) });
                        noteObject.moveDirection = -1;
                    }
                    break;
                case (int)NoteType.Long:
                    // 왼쪽 (1, 2번 줄)에서 오는 노트는 이동 방향 오른쪽 (moveDirection = 1)
                    if ( note.line == 1 )
                    {
                        noteObject = notePool.PoolLongL1.Get();
                        noteObject.SetPosition(new Vector3[]
                        {
                            new Vector3(((note.time - currentTime) * Interval * (-1)) - xInterval, yInterval[1], 0f),
                            new Vector3(((note.tail - currentTime) * Interval * (-1)) - xInterval, yInterval[1], 0f)
                        });
                        noteObject.moveDirection = 1;
                    }
                    else if ( note.line == 2 )
                    {
                        noteObject = notePool.PoolLongL2.Get();
                        noteObject.SetPosition(new Vector3[]
                        {
                            new Vector3((note.time - currentTime) * Interval * (-1) - xInterval, yInterval[0], 0f),
                            new Vector3((note.tail - currentTime) * Interval * (-1) - xInterval, yInterval[0], 0f)
                        });
                        noteObject.moveDirection = 1;
                    }
                    // 오른쪽 (3, 4번 줄)에서 오는 노트는 이동 방향 왼쪽 (moveDirection = -1)
                    else if ( note.line == 3 )
                    {
                        noteObject = notePool.PoolLongL3.Get();
                        noteObject.SetPosition(new Vector3[]
                        {
                            new Vector3((note.time - currentTime) * Interval + xInterval, -yInterval[0], 0f),
                            new Vector3((note.tail - currentTime) * Interval + xInterval, -yInterval[0], 0f)
                        });
                        noteObject.moveDirection = -1;
                    }
                    else if ( note.line == 4 )
                    {
                        noteObject = notePool.PoolLongL4.Get();
                        noteObject.SetPosition(new Vector3[]
                        {
                            new Vector3((note.time - currentTime) * Interval + xInterval, -yInterval[1], 0f),
                            new Vector3((note.tail - currentTime) * Interval + xInterval, -yInterval[1], 0f)
                        });
                        noteObject.moveDirection = -1;
                    }
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
        List<DanceNoteObject> reconNotes = new List<DanceNoteObject>();
        foreach ( DanceNoteObject note in toReleaseList )
        {
            if (!note.isActivated)
            {
                if ( note is DanceNoteShort )
                {
                    if ( note.note.line == 1 )
                        notePool.PoolShortL1.Release(note as DanceNoteShort);
                    else if ( note.note.line == 2 )
                        notePool.PoolShortL2.Release(note as DanceNoteShort);
                    else if ( note.note.line == 3 )
                        notePool.PoolShortL3.Release(note as DanceNoteShort);
                    else if ( note.note.line == 4 )
                        notePool.PoolShortL4.Release(note as DanceNoteShort);
                }
                else
                {
                    DanceNoteLong n = note.GetComponent<DanceNoteLong>();
                    if ( n.isDarked ) // 롱노트 실패로 인해 어두워진 롱노트라면 다시 밝게함.
                        n.SetTransparent(2f); // 0.5 낮춘 값을 2배 시키면 원래값 ( 색상값 1 * 0.5 * 2 )

                    if ( note.note.line == 1 )
                        notePool.PoolLongL1.Release(note as DanceNoteLong);
                    else if ( note.note.line == 2 )
                        notePool.PoolLongL2.Release(note as DanceNoteLong);
                    else if ( note.note.line == 3 )
                        notePool.PoolLongL3.Release(note as DanceNoteLong);
                    else if ( note.note.line == 4 )
                        notePool.PoolLongL4.Release(note as DanceNoteLong);
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
        foreach ( DanceNoteObject note in toReleaseList )
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

    public void LongNoteFail( int line, int time ) // 롱노트 실패시 투명화 처리
    {
        foreach (DanceNoteObject note in toReleaseList )
        {
            if ( note is DanceNoteLong )
            {
                if( line == note.note.line && time == note.note.time )
                {
                    DanceNoteLong n = note.GetComponent<DanceNoteLong>();
                    n.SetTransparent(0.5f);
                    break;
                }
            }
        }
    }

    public void ChangeNoteSpeed( float num )
    {
        Speed += num;
        PlayerPrefs.SetFloat("DanceSpeed", Speed);

        Interval = defaultInterval * Speed;
        noteSpeed = Interval * 1000;
        DanceEffect.Instance.SetHiderSpeed();

        Interpolate();
    }

    public void Interpolate() // 노트 위치 보간 메소드
    {
        if(coInterpolateWhenCreate != null)
            StopCoroutine(coInterpolateWhenCreate);

        coInterpolateWhenCreate = StartCoroutine(IEInterpolate());
    }

    IEnumerator IEGenTimer(float interval)
    {
        while ( true )
        {
            GenNote();
            Interpolate(); // 생성때 마다 보간 | 안해주면 같은 타이밍에 나온 노트의 y값에 오차가 생김
            yield return new WaitForSeconds(interval);
            currentBar++;
        }
    }

    IEnumerator IEReleaseTimer(float interval)
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
            float milli = MusicPlayer.Instance.GetMilliSec() - sync;

            foreach (DanceNoteObject note in toReleaseList)
            {
                note.speed = noteSpeed;
                note.Interpolate(milli, Interval);
            }
            time += rate;
            yield return new WaitForSeconds(rate);
        }
    }
}
