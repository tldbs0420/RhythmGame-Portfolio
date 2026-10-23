using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour
{
    static ResultManager instance;
    public static ResultManager Instance
    {
        get
        {
            return instance;
        }
    }
    [SerializeField] private RankSpriteSet rankSpriteSet;

    Image Thumbnail = null;
    TextMeshProUGUI PerfectName = null;
    TextMeshProUGUI Music = null;
    TextMeshProUGUI Singer = null;
    TextMeshProUGUI Playmode = null;
    TextMeshProUGUI Accuracy = null;
    TextMeshProUGUI PerfectNum = null;
    TextMeshProUGUI GoodNum = null;
    TextMeshProUGUI MissNum = null;
    TextMeshProUGUI MaxCombo = null;
    TextMeshProUGUI ScoreNum = null;
    Image Rank = null;

    GameManager gameManager = null;
    MusicListManager musicListManager = null;
    Score score = null;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void Init()
    {
        gameManager = GameManager.Instance;
        musicListManager = MusicListManager.Instance;
        score = Score.Instance;

        Thumbnail = transform.GetChild(0).GetComponent<Image>();
        PerfectName = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        Music = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        Singer = transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        Playmode = transform.GetChild(4).GetComponent<TextMeshProUGUI>();
        Accuracy = transform.GetChild(5).GetComponent<TextMeshProUGUI>();
        PerfectNum = transform.GetChild(6).GetComponent<TextMeshProUGUI>();
        GoodNum = transform.GetChild(7).GetComponent<TextMeshProUGUI>();
        MissNum = transform.GetChild(8).GetComponent<TextMeshProUGUI>();
        MaxCombo = transform.GetChild(9).GetComponent<TextMeshProUGUI>();
        ScoreNum = transform.GetChild(10).GetComponent<TextMeshProUGUI>();
        Rank = transform.GetChild(11).GetComponent<Image>();
    }

    public void UpdateResult()
    {
        ScoreData data = score.data;
        var sheet = musicListManager.sheets[gameManager.title];

        // 곡 정보
        Thumbnail.sprite = sheet.img;
        Music.text = gameManager.title;
        Singer.text = sheet.Artist;
        PerfectName.text = score.perfectString;

        Playmode.text = gameManager.GameMode == 0 ? "Vertical Mode" : "Dance Mode";

        // 이번 플레이의 결과
        Accuracy.text = $"{data.Accuracy:F2}%";
        PerfectNum.text = data.nice.ToString();
        GoodNum.text = data.good.ToString();
        MissNum.text = (data.miss + data.fastMiss).ToString();
        MaxCombo.text = data.maxCombo.ToString();
        ScoreNum.text = data.Score.ToString();

        RankType rank = RankCalculator.GetRank(data.Accuracy);
        Rank.sprite = rankSpriteSet.GetSprite(rank);
        Rank.preserveAspect = true;
    }

}
