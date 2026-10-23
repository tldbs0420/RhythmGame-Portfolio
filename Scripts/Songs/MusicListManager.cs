using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class MusicListManager : MonoBehaviour
{
    static MusicListManager instance;
    public static MusicListManager Instance
    {
        get
        {
            return instance;
        }
    }

    public Dictionary<string, Sheet> sheets = new Dictionary<string, Sheet>();

    SheetLoader sheetLoader;
    SheetParser sheetParser;

    const int member = 14;

    List<string>[] musicList = new List<string>[member];
    // 0 즐겨찾기 1 전체 2 스텔라이브 3 아야츠노 유니 4 아이리 칸나
    // 5 아라하시 타비 6 네네코 마시로 7 시라유키 히나 8 아카네 리제
    // 9 텐코 시부키 10 아오쿠모 린 11 하나코 나나 12 유즈하 리코 13 강지


    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void Init()
    {
        StartCoroutine(IEInit());
    }

    IEnumerator IEInit()
    {
        SheetLoader.Instance.Init();
        yield return new WaitUntil(() => SheetLoader.Instance.bLoadFinish == true);

        musicList[0] = new List<string>();
        for (int i = 2; i < member; i++)
        {
            musicList[i] = new List<string>();
            musicList[i].Add("Random");
        }

        musicList[1] = sheets.Keys.ToList();
        musicList[1].Remove("Random");
        if (PlayerPrefs.HasKey("Favorite"))
        {
            string favoriteList = PlayerPrefs.GetString("Favorite");
            Debug.Log(favoriteList);
            musicList[0].Insert(0, "Random");
            for (int i = 0; i < musicList[1].Count; i++)
            {
                Debug.Log(sheets[musicList[1][i]].Title + "-" + sheets[musicList[1][i]].Artist);
                if (favoriteList.Contains(sheets[musicList[1][i]].Title + "-" + sheets[musicList[1][i]].Artist))
                {
                    musicList[0].Add(sheets[musicList[1][i]].Title);
                }
            }
        }
        musicList[1].Insert(0, "Random");
        for (int i = 0; i < musicList[1].Count; i++)
        {
            string title = musicList[1][i];
            if (title == "Random") continue;

            string artist = sheets[title].Artist ?? string.Empty;
            bool isCliche = artist.Contains("클리셰");
            bool isUniverse = artist.Contains("유니버스");

            if (artist.Contains(",") || artist.Contains("스텔라이브") || isCliche || isUniverse)
                musicList[2].Add(title);
            if (artist.Contains("아야츠노 유니"))
                musicList[3].Add(title);
            if (artist.Contains("아이리 칸나"))
                musicList[4].Add(title);
            if (artist.Contains("아라하시 타비") || isUniverse)
                musicList[5].Add(title);
            if (artist.Contains("네네코 마시로") || isUniverse)
                musicList[6].Add(title);
            if (artist.Contains("시라유키 히나") || isUniverse)
                musicList[7].Add(title);
            if (artist.Contains("아카네 리제") || isUniverse)
                musicList[8].Add(title);
            if (artist.Contains("텐코 시부키") || isCliche)
                musicList[9].Add(title);
            if (artist.Contains("아오쿠모 린") || isCliche)
                musicList[10].Add(title);
            if (artist.Contains("하나코 나나") || isCliche)
                musicList[11].Add(title);
            if (artist.Contains("유즈하 리코") || isCliche)
                musicList[12].Add(title);
            if (artist.Contains("강지"))
                musicList[13].Add(title);
        }
    }

    public void SetAsFavorite(string musicName, string artistName)
    {
        if (musicName == "Random") return;
        if (!PlayerPrefs.HasKey("Favorite"))
        {
            musicList[0].Add("Random");
            musicList[0].Add(musicName);
            PlayerPrefs.SetString("Favorite", musicName + "-" + artistName + "|");
        }
        else
        {
            string favorites = PlayerPrefs.GetString("Favorite");
            if (favorites.Contains(musicName + "-" + artistName))
            {
                favorites = favorites.Replace(musicName + "-" + artistName + "|", "");
                Debug.Log(favorites);
                if (favorites == "")
                {
                    PlayerPrefs.DeleteKey("Favorite");
                    musicList[0].Remove("Random");
                }
                else PlayerPrefs.SetString("Favorite", favorites);
                musicList[0].Remove(musicName);
            }
            else
            {
                favorites += musicName + "-" + artistName + "|";
                PlayerPrefs.SetString("Favorite", favorites);
                musicList[0].Add(musicName);
            }
            //Debug.Log(PlayerPrefs.GetString("Favorite"));
        }
    }

    public string[] GetMusicList(int i)
    {
        return musicList[i].ToArray();
    }

    public bool isListEmpty(int i)
    {
        return musicList[i].Count == 0;
    }
}
