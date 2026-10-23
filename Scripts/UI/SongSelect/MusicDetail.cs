using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class MusicDetail : MonoBehaviour
{
    static MusicDetail instance;
    public static MusicDetail Instance
    {
        get
        {
            return instance;
        }
    }

    Image Thumbnail = null;
    public Image[] DifficultyImage;
    public Image ScoreBoardBackground;
    TextMeshProUGUI Title = null;
    TextMeshProUGUI Singer = null;
    TextMeshProUGUI Original = null;
    TextMeshProUGUI BPM = null;
    TextMeshProUGUI NoteBy = null;
    TextMeshProUGUI Combo = null;
    TextMeshProUGUI Rate = null;
    TextMeshProUGUI Score = null;
    Image Rank = null;

    Image GameMode;
    public Sprite[] GameMode_Images;
    [SerializeField] private RankSpriteSet rankSpriteSet;

    [SerializeField] private GameObject gearPreview;
    [SerializeField] private GameObject[] notePreviews = new GameObject[4];

    GameManager gameManager;
    MusicListManager musicListManager;

    Color[] colors = new Color[4]{
        new Color(0, 1, 0.1724083f, 0.85f), 
        new Color(1, 0.4980392f, 0, 0.85f), 
        new Color(1, 0, 0, 0.85f),
        new Color(1, 1, 1, 0.85f)
    };



    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void Init()
    {
        gameManager = GameManager.Instance;
        musicListManager = MusicListManager.Instance;

        Thumbnail = transform.GetChild(0).GetComponent<Image>();
        Title = transform.GetChild(4).GetComponent<TextMeshProUGUI>();
        Singer = transform.GetChild(5).GetComponent<TextMeshProUGUI>();
        Original = transform.GetChild(6).GetComponent<TextMeshProUGUI>();
        BPM = transform.GetChild(7).GetComponent<TextMeshProUGUI>();
        NoteBy = transform.GetChild(8).GetComponent<TextMeshProUGUI>();
        Combo = transform.GetChild(9).GetComponent<TextMeshProUGUI>();
        Rate = transform.GetChild(10).GetComponent<TextMeshProUGUI>();
        Score = transform.GetChild(11).GetComponent<TextMeshProUGUI>();
        Rank = transform.GetChild(12).GetComponent<Image>();
        GameMode = transform.GetChild(13).GetComponent<Image>();

        RefreshCustomizePreview();

        if (GameManager.Instance.GetSceneName() == "MusicSelect")
        {
            if (PlayerPrefs.GetInt("LastGameMode") == 0)
                GameMode.sprite = GameMode_Images[0];
            else if (PlayerPrefs.GetInt("LastGameMode") == 1)
                GameMode.sprite = GameMode_Images[1];
        }
    }

    public void UpdateDetail()
    {
        Thumbnail.sprite = musicListManager.sheets[gameManager.title].img;
        Title.text = gameManager.title;
        Singer.text = musicListManager.sheets[gameManager.title].Artist;
        Original.text = musicListManager.sheets[gameManager.title].Original;
        BPM.text = musicListManager.sheets[gameManager.title].BPM.ToString();
        
        RefreshRecord();
        UpdateColor();

        MusicScroll.Instance.handle.color = Category.Instance.colors[Category.Instance.GetCategoryIndex(MusicListManager.Instance.sheets[GameManager.Instance.title].Artist)];
    }

    public void UpdateColor()
    {
        ScoreBoardBackground.color = Category.Instance.colors[Category.Instance.GetCategoryIndex(MusicListManager.Instance.sheets[GameManager.Instance.title].Artist)];
    }

    public void SelectDifficultyImage(int i)
    {
        if ( i == (int)GameManager.DifficultyType.EASY)
        {
            DifficultyImage[0].color = colors[0];
            DifficultyImage[1].color = colors[3];
            DifficultyImage[2].color = colors[3];
        }
        else if ( i == (int)GameManager.DifficultyType.NORMAL)
        {
            DifficultyImage[0].color = colors[3];
            DifficultyImage[1].color = colors[1];
            DifficultyImage[2].color = colors[3];
        }
        else if ( i == (int)GameManager.DifficultyType.HARD)
        {
            DifficultyImage[0].color = colors[3];
            DifficultyImage[1].color = colors[3];
            DifficultyImage[2].color = colors[2];
        }
        RefreshRecord();
    }

    public void DifficultySet(int i)
    {
        GameManager.Instance.Difficulty = i;
        PlayerPrefs.SetInt("Difficulty", i);
        SelectDifficultyImage(i);
    }

    public void ChangeMode()
    {
        if (GameManager.Instance.GameMode == 0)
        {
            GameManager.Instance.GameMode = 1;
            GameMode.sprite = GameMode_Images[1];
        }
        else if (GameManager.Instance.GameMode == 1)
        {
            GameManager.Instance.GameMode = 0;
            GameMode.sprite = GameMode_Images[0];
        }

        PlayerPrefs.SetInt("LastGameMode", gameManager.GameMode);

        RefreshRecord();
        MusicScroll.Instance.RefreshVisibleRanks();
    }

    private void RefreshRecord()
    {
        PlayRecord record = gameManager.Records.GetRecord(gameManager.title, gameManager.Difficulty, gameManager.GameMode);

        if (record == null)
        {
            Combo.text = "0";
            Rate.text = "0%";
            Score.text = "0";
            Rank.gameObject.SetActive(false);
            return;
        }

        Combo.text = record.bestCombo.ToString();
        Rate.text = $"{record.bestAccuracy:F2}%";
        Score.text = record.bestScore.ToString();

        Rank.gameObject.SetActive(true);
        RankType rank = RankCalculator.GetRank(record.bestAccuracy);
        Rank.sprite = rankSpriteSet.GetSprite(rank);
    }

    public void RefreshCustomizePreview()
    {
        PlayerCustomize customize = GameManager.Instance.GetPlayerCustomize();
        if (customize == null) return;

        if (gearPreview != null && customize.gearData != null)
        {
            Image image = gearPreview.GetComponent<Image>();

            if (image != null)
                image.sprite = customize.gearData.thumbNail;
        }

        LineData[] lines =
        {
            customize.line1,
            customize.line2,
            customize.line3,
            customize.line4
        };
        for (int i = 0; i < 4; i++)
        {
            if (notePreviews == null || i >= notePreviews.Length)
                break;

            if (notePreviews[i] == null || lines[i] == null)
                continue;

            Image image = notePreviews[i].GetComponent<Image>();

            if (image != null) 
            image.sprite = lines[i].note;
        }
    }

    private void OnDestroy() {
        instance = null;
    }
}
