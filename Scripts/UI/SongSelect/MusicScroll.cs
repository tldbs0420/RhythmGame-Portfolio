using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MusicScroll : MonoBehaviour
{
    public RectTransform contentPanelTransform;
    public Scrollbar scrollbar;
    public Image handle;
    float scrollDelta;

    //public VerticalLayoutGroup VLG;
    public SongItem[] buttons;

    static MusicScroll instance;
    public static MusicScroll Instance
    {
        get
        {
            return instance;
        }
    }

    private string[] songs = new string[] {
        "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15"
    };

    string select = "1";
    int highest;
    int lowest;
    public int mid;
    int selectIndex;

    bool scrollable = true;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void Init()
    {
        select = GameManager.Instance.title;
        highest = 0;
        lowest = 8;
        mid = 4;
        selectIndex = Array.IndexOf(songs, select);
        int finalIndex = 0;
        for (int i = 0; i < 9; i++)
        {
            finalIndex = (selectIndex + i - 4) % songs.Length;
            while (finalIndex < 0)
                finalIndex += songs.Length;
            buttons[i].setInfo(songs[finalIndex]);
        }
        buttons[mid].MusicSelected();
        MusicDetail.Instance.UpdateDetail();
        MusicPlayer.Instance.PlayPreview(select);

        contentPanelTransform.localPosition = new Vector3(contentPanelTransform.localPosition.x,
                ((buttons[0].gameObject.GetComponent<RectTransform>().rect.height + 5)),
            contentPanelTransform.localPosition.z);

        Canvas.ForceUpdateCanvases();
        scrollbar.numberOfSteps = songs.Length;
        scrollDelta = 1.0f / (scrollbar.numberOfSteps - 1);
        scrollbar.value = scrollDelta * selectIndex;
        scrollbar.size = 1.0f / scrollbar.numberOfSteps;
    }

    public void UpdateList()
    {
        SetSongsList(Category.Instance.GetCategoryName());
        
        if (SceneManager.GetActiveScene().name == "EditSelect")  select = songs[0];
        else select = songs[1];

        GameManager.Instance.title = select;
        buttons[mid].SetColor();
        
        selectIndex = Array.IndexOf(songs, select);
        int finalIndex;
        int j = highest;
        for ( int i = 0; i < 9; i++ )
        {
            finalIndex = (selectIndex + i - 4 ) % songs.Length;
            while (finalIndex < 0)
                finalIndex += songs.Length; 
            buttons[j].setInfo(songs[finalIndex]);
            j += 1;
            if (j > 8)
                j=0;
        }

        MusicDetail.Instance.UpdateDetail();
        MusicPlayer.Instance.PlayPreview(select);

        scrollbar.numberOfSteps = songs.Length;
        scrollDelta = 1.0f / (scrollbar.numberOfSteps - 1);
        scrollbar.value = scrollDelta * selectIndex;
        scrollbar.size = 1f / scrollbar.numberOfSteps;
    }

    public void ListDown()
    {
        if (scrollable)
        {
            buttons[highest].gameObject.GetComponent<RectTransform>().SetAsLastSibling();
            //Debug.Log("button" + highest + "moved down");
            buttons[mid].OnDeselect();


            selectIndex += 1;
            if (selectIndex > songs.Length - 1)
                selectIndex = 0;
            mid += 1;
            if (mid > 8)
                mid = 0;
            //Debug.Log("selectIndex = " + selectIndex);
            buttons[mid].MusicSelected();

            int finalIndex = (selectIndex + 4) % songs.Length;
            //Debug.Log("finalIndex = " + finalIndex);
            buttons[highest].setInfo(songs[finalIndex]);
            //Debug.Log("song : " + songs[finalIndex]);

            highest += 1;
            if (highest > 8)
                highest = 0;
            //Debug.Log("highest = " + highest);

            lowest += 1;
            if (lowest > 8)
                lowest = 0;
            //Debug.Log("lowest = " + lowest);
            scrollbar.value = scrollDelta * selectIndex;
        }
    }

    public void ListUp()
    {
        if (scrollable)
        {
            buttons[lowest].gameObject.GetComponent<RectTransform>().SetAsFirstSibling();
            // Debug.Log("button" + highest + "moved up");
            buttons[mid].OnDeselect();

            //buttons[highest].gameObject.GetComponent<RectTransform>().anchoredPosition3D -= new Vector3(0, height * 8, 0);
            selectIndex -= 1;
            if (selectIndex < 0)
                selectIndex = songs.Length - 1;
            mid -= 1;
            if (mid < 0)
                mid = 8;
            // Debug.Log("selectIndex = " + selectIndex);
            buttons[mid].MusicSelected();
            MusicPlayer.Instance.PlayPreview(GameManager.Instance.title);

            int finalIndex = (selectIndex - 4) % songs.Length;
            while (finalIndex < 0)
                finalIndex += songs.Length;
            // Debug.Log("finalIndex = " + finalIndex);
            buttons[lowest].setInfo(songs[finalIndex]);
            // Debug.Log("song : " + songs[finalIndex]);

            highest -= 1;
            if (highest < 0)
                highest = 8;
            //Debug.Log("highest = " + highest);

            lowest -= 1;
            if (lowest < 0)
                lowest = 8;
            // Debug.Log("lowest = " + lowest);
            scrollbar.value = scrollDelta * selectIndex;
        }
    }

    public void SetSongsList(string name)
    {
        //int index = Category.Instance.GetCategoryIndex(name);
        songs = MusicListManager.Instance.GetMusicList(Category.Instance.GetCategoryIndex(name));
        if (GameManager.Instance.GetSceneName() == "EditSelect")
        {
            List<String> test = songs.ToList();
            test.RemoveAt(0);
            songs = test.ToArray();
        }
    }

    public void MusicMouseSelect()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i].GetTitle() != GameManager.Instance.title && buttons[i].GetSelect() == 1)
            {
                buttons[i].OnDeselect();
                break;
            }
        }
    }

    public void MusicSelected()
    {
        if (scrollable)
            buttons[mid].MusicSelected();
    }

    public void MusicFavorite()
    {
        MusicListManager.Instance.SetAsFavorite(buttons[mid].GetTitle(), buttons[mid].GetArtist());
        for (int i = 0; i < 9; i++)
        {
            buttons[i].setInfo(buttons[i].GetTitle());
        }
    }

    public void RefreshVisibleRanks()
    {
        foreach (SongItem button in buttons)
        {
            if (button != null)
                button.RefreshRanks();
        }
    }

    public void SetScrollable(bool tmp)
    {
        scrollable = tmp;
    }

    public string GetRandomSong()
    {
        return songs[UnityEngine.Random.Range(1, songs.Length)];
    }

    private void OnDestroy() {
        instance = null;
    }
}
