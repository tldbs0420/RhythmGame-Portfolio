using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Editor : MonoBehaviour
{

    static Editor instance;
    public static Editor Instance { get { return instance; } }

    UISlider slider = null;
    UIButton musicController = null;
    UIText timer = null;
    UIText musicSpeedInfo = null;
    UIText title = null;

    public Sprite[] sprites;

    public GameObject objects;
    Coroutine coMove;

    MusicPlayer musicPlayer;
    EditorController editorController;
    Sheet sheet;

    private int snap = 2; // 비트 쪼개기용 ( 1 - 64비트 2 - 32비트 ... )
    public int Snap
    {
        get { return snap; }
        set
        {
            snap = Mathf.Clamp(value, 1, 16);
        }
    }

    public int currentBar = 0; // 현재 마디
    public float offsetPosition;

    private float speed;
    private float musicSpeed = 1f;

    private int gridInterval = 32; // 그리드 높이 (Prefab 참조)

    private void Awake()
    {
        if ( instance == null )
            instance = this;
    }

    public void Init()
    {
        title = UIController.Instance.GetUI("Title").uiObject as UIText;
        slider = UIController.Instance.GetUI("ProgressBar").uiObject as UISlider;
        musicController = UIController.Instance.GetUI("Start").uiObject as UIButton;
        timer = UIController.Instance.GetUI("Time").uiObject as UIText;
        musicSpeedInfo = UIController.Instance.GetUI("SpeedInfo").uiObject as UIText;
        musicSpeed = 1f;

        title.SetText(GameManager.Instance.title);

        StartCoroutine(IEBarTimer());

        speed = gridInterval / MusicListManager.Instance.sheets[GameManager.Instance.title].BarPerSec;
        offsetPosition = speed * MusicListManager.Instance.sheets[GameManager.Instance.title].offset * 0.001f;
        objects.transform.position = offsetPosition * Vector3.up;
    }

    void Start()
    {
        musicPlayer = MusicPlayer.Instance;
        editorController = EditorController.Instance;
    }

    void Update()
    {
        if ( slider != null )
        {
            float value = Mathf.Clamp(1 / (float)musicPlayer.Length * (float)musicPlayer.progressTime, 0f, 1f);
            slider.slider.value = value;
        }
        
        if ( timer != null )
        {
            timer.SetText(TimeSpan.FromMilliseconds(musicPlayer.progressTime).ToString(@"mm\:ss\:fff"));
        }
    }

    public void Play()
    {
        if (musicPlayer.state == MusicPlayer.State.Playing || musicPlayer.state == MusicPlayer.State.Unpaused)
        {
            musicPlayer.Pause();
            musicController.SetImage(sprites[0]);
            if ( coMove != null )
                StopCoroutine(coMove);
        }
        else
        {
            if (musicPlayer.state == MusicPlayer.State.Stop)
                musicPlayer.Play();
            else if (musicPlayer.state == MusicPlayer.State.Paused)
                musicPlayer.UnPause();
            musicController.SetImage(sprites[1]);
            coMove = StartCoroutine(IEMove());
        }
    }

    public void Stop()
    {
        if ( coMove != null )
            StopCoroutine(coMove);

        objects.transform.position = new Vector3(0f, offsetPosition, 0f);
        musicPlayer.progressTime = 0;
        musicPlayer.Stop();
        musicController.SetImage(sprites[0]);
    }

    public void CalculateCurrentBar()
    {
        currentBar = (int)(musicPlayer.progressTime / MusicListManager.Instance.sheets[GameManager.Instance.title].BarPerMilliSec);
    }

    public void SpeedUp()
    {
        if (musicSpeed < 1.0f)
        {
            musicSpeed += 0.1f;
            musicSpeed = Mathf.Round(musicSpeed * 10f) / 10f;
            musicPlayer.SetMusicPitch(musicSpeed);
            //MusicPlayer.Instance.videoPlayer.playbackSpeed = musicSpeed;
            musicSpeedInfo.SetText(musicSpeed.ToString() + "x");
        }
    }
    public void SpeedDown()
    {
        if (musicSpeed > 0.5f)
        {
            musicSpeed -= 0.1f;
            musicSpeed = Mathf.Round(musicSpeed * 10f) / 10f;
            musicPlayer.SetMusicPitch(musicSpeed);
            //MusicPlayer.Instance.videoPlayer.playbackSpeed = musicSpeed;
            musicSpeedInfo.SetText(musicSpeed.ToString() + "x");
        }
    }

    IEnumerator IEBarTimer()
    {
        WaitForSeconds wait = new WaitForSeconds(0.1f);
        while ( true )
        {
            CalculateCurrentBar();
            yield return wait;
        }
    }

    IEnumerator IEMove()
    {
        while ( true )
        {
            objects.transform.position += Vector3.down * Time.deltaTime * speed * musicSpeed;
            yield return null;
        }
    }



    public void Play(UIObject uIObject)
    {
        Play();
    }

    public void Stop(UIObject uIObject)
    {
        Stop();
    }



    public void Progress() // 슬라이더 클릭 시 해당 위치로 이동
    {
        if ( slider != null )
        {
            float time = musicPlayer.Length * slider.slider.value;
            musicPlayer.progressTime = (uint)time;

            CalculateCurrentBar();

            float barPerTime = MusicListManager.Instance.sheets[GameManager.Instance.title].BarPerSec;
            float pos = (time * 0.001f / barPerTime) * gridInterval;

            objects.transform.position = new Vector3(0f, -pos + offsetPosition, 0f);
        }
    }

    public void SelectShortNote()
    {
        if ( editorController.longNoteMakingCount == 1 )
        {
            Debug.Log("안돼.");
        }
        else
        {
            if (editorController.isLongNoteActivated)
        {
            editorController.isLongNoteActivated = false;
        }
        editorController.isShortNoteActivated = !editorController.isShortNoteActivated;
        }
    }

    public void SelectLongNote()
    {
        if (editorController.isShortNoteActivated)
        {
            editorController.isShortNoteActivated = false;
        }
        editorController.isLongNoteActivated = !editorController.isLongNoteActivated;
    }

    public void FileSave()
    {
        FindObjectOfType<SheetSave>().Save();
    }
}
