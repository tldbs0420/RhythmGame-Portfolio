using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public struct ScoreData
{
    public int nice; // NICE 판정 개수
    public int good; // GOOD 판정 개수
    public int miss; // MISS 판정 개수
    public int fastMiss; // FASTMISS 판정 개수

    public string[] judgeText;
    public Color[] judgeColor;
    public JudgeType judge;
    public int combo;
    public int maxCombo; 
    public int Score{
        get
        {
            return ( nice * 500 ) + ( good * 250 ); // NICE 500점, GOOD 250점
        }
        set
        {
            Score = value;
        }
    }

    public float Accuracy
    {
        get
        {
            int total = nice + good + miss + fastMiss;
            if (total == 0) return 0f;
                
            return ( ((float)Score  / (total * 500)) * 100 ); // 현재 점수 / 현재까지 판정된 모든 노트가 NICE일 때의 점수 비율 = 정확도
        }

        set
        {
            Accuracy = value;
        }
    }
}

public class Score : MonoBehaviour
{
    static Score instance;
    public static Score Instance { get { return instance; } }

    public ScoreData data;

    UIText uiJudgement;
    UIText uiCombo;
    UIText uiScore;
    UIText uiAccuracy;
    UISlider uiProgress;

    GameAnimations gameAnimations;
    HPManager hpManager;
    MusicPlayer musicPlayer;

    public string perfectString = "뿡빵띠";

    void Awake()
    {
        if ( instance == null )
            instance = this;
    }

    void Start()
    {
        gameAnimations = GameAnimations.Instance;
        hpManager = HPManager.Instance;
        musicPlayer = MusicPlayer.Instance;
    }

    public void Init()
    {
        uiJudgement = UIController.Instance.FindUI("UI_Judgement").uiObject as UIText;
        uiCombo = UIController.Instance.FindUI("UI_Combo").uiObject as UIText;
        uiScore = UIController.Instance.FindUI("UI_Score").uiObject as UIText;
        uiAccuracy = UIController.Instance.FindUI("UI_Accuracy").uiObject as UIText;
        uiProgress = UIController.Instance.FindUI("UI_Progress").uiObject as UISlider;
        perfectString = GameManager.Instance.GetPlayerCustomize().gearData.perfect;

        gameAnimations.Join(uiJudgement.Name);
        gameAnimations.Join(uiCombo.Name);
        Debug.Log(uiAccuracy);
        Debug.Log(data);
    }

    public void Clear() // 초기화
    {
        data = new ScoreData();
        data.judgeText = Enum.GetNames(typeof(JudgeType));
        data.judgeColor = new Color[3] { GameManager.Instance.GetPlayerCustomize().gearData.personalColor, Color.yellow, Color.red };
        uiJudgement.SetText("");
        uiCombo.SetText("");
        uiScore.SetText("0");
        uiAccuracy.SetText("0.00%");
    }

    public void SetScore()
    {
        uiJudgement.SetColor(data.judgeColor[(int)data.judge]);
        if ( data.judgeText[(int)data.judge] == "NICE")
        {
            uiJudgement.SetText(perfectString);
            if (data.combo > data.maxCombo)
                data.maxCombo = data.combo;
        }
        else
            uiJudgement.SetText(data.judgeText[(int)data.judge]);
        if ( data.combo == 0 )
            uiCombo.SetText("");
        else
            uiCombo.SetText($"{data.combo}");
        uiScore.SetText($"{data.Score}");

        gameAnimations.PlayPop(uiCombo.Name, uiCombo.rect);
        gameAnimations.PlayBig(uiJudgement.Name, uiJudgement.rect);

        if (data.judgeText[(int)data.judge] == "MISS")
            hpManager.AdjustHP(-10);
        else
            hpManager.AdjustHP(1);
    }

    public void SetAccuracy()
    {
        uiAccuracy.SetText($"{data.Accuracy.ToString("F2")}%"); // "F2" = 소수점 둘째자리까지 출력
    }

    void Update()
    {
        if ( uiProgress != null )
        {
            float value = Mathf.Clamp((1 / (float)musicPlayer.Length * (float)musicPlayer.progressTime), 0f, 1f);
            uiProgress.slider.value = value;
        }
    }

    private void OnDestroy() {
        instance = null;
    }
}
