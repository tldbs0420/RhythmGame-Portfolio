using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FMODUnity;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }

    public enum GameState
    {
        Main,
        PlaySelect,
        EditSelect,
        Play,
        Result,
        Edit,
    }
    public string title;
    public GameState state = GameState.Main;
    public RecordRepository Records { get; private set; }

    //public Dictionary<string, Sheet> sheets = new Dictionary<string, Sheet>();

    public GameObject[] backGrounds;

    List<GameObject> backgroundList = new List<GameObject>();

    Coroutine coPlaying;

    public enum DifficultyType
    {
        EASY = 0,
        NORMAL = 1,
        HARD = 2
    }

    private int difficulty;
    public int Difficulty
    {
        get { return difficulty; }
        set
        {
            difficulty = value;
        }
    }

    private int gameMode;
    public int GameMode
    {
        get { return gameMode; }
        set
        {
            gameMode = value; // 0 Vertical 1 Dance
        }
    }

    float speed = 3.0f;
    public float Speed
    {
        get
        {
            return speed;
        }
        set
        {
            speed = Mathf.Clamp(value, 1.0f, 9.9f);
        }
    }
    float danceSpeed = 3.0f;
    public float DanceSpeed
    {
        get
        {
            return danceSpeed;
        }
        set
        {
            danceSpeed = Mathf.Clamp(value, 1.0f, 9.9f);
        }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(instance.gameObject);
            instance = this;
        }

        Records = new RecordRepository();
        Records.Load();
    }

    void Start()
    {
        StartCoroutine(IEInit());
    }

    public void PlaySelect()
    {
        StartCoroutine(IEPlaySelect());
    }

    public void InGame()
    {
        StartCoroutine(IEInGame());
    }

    public void InGameDance()
    {
        StartCoroutine(IEInGameDance());
    }

    public void EditSelect()
    {
        StartCoroutine(IEEditSelect());
    }

    public void InEdit()
    {
        StartCoroutine(IEInEdit());
    }

    public void ReturnMain()
    {
        StartCoroutine(IEReturnMain());
    }

    public void OpenOptionInSelect()
    {
        StartCoroutine(IEOpenOptionInSelect());
    }

    public void OptionToSelect()
    {
        StartCoroutine(IEOptionToSelect());
    }

    public void InSync()
    {
        StartCoroutine(IEInSync());
    }

    public void InCustomize()
    {
        StartCoroutine(IEInCustomize());
    }

    public void CustomizeToSelect()
    {
        StartCoroutine(IECustomizeToSelect());
    }

    public void InGearPosition()
    {
        StartCoroutine(IEInGearPosition());
    }

    public void OutGearPosition()
    {
        StartCoroutine(IEOutGearPosition());
    }

    public void ReturnSelect()
    {
        PlaySelect();
    }

    public void SongSelected()
    {
        if (SceneManager.GetActiveScene().name == "MusicSelect")
        {
            if (GameMode == 0)
                InGame();
            else if (gameMode == 1)
                InGameDance();
        }
        else if ((SceneManager.GetActiveScene().name == "EditSelect"))
            InEdit();
        else
        {
            if (GameMode == 0)
                InGame();
            else if (gameMode == 1)
                InGameDance();
        }
    }

    public void StopCheckEnd()
    {
        if (coPlaying != null)
        {
            StopCoroutine(coPlaying);
            coPlaying = null;
        }
    }

    public void DifficultyUp()
    {
        if (Difficulty < 2)
        {
            Difficulty += 1;
            PlayerPrefs.SetInt("Difficulty", Difficulty);
        }
        MusicDetail.Instance.SelectDifficultyImage(GameManager.Instance.Difficulty);
    }
    public void DifficultyDown()
    {
        if (Difficulty > 0)
        {
            Difficulty -= 1;
            PlayerPrefs.SetInt("Difficulty", Difficulty);
        }
        MusicDetail.Instance.SelectDifficultyImage(GameManager.Instance.Difficulty);
    }

    public string GetSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }

    IEnumerator IEInit()
    {
        // if ( backgroundList.Count == 0 )
        // {
        //     for (int num = 0; num < GameObject.Find("Background").transform.childCount; num++)
        //         backgroundList.Add(GameObject.Find("Background").transform.GetChild(num));
        // }
        int i = Random.Range(0, backGrounds.Length);
        backGrounds[i].SetActive(true);
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(MusicPlayer.Instance);
        DontDestroyOnLoad(MusicListManager.Instance);
        MusicListManager.Instance.Init();
        yield return new WaitUntil(() => SheetLoader.Instance.bLoadFinish == true);

        // PlayerPrefs.DeleteAll();
        if (PlayerPrefs.HasKey("LastPlayedSong"))
            title = PlayerPrefs.GetString("LastPlayedSong");
        else
        {
            title = "8-bit game music";
            PlayerPrefs.SetString("LastPlayedSong", title);
        }

        if (PlayerPrefs.HasKey("Difficulty"))
            Difficulty = PlayerPrefs.GetInt("Difficulty");
        else
        {
            Difficulty = 1;
            PlayerPrefs.SetInt("Difficulty", 1);
        }

        if (!PlayerPrefs.HasKey("LastCategory"))
        {
            PlayerPrefs.SetString("LastCategory", "전체");
        }

        if (!PlayerPrefs.HasKey("LastGameMode"))
        {
            PlayerPrefs.SetInt("LastGameMode", 0);
        }
        else
        {
            GameMode = PlayerPrefs.GetInt("LastGameMode");
        }

        if (PlayerPrefs.HasKey("Speed"))
            Speed = PlayerPrefs.GetFloat("Speed");
        else
        {
            Speed = 5.0f;
            PlayerPrefs.SetFloat("Speed", 5.0f);
        }
        if (PlayerPrefs.HasKey("DanceSpeed"))
            DanceSpeed = PlayerPrefs.GetFloat("DanceSpeed");
        else
        {
            DanceSpeed = 3.0f;
            PlayerPrefs.SetFloat("DanceSpeed", 3.0f);
        }

        if (!PlayerPrefs.HasKey("DisplayMode"))
        {
            PlayerPrefs.SetString("DisplayMode", "전체화면");
        }
        if (!PlayerPrefs.HasKey("Resolution"))
        {
            PlayerPrefs.SetString("Resolution", "1920 x 1080");
        }
        if (!PlayerPrefs.HasKey("Hz"))
        {
            PlayerPrefs.SetInt("Hz", 144);
        }

        if (!PlayerPrefs.HasKey("Line1"))
        {
            PlayerPrefs.SetString("Line1", "D");
        }
        if (!PlayerPrefs.HasKey("Line2"))
        {
            PlayerPrefs.SetString("Line2", "F");
        }
        if (!PlayerPrefs.HasKey("Line3"))
        {
            PlayerPrefs.SetString("Line3", "J");
        }
        if (!PlayerPrefs.HasKey("Line4"))
        {
            PlayerPrefs.SetString("Line4", "K");
        }


        if (!PlayerPrefs.HasKey("Sync"))
        {
            PlayerPrefs.SetInt("Sync", 0);
        }

        if (!PlayerPrefs.HasKey("MasterVolume"))
        {
            PlayerPrefs.SetInt("MasterVolume", 100);
        }
        if (!PlayerPrefs.HasKey("BGMVolume"))
        {
            PlayerPrefs.SetInt("BGMVolume", 100);
        }
        if (!PlayerPrefs.HasKey("SFXVolume"))
        {
            PlayerPrefs.SetInt("SFXVolume", 100);
        }
        if (!PlayerPrefs.HasKey("HitSFXVolume"))
        {
            PlayerPrefs.SetInt("HitSFXVolume", 100);
        }

        if (!PlayerPrefs.HasKey("GearPosition"))
            PlayerPrefs.SetFloat("GearPosition", -0.13f);
        if (!PlayerPrefs.HasKey("UIPosition"))
            PlayerPrefs.SetFloat("UIPosition", -585f);
        if (!PlayerPrefs.HasKey("HPPosition"))
            PlayerPrefs.SetFloat("HPPosition", -20f);
        if (!PlayerPrefs.HasKey("CharaPosition"))
            PlayerPrefs.SetFloat("CharaPosition", 4.17f);

        if (!PlayerPrefs.HasKey("GearTransparency"))
            PlayerPrefs.SetFloat("GearTransparency", 0.9882353f);
        if (!PlayerPrefs.HasKey("CharaTransparency"))
            PlayerPrefs.SetFloat("CharaTransparency", 1f);

        MusicPlayer.Instance.Init();
        MusicPlayer.Instance.InsertBGM("Akane Lize - BGM (Jazz Bass Music)");

        GetPlayerCustomize().Init();
    }

    IEnumerator IEPlaySelect()
    {
        if (SceneManager.GetActiveScene().name == "Main")
            yield return new WaitForSeconds(3f);

        if (SceneManager.GetActiveScene().name == "VerticalPlay")
        {
            if (coPlaying != null)
            {
                StopCoroutine(coPlaying);
                coPlaying = null;
            }
        }
        else if (SceneManager.GetActiveScene().name == "DancePlay")
        {
            if (coPlaying != null)
            {
                StopCoroutine(coPlaying);
                coPlaying = null;
            }
        }

        AsyncOperation asyncOper = SceneManager.LoadSceneAsync("MusicSelect");
        while (!asyncOper.isDone)
            yield return null;
        //ItemGenerator.Instance.Init();
        MusicDetail.Instance.Init();
        Category.Instance.Init();
        MusicScroll.Instance.SetSongsList(PlayerPrefs.GetString("LastCategory"));
        MusicScroll.Instance.Init();
        MusicDetail.Instance.SelectDifficultyImage(Difficulty);
    }

    IEnumerator IEInGame()
    {
        MusicListManager.Instance.sheets[title].SelectDifficulty(Difficulty);
        MusicListManager.Instance.sheets[title].Init();
        PlayerPrefs.SetString("LastPlayedSong", title);
        string lastCategory = Category.Instance.GetCategoryName();
        if (lastCategory == "즐겨찾기")
            if (!PlayerPrefs.HasKey("Favorite")
                || !PlayerPrefs.GetString("Favorite").Contains(title + "-" + MusicListManager.Instance.sheets[title].Artist))
            { lastCategory = "전체"; }
        PlayerPrefs.SetString("LastCategory", lastCategory);
        PlayerPrefs.SetInt("LastGameMode", GameMode);

        Debug.Log(MusicPlayer.Instance);
        MusicPlayer.Instance.ReleaseMusic();
        MusicPlayer.Instance.Insert(title, "hit_keyboard");

        AsyncOperation asyncOper = SceneManager.LoadSceneAsync("VerticalPlay");
        while (!asyncOper.isDone)
            yield return null;

        PauseManager.Instance.Init();
        UIController.Instance.Init();
        Score.Instance.Init();
        Score.Instance.Clear();
        HPManager.Instance.Init();
        Judgement.Instance.Init();
        MusicPlayer.Instance.Play();
        NoteGenerator.Instance.SetNoteSpeed(Speed);
        NoteGenerator.Instance.StartGen();
        Effect.Instance.SetHiderSpeed();

        coPlaying = StartCoroutine(IECheckEnd());
    }

    IEnumerator IEInGameDance()
    {
        MusicListManager.Instance.sheets[title].SelectDifficulty(Difficulty);
        MusicListManager.Instance.sheets[title].Init();
        PlayerPrefs.SetString("LastPlayedSong", title);
        string lastCategory = Category.Instance.GetCategoryName();
        if (lastCategory == "즐겨찾기")
            if (!PlayerPrefs.HasKey("Favorite")
                || !PlayerPrefs.GetString("Favorite").Contains(title + "-" + MusicListManager.Instance.sheets[title].Artist))
            { lastCategory = "전체"; }
        PlayerPrefs.SetString("LastCategory", lastCategory);
        PlayerPrefs.SetInt("LastGameMode", GameMode);

        Debug.Log(MusicPlayer.Instance);
        MusicPlayer.Instance.ReleaseMusic();
        MusicPlayer.Instance.Insert(title, "hit_keyboard");

        AsyncOperation asyncOper = SceneManager.LoadSceneAsync("DancePlay");
        while (!asyncOper.isDone)
            yield return null;

        PauseManager.Instance.Init();
        UIController.Instance.Init();
        Score.Instance.Init();
        Score.Instance.Clear();
        HPManager.Instance.Init();
        DanceJudgement.Instance.Init();
        MusicPlayer.Instance.Play();
        DanceNoteGenerator.Instance.StartGen();
        DanceEffect.Instance.SetHiderSpeed();

        coPlaying = StartCoroutine(IECheckEnd());
    }

    IEnumerator IECheckEnd()
    {
        while (true)
        {
            if (!MusicPlayer.Instance.IsPlaying())
                break;
            yield return new WaitForSeconds(1f);
        }

        // 명시적으로 중단한 플레이는 기록하지 않음
        if (MusicPlayer.Instance.state == MusicPlayer.State.Stop)
            yield break;
        ScoreData result = Score.Instance.data;
        Records.UpdateRecord(title, Difficulty, GameMode, result.Score, result.maxCombo, result.Accuracy);

        DontDestroyOnLoad(Score.Instance);

        AsyncOperation asyncOper = SceneManager.LoadSceneAsync("Result");
        while (!asyncOper.isDone)
            yield return null;

        ResultManager.Instance.Init();
        ResultManager.Instance.UpdateResult();
        MusicPlayer.Instance.InsertBGM("Arahashi Tabi - BGM (Bbungbbang Goat Music)");

        Destroy(Score.Instance.gameObject);
    }

    IEnumerator IEEditSelect()
    {
        if (SceneManager.GetActiveScene().name == "Main")
            yield return new WaitForSeconds(3f);


        AsyncOperation asyncOper = SceneManager.LoadSceneAsync("EditSelect");
        while (!asyncOper.isDone)
            yield return null;
        MusicDetail.Instance.Init();
        MusicScroll.Instance.SetSongsList(PlayerPrefs.GetString("LastCategory"));
        MusicScroll.Instance.Init();
        Category.Instance.Init();
        MusicDetail.Instance.SelectDifficultyImage(Difficulty);
    }

    IEnumerator IEInEdit()
    {
        Sheet sheet = MusicListManager.Instance.sheets[title];
        sheet.SelectDifficulty(Difficulty);
        sheet.Init();

        PlayerPrefs.SetString("LastPlayedSong", title);
        PlayerPrefs.SetString("LastCategory", Category.Instance.GetCategoryName());

        MusicPlayer.Instance.ReleaseMusic();
        MusicPlayer.Instance.Insert(title, "hit_keyboard");

        AsyncOperation asyncOper = SceneManager.LoadSceneAsync("Editor");
        while (!asyncOper.isDone)
            yield return null;

        UIController.Instance.Init();
        GameObject.Find("Grids").GetComponent<GridGenerator>().Init();
        EditNoteGenerator.Instance.Gen2();
        Editor.Instance.Init();
    }

    IEnumerator IEReturnMain()
    {
        MusicPlayer.Instance.ReleaseMusic();
        AsyncOperation asyncOper = SceneManager.LoadSceneAsync("Main");
        while (!asyncOper.isDone)
            yield return null;
    }

    IEnumerator IEOpenOptionInSelect()
    {
        AsyncOperation asyncOper = SceneManager.LoadSceneAsync("Setting", LoadSceneMode.Additive); //, LoadSceneMode.Additive
        while (!asyncOper.isDone)
            yield return null;
        SettingUIManager.Instance.Init();
        SettingManager.Instance.Init();
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Setting"));
        SelectInputManager.Instance.EnterOption();
    }

    IEnumerator IEOptionToSelect()
    {
        Scene selectScene = SceneManager.GetSceneByName("MusicSelect");
        if (!selectScene.IsValid() || !selectScene.isLoaded)
            selectScene = SceneManager.GetSceneByName("EditSelect");

        SceneManager.SetActiveScene(selectScene);
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName("Setting"));
        SelectInputManager.Instance.ReturnSelelct();
        yield return null;
    }

    IEnumerator IEInSync()
    {
        AsyncOperation asyncOper = SceneManager.LoadSceneAsync("SyncSetting");
        while (!asyncOper.isDone)
            yield return null;

        SyncManager.Instance.Init();
    }

    IEnumerator IEInCustomize()
    {
        AsyncOperation asyncOper = SceneManager.LoadSceneAsync("Customize", LoadSceneMode.Additive); //, LoadSceneMode.Additive
        while (!asyncOper.isDone)
            yield return null;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Customize"));
        SelectInputManager.Instance.EnterOption();
        CustomManager.Instance.Init();
    }

    IEnumerator IECustomizeToSelect()
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("MusicSelect"));
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName("Customize"));
        SelectInputManager.Instance.ReturnSelelct();
        MusicDetail.Instance.RefreshCustomizePreview();
        yield return null;
    }

    IEnumerator IEInGearPosition()
    {
        AsyncOperation asyncOper = SceneManager.LoadSceneAsync("GearPosition", LoadSceneMode.Additive); //, LoadSceneMode.Additive
        while (!asyncOper.isDone)
            yield return null;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("GearPosition"));
        CustomManager.Instance.EnterGearPosition();
    }

    IEnumerator IEOutGearPosition()
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Customize"));
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName("GearPosition"));
        yield return null;
        CustomManager.Instance.ExitGearPosition();
    }

    public PlayerCustomize GetPlayerCustomize()
    {
        return GetComponent<PlayerCustomize>();
    }

    public void SetSavedSpeed(float value)
    {
        Speed = value;
        PlayerPrefs.SetFloat("Speed", Speed);
    }

     public void SetSavedDanceSpeed(float value)
    {
        DanceSpeed = Mathf.Clamp(value, 1.0f, 9.9f);
        PlayerPrefs.SetFloat("DanceSpeed", DanceSpeed);
    }
}
