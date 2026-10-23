using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncJudgment : MonoBehaviour
{
    private readonly int good = 150; // GOOD 판정 | 단위 : ms
    private readonly int miss = 225; // MISS 판정 | 단위 : ms

    private List<Queue<Note>> notes = new List<Queue<Note>>(); // 노트라인 Queue를 담을 리스트
    private Queue<Note> note1 = new Queue<Note>(); // 1번 라인의 노트를 담을 Queue


    private int currentTime = 0;


    // ========== 아래의 변수들은 롱노트 판정 구현용이다. ==========

    private Coroutine coCheckMiss;


    SyncNoteGenerator noteGenerator;
    MusicPlayer musicPlayer;
    Effect effect;
    HitEffectPool hitEffectPool;



    static SyncJudgment instance;
    public static SyncJudgment Instance { get { return instance; } }


    void Awake()
    {
        if ( instance == null )
            instance = this;
    }

    void Start()
    {
        noteGenerator = SyncNoteGenerator.Instance;
        musicPlayer = MusicPlayer.Instance;
        effect = Effect.Instance;
        hitEffectPool = HitEffectPool.Instance;
    }

    public void Init()
    {
        Debug.Log("start");
        foreach (var note in notes)
        {
            note.Clear();
        }
        notes.Clear();

        foreach (var note in SyncSheetParser.Instance.sheet.notes)
        {
            if (note.line == 1)
                note1.Enqueue(note);
        }
        notes.Add(note1);

        if (coCheckMiss != null)
        {
            StopCoroutine(coCheckMiss);
        }
        coCheckMiss = StartCoroutine(IECheckMiss());
    }

    public void Judge(int line) // 판정
    {
        if ( notes[line].Count <= 0)
            return;

        Note note = notes[line].Peek();
        int judgeTime = currentTime - note.time;

        if ( judgeTime < good && judgeTime > -good ) // GOOD 판정
        {
            musicPlayer.PlayKeySoundEffect(); // 효과음 재생
            if ( note.type == (int)NoteType.Short)
            {
                hitEffectPool.ShowHitEffect(line);
                NotePress(line, note.time);
                SyncManager.Instance.CalcError(judgeTime);
            }
        }
    }

    void NotePress(int line, int time) // 숏노트 판정 시 반복 부분
    {
        noteGenerator.PressedNote(line + 1, time);
        notes[line].Dequeue();
    }

    IEnumerator IECheckMiss() // 안쳤을 때 MISS 확인용
    {
        while ( true )
        {
            currentTime = (int) MusicPlayer.Instance.GetMilliSec(); // 노래 시간 체크 -> Judge 메소드에서도 쓰인다.

            for (int i = 0; i < notes.Count; i++ )
            {
                if ( notes[i].Count <= 0 )
                    break;
                Note note = notes[i].Peek(); // i 줄의 노트를 불러온다 (i = 0, 1, 2, 3)
                int judgeTime = note.time - currentTime;

                if (note.type == (int)NoteType.Short) // 숏노트 놓침
                {
                    if ( judgeTime < -miss )
                    {
                    noteGenerator.PressedNote(note.line, note.time);
                    notes[i].Dequeue();
                    }
                }

            }

            yield return null;
        }
    }



    private void OnDestroy() {
        instance = null;
    }
}
