using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SongItem : MonoBehaviour
{
    Button button;

    Image background;
    Image image;
    TextMeshProUGUI title;
    TextMeshProUGUI singer;
    Image favorite;
    Image hard;
    Image normal;
    Image easy;

    [SerializeField] private RankSpriteSet rankSpriteSet;

    GameManager gameManager;
    MusicListManager musicListManager;
    MusicScroll musicScroll;

    int select = 0;

    void Start()
    {
        background = gameObject.GetComponent<Image>();
        image = transform.GetChild(0).GetComponent<Image>();
        title = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        singer = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        favorite = transform.GetChild(3).GetComponent<Image>();
        easy = transform.GetChild(6).GetComponent<Image>();
        normal = transform.GetChild(5).GetComponent<Image>();
        hard = transform.GetChild(4).GetComponent<Image>();

        select = 0;

        button = GetComponent<Button>();

        gameManager = GameManager.Instance;
        musicListManager = MusicListManager.Instance;
        musicScroll = MusicScroll.Instance;
    }

    public void MusicSelected()
    {
        // if ( SceneManager.GetActiveScene().name == "MusicSelect" )
        //     GameManager.Instance.InGame();
        // else if ( SceneManager.GetActiveScene().name == "EditSelect" )
        //     GameManager.Instance.InEdit();
        select += 1;
        if(select == 1)
        {
            gameManager.title = title.text;
            musicScroll.MusicMouseSelect();
            background.color = Category.Instance.colors[Category.Instance.GetCategoryIndex(MusicListManager.Instance.sheets[GameManager.Instance.title].Artist)];
            background.color = new Color(background.color.r, background.color.g, background.color.b, 0.8352941f);
            MusicDetail.Instance.UpdateDetail();
            MusicPlayer.Instance.PlayPreview(GameManager.Instance.title);
        }
        else if (select == 2)
        {
            if (gameManager.title == "Random")
                title.text = MusicScroll.Instance.GetRandomSong();
            gameManager.title = title.text;
            gameManager.SongSelected();
        }



    }

    public void OnDeselect()
    {
        select = 0;
        background.color = new Color(0, 0, 0, 0.8352941f);
    }

    public void setInfo(string title)
    {
        image.sprite = musicListManager.sheets[title].img;
        this.title.text = title;
        singer.text = musicListManager.sheets[title].Artist;
        if (PlayerPrefs.HasKey("Favorite") && PlayerPrefs.GetString("Favorite").Contains(this.title.text + "-" + singer.text))
            favorite.gameObject.SetActive(true);
        else
            favorite.gameObject.SetActive(false);
        
        UpdateRanks(title);
    }

    public void SetColor()
    {
        background.color = Category.Instance.colors[Category.Instance.GetCategoryIndex(MusicListManager.Instance.sheets[GameManager.Instance.title].Artist)];
        background.color = new Color(background.color.r, background.color.g, background.color.b, 0.8352941f);
    }

    private void OnDestroy()
    {
        select = 0;
    }

    public string GetTitle()
    {
        return title.text;
    }

    public string GetArtist()
    {
        return singer.text;
    }

    public int GetSelect()
    {
        return select;
    }

    public void RefreshRanks()
    {
        UpdateRanks(title.text);
    }

    private void UpdateRanks(string songId)
    {
        Image[] rankImages = { easy, normal, hard };

        for (int i = 0; i < rankImages.Length; i++)
        {
            PlayRecord record = gameManager.Records.GetRecord(songId, i, gameManager.GameMode);
            RankType rank = record == null ? RankType.None : RankCalculator.GetRank(record.bestAccuracy);
            if (rank == RankType.None)
            {
                rankImages[i].rectTransform.sizeDelta = new Vector2(35f, 35f);
                rankImages[i].color = new Color(0.1981132f, 0.1981132f, 0.1981132f, 0.8235294f);
            }
            else
            {
                rankImages[i].rectTransform.sizeDelta = new Vector2(100f, 100f);
                rankImages[i].color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            }
            rankImages[i].sprite = rankSpriteSet.GetSprite(rank);
        }
    }
}
