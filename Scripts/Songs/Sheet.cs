using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[Serializable]
public struct Note // 노트 구조체 구성
    {
        public int time; // 노트 위치값
        public int type; // 노트 종류 (일반노트/롱노트)

        /// <summary>
        /// 노트 줄 (1, 2, 3, 4)
        /// </summary>
        public int line;

        public int tail; // 롱노트 여부 및 위치 (값이 -1이 아니라면 해당 값이 롱노트 끝의 위치)

        public Note (int time, int type, int line, int tail) // 노트 생성자
        {
            this.time = time;
            this.type = type;
            this.line = line;
            this.tail = tail;
        }
    }

public enum NoteType
{
    Short = 0,
    Long = 1,
}

public class Sheet
{
    static Sheet instance;
    public static Sheet Instance
    {
        get
        {
            if ( instance == null )
                instance = new Sheet();
            return instance;
        }
    }
    
    // 채보 정보
    public float BPM { set; get; }
    public float offset { set; get; }
    public string Signature { get; set; } = "6/8";

    // 곡 세부사항 정보
    public string Title { set; get; }
    public string Artist { set; get; }
    public string Original { set; get; }

    public string clip;
    public Sprite img;

    public int sign = 6 ;
    public float beatCount = 64f;

    // 노트 정보
    private readonly Dictionary<GameManager.DifficultyType, List<Note>> noteGroups = new Dictionary<GameManager.DifficultyType, List<Note>>()
    {
        { GameManager.DifficultyType.EASY, new List<Note>() },
        { GameManager.DifficultyType.NORMAL, new List<Note>() },
        { GameManager.DifficultyType.HARD, new List<Note>() }
    }; // 이전과는 다르게 각 난이도별로 이곳에 "모든"노트를 넣어둔다. (라인 정보도 함께 들어있기 때문)

    private GameManager.DifficultyType currentDifficulty = GameManager.DifficultyType.NORMAL;
    public List<Note> notes => noteGroups[currentDifficulty];
    public List<Note> GetNotes(GameManager.DifficultyType difficulty)
    {
        return noteGroups[difficulty];
    }
    public void SelectDifficulty(int difficulty)
    {
        currentDifficulty = (GameManager.DifficultyType)Mathf.Clamp(difficulty, 0, 2);
    }

    public float BarPerSec { get; private set; } // 초당 마디
    public float BeatPerSec { get; private set; } // 초당 비트

    public int BarPerMilliSec { get; private set; } // 초당 마디 (ms)
    public float BeatPerMilliSec { get; private set; } // 초당 비트 (ms)

    public void Init()
    {
        BarPerMilliSec = (int)(sign * (60f / BPM) * 1000);
        BeatPerMilliSec = BarPerMilliSec / beatCount;

        BarPerSec = BarPerMilliSec * 0.001f;
        BeatPerSec = BarPerSec / beatCount;

    }
}
