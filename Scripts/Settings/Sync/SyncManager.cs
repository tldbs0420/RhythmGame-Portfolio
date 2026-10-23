using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class SyncManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI PressButtonPlease;
    [SerializeField] private TextMeshProUGUI ErrorRecent;
    [SerializeField] private TextMeshProUGUI ErrorAverage;

    private int count = 0;
    private int totalError = 0;
    private int averageError = 0;
    
    [SerializeField] private InputActionAsset playerInputAsset = null;
    private InputAction line1;

    static SyncManager instance;
    public static SyncManager Instance
    {
        get
        {
            return instance;
        }
    }
    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void Init()
    {
        SyncSheetParser.Instance.ParsePlease();
        SyncSheetParser.Instance.sheet.Init();
        MusicPlayer.Instance.InsertSyncSetting("hit_keyboard");
        SyncJudgment.Instance.Init();
        MusicPlayer.Instance.Play();
        SyncNoteGenerator.Instance.StartGen();

        count = 0;
        totalError = 0;
        averageError = 0;

        ErrorRecent.text = "0 ms";
        ErrorAverage.text = "0 ms";

        SettingManager.LoadKeyBindings(playerInputAsset);
        line1 = playerInputAsset.FindAction("NoteLine1");
        string key = InputControlPath.ToHumanReadableString(line1.bindings[0].effectivePath, 
                    InputControlPath.HumanReadableStringOptions.OmitDevice);

        PressButtonPlease.text = "음악에 맞춰 " + key + "를 눌러주세요!";

        StartCoroutine(IECheckEnd());
    }

    public void CalcError(int error)
    {
        ErrorRecent.text = error + " ms";

        count += 1;
        totalError += error;
        averageError = (int)((float)totalError / count);

        //Debug.Log(count);
        //Debug.Log(error);
        //Debug.Log(totalError);
        //Debug.Log(averageError);

        ErrorAverage.text = averageError + "ms";
    }

    IEnumerator IECheckEnd()
    {
        while (true){
            if (!MusicPlayer.Instance.IsPlaying())
                break;
            yield return new WaitForSeconds(1f);
        }
        if (count > 0)
        {
            PlayerPrefs.SetInt("Sync", averageError);
            PlayerPrefs.Save();
        }

        GameManager.Instance.PlaySelect();
    }
}
