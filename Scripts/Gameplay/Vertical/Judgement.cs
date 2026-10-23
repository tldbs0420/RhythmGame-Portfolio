using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum JudgeType // 일단은 NICE, GOOD, MISS 3개의 판정만 존재한다. 추후 요청 다수 발생시 / 또는 하드모드 개발시? 확장 예정
{
    NICE,
    GOOD, 
    MISS,
}


public class Judgement : MonoBehaviour
{
    private readonly int nice = 75; // NICE 판정 | 단위 : ms
    private readonly int good = 150; // GOOD 판정 | 단위 : ms
    private readonly int miss = 225; // MISS 판정 | 단위 : ms

    private List<Queue<Note>> notes = new List<Queue<Note>>(); // 노트라인 Queue를 담을 리스트
    private Queue<Note> note1 = new Queue<Note>(); // 1번 라인의 노트를 담을 Queue
    private Queue<Note> note2 = new Queue<Note>(); // 2번 라인의 노트를 담을 Queue
    private Queue<Note> note3 = new Queue<Note>(); // 3번 라인의 노트를 담을 Queue
    private Queue<Note> note4 = new Queue<Note>(); // 4번 라인의 노트를 담을 Queue

    private int[] longNoteCheck = new int[4] { 0, 0, 0, 0 }; // 롱노트 누르는 중인지 확인용 | 리스트는 각 라인


    private int currentTime = 0;

    private float waitTime = 0f; // 롱노트 누르는 중의 판정 속도

    private int sync;


    // ========== 아래의 변수들은 롱노트 판정 구현용이다. ==========
    private int[] longComboCount = new int[4] { 0, 0, 0, 0 };               // 롱노트의 콤보가 얼마나 쌓였는지 확인용 | 리스트는 각 라인
    private float[] longTimeCount = new float[4] { 0, 0, 0, 0 };            // 롱노트 판정 마지막 시간 확인용 | 리스트는 각 라인
    private float[] longTimeCountStart = new float[4] { 0, 0, 0, 0 };       // 롱노트의 판정 시간(ms) 시작 후 얼마나 지났는지 확인용 (waitTime 활용) | 리스트는 각 라인
    private JudgeType[] longJudge = new JudgeType[4] { JudgeType.NICE, JudgeType.NICE, JudgeType.NICE, JudgeType.NICE }; // 롱노트 판정 확인용 | 리스트는 각 라인

    private Coroutine coCheckMiss;
    private Coroutine[] coLongEffect = new Coroutine[4];



    NoteGenerator noteGenerator;
    MusicPlayer musicPlayer;
    Effect effect;
    Score score;
    HitEffectPool hitEffectPool;



    static Judgement instance;
    public static Judgement Instance { get { return instance; } }


    void Awake()
    {
        if ( instance == null )
            instance = this;
    }

    void Start()
    {
        noteGenerator = NoteGenerator.Instance;
        musicPlayer = MusicPlayer.Instance;
        effect = Effect.Instance;
        score = Score.Instance;
        hitEffectPool = HitEffectPool.Instance;
    }

    public void Init()
    {
        sync = PlayerPrefs.GetInt("Sync");
        waitTime = MusicListManager.Instance.sheets[GameManager.Instance.title].BeatPerSec * 2; // * 1 - 64비트 | * 2 - 32비트 | * 4 - 16비트
        Debug.Log("start");
        foreach (var note in notes)
        {
            note.Clear();
        }
        notes.Clear();

        foreach (var note in MusicListManager.Instance.sheets[GameManager.Instance.title].notes)
        {
            if (note.line == 1)
                note1.Enqueue(note);
            else if (note.line == 2)
                note2.Enqueue(note);
            else if (note.line == 3)
                note3.Enqueue(note);
            else
                note4.Enqueue(note);
        }
        notes.Add(note1);
        notes.Add(note2);
        notes.Add(note3);
        notes.Add(note4);

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

        if ( judgeTime < nice && judgeTime > -nice ) // NICE 판정
        {
            musicPlayer.PlayKeySoundEffect(); // 효과음 재생
            if ( note.type == (int)NoteType.Short)
            {
                hitEffectPool.ShowHitEffect(line);
                NotePress(JudgeType.NICE, line, note.time);
            }
            else if ( note.type == (int)NoteType.Long )
            {
                LongNotePress(JudgeType.NICE, line, note.time, note.tail);
            }
        }

        else if ( judgeTime < good && judgeTime > -good ) // GOOD 판정
        {
            musicPlayer.PlayKeySoundEffect(); // 효과음 재생
            if ( note.type == (int)NoteType.Short)
            {
                hitEffectPool.ShowHitEffect(line);
                NotePress(JudgeType.GOOD, line, note.time);
            }
            else if ( note.type == (int)NoteType.Long )
            {
                LongNotePress(JudgeType.GOOD, line, note.time, note.tail);
            }
        }

        else if ( judgeTime < miss && judgeTime > -miss ) // MISS 판정 | 이때는 효과음이 재생되지 않고 Score에 fastMiss로 등록된다.
        {
            NotePress(JudgeType.MISS, line, note.time);
        }
    }

    public void CheckLongNote ( int line )  // 롱노트 확인
    {
        if (coLongEffect[line] != null)
            StopCoroutine(coLongEffect[line]);

        if ( notes[line].Count <= 0 )
            return;

        Note note = notes[line].Peek();
        if ( note.type != (int)NoteType.Long )
            return;

        if ( longNoteCheck[note.line - 1] != 1 )
            return;



            
        int judgeTime = currentTime - note.tail;
        if ( judgeTime + note.tail > note.tail ) // 롱노트가 가진 시간보다 길게 눌렀다가 땠을 경우, 판정할 필요가 없으므로 종료
        {
            return;
        }

        if ( judgeTime > -good ) // GOOD 판정 범위 내에서 롱노트를 때면 최대 콤보로 적용시킨다
        {
            while ( longTimeCountStart[line] < longTimeCount[line] ) // 최대 판정보다 몇개의 콤보만큼 부족한지 확인
            {
                longComboCount[line] += 1;
                longTimeCountStart[line] += waitTime * 1000f;
            }
            if ( longJudge[line] == JudgeType.NICE ) // 롱노트 판정 시작을 NICE로 했을 때
            {
                score.data.nice += longComboCount[line];
                score.data.combo += longComboCount[line];
            }
            else // 롱노트 판정 시작을 GOOD으로 했을 때
            {
                score.data.good += longComboCount[line];
                score.data.combo = 0;
            }
            longComboCount[line] = 0;
            effect.HideHider(line);
            longNoteCheck[line] = 0;
            noteGenerator.PressedNote(note.line, note.tail);
            notes[line].Dequeue();
            score.SetAccuracy();
            score.SetScore();
        }
        else if ( judgeTime < -good ) // GOOD 판정 이전에 롱노트를 떼버리면 MISS
        {
            score.data.fastMiss++;
            score.data.judge = JudgeType.MISS;
            score.data.combo = 0;
            longNoteCheck[line] = 0;
            noteGenerator.LongNoteFail(note.line, note.time); // 롱노트 반투명화
            notes[line].Dequeue();
            effect.MoveHider(line);
            score.SetAccuracy();
            score.SetScore();
        }
    }

    void NotePress(JudgeType type, int line, int time) // 숏노트 판정 시 반복 부분
    {
        switch ( type )
        {
            case JudgeType.NICE:
                score.data.nice++;
                score.data.combo++;
                break;
            case JudgeType.GOOD:
                score.data.good++;
                score.data.combo = 0;
                break;
            case JudgeType.MISS:
                score.data.fastMiss++;
                score.data.combo = 0;
                break;
        }
        score.data.judge = type;
        noteGenerator.PressedNote(line + 1, time);
        notes[line].Dequeue();
        score.SetAccuracy();
        score.SetScore();
    }

    void LongNotePress(JudgeType type, int line, int time, int tail) // 롱노트 판정 시 반복 부분
    {
        switch ( type )
        {
            case JudgeType.NICE:
                longJudge[line] = JudgeType.NICE;
                break;
            case JudgeType.GOOD:
                longJudge[line] = JudgeType.GOOD;
                break;
        }
        longNoteCheck[line] = 1;
        effect.ShowHider(line);
        longTimeCountStart[line] = time;
        longTimeCount[line] = tail;
        coLongEffect[line] = StartCoroutine(IELongEffect(line, longJudge[line]));
    }

    IEnumerator IECheckMiss() // 안쳤을 때 MISS 확인용
    {
        while ( true )
        {
            currentTime = (int) MusicPlayer.Instance.GetMilliSec() - sync; // 노래 시간 체크 -> Judge 메소드에서도 쓰인다.

            for (int i = 0; i < notes.Count; i++ )
            {
                if ( notes[i].Count <= 0 )
                    continue;
                Note note = notes[i].Peek(); // i 줄의 노트를 불러온다 (i = 0, 1, 2, 3)
                int judgeTime = note.time - currentTime;

                if (note.type == (int)NoteType.Short) // 숏노트 놓침
                {
                    if ( judgeTime < -miss )
                    {
                    score.data.miss++;
                    score.data.judge = JudgeType.MISS;
                    score.data.combo = 0;
                    noteGenerator.PressedNote(note.line, note.time);
                    notes[i].Dequeue();
                    score.SetAccuracy();
                    score.SetScore();
                    }
                }
                else if (note.type == (int)NoteType.Long) // 롱노트 놓침
                {
                    if ( longNoteCheck[note.line - 1] == 0 ) // 롱노트를 안누르고 놓쳤을 때
                    {
                        if ( judgeTime < -miss )
                        {
                            score.data.miss++;
                            score.data.judge = JudgeType.MISS;
                            score.data.combo = 0;
                            noteGenerator.LongNoteFail(note.line, note.time);
                            notes[i].Dequeue();
                            score.SetAccuracy();
                            score.SetScore();
                        }
                    }
                    else if ( longNoteCheck[note.line - 1] == 1 ) // 롱노트를 누르고 있을 때
                    {
                        int judgeLongTime = note.tail - currentTime;
                        if ( judgeLongTime < -good ) // 계속 누르고 있으면 해당 롱노트는 성공이기 때문에 없애기만 함. 롱노트를 끝까지 누르고 GOOD 판정 이내에 때면 CheckLongNote 메소드에서 처리함.
                        {
                            effect.HideHider(note.line - 1);
                            noteGenerator.PressedNote(note.line, note.time);
                            longNoteCheck[note.line - 1] = 0;
                            notes[i].Dequeue();
                        }
                    }
                }

            }

            yield return null;
        }
    }

    IEnumerator IELongEffect( int line, JudgeType judgeType ) // 롱노트 누르는 중 판정
    {
        while ( longTimeCountStart[line] < longTimeCount[line] )
        {
            if ( judgeType == JudgeType.NICE )
            {
                score.data.nice++;
                score.data.judge = JudgeType.NICE;
                score.data.combo++;
            }
            else if ( judgeType == JudgeType.GOOD )
            {
                score.data.good++;
                score.data.judge = JudgeType.GOOD;
                score.data.combo = 0;
            }
            score.SetScore();
            hitEffectPool.ShowHitEffect(line);
            longTimeCountStart[line] += waitTime * 1000f ;

            if (longTimeCountStart[line] > longTimeCount[line])
                score.SetAccuracy();
            
            yield return new WaitForSeconds( waitTime );
        }
            
    }


    private void OnDestroy() {
        instance = null;
    }
}
