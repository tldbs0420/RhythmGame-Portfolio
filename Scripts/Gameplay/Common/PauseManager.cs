using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    static PauseManager instance;
    public static PauseManager Instance => instance;

    Button Resume;
    Button Restart;
    Button SoundSetting;
    Button Exit;

    [SerializeField] private TextMeshProUGUI countdownText;

    private Coroutine coCountdown;

    public bool IsPaused { get; private set; }

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void Init()
    {
        Resume = transform.GetChild(0).GetComponent<Button>();
        Restart = transform.GetChild(1).GetComponent<Button>();
        SoundSetting = transform.GetChild(2).GetComponent<Button>();
        Exit = transform.GetChild(3).GetComponent<Button>();

        IsPaused = false;
        coCountdown = null;

        SetMenuVisible(true);
        countdownText.gameObject.SetActive(false);

        gameObject.SetActive(false);
    }

    public void PauseGame()
    {
        if (IsPaused) return;
        IsPaused = true;

        SetMenuVisible(true);
        countdownText.gameObject.SetActive(false);
        gameObject.SetActive(true);

        MusicPlayer.Instance.Pause();
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        if (!IsPaused || coCountdown != null) return;

        coCountdown = StartCoroutine(IECountdown());
    }

    private IEnumerator IECountdown()
    {
        SetMenuVisible(false);
        countdownText.gameObject.SetActive(true);

        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSecondsRealtime(1f);
        }

        MusicPlayer.Instance.UnPause();
        Time.timeScale = 1f;

        IsPaused = false;
        coCountdown = null;

        countdownText.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    private void SetMenuVisible(bool visible)
    {
        Resume.gameObject.SetActive(visible);
        Restart.gameObject.SetActive(visible);
        SoundSetting.gameObject.SetActive(visible);
        Exit.gameObject.SetActive(visible);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        GameManager.Instance.StopCheckEnd();
        MusicPlayer.Instance.Stop();
        GameManager.Instance.SongSelected();
    }

    public void ToMusicSelect()
    {
        Time.timeScale = 1f;

        GameManager.Instance.StopCheckEnd();
        MusicPlayer.Instance.Stop();
        GameManager.Instance.PlaySelect();
    }

    private void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }
}