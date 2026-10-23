using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class SettingManager : MonoBehaviour
{
    static SettingManager instance;
    public static SettingManager Instance
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



    string[] displayMode = { "전체화면", "경계없는 창모드", "창모드" };
    public int currentDisplayMode;

    List<Resolution> resolutions169 = new List<Resolution>();
    public int currentResolution;


    string[] Hz = { "60", "75", "90", "120", "144", "165", "180", "240", "제한 없음" };
    public int currentHz;

    //디버그용
    string nowRes = "2560 x 1440";
    string nowMode = "전체화면";
    int nowHz = 165;

    public PlayerInput playerInput = null;

    public InputActionAsset playerInputAsset = null;

    private InputAction line1;
    private InputAction line2;
    private InputAction line3;
    private InputAction line4;

    public GameObject waitingForInput = null;
    private InputActionRebindingExtensions.RebindingOperation rebindingOperation = null;

    private int sync = 0;

    private int currentMaster;
    private int currentBGM;
    private int currentSFX;
    private int currentHitSFX;

    private const string KeyBindingSaveKey = "KeyBindings";

    public void Init()
    {
        LoadKeyBindings(playerInputAsset);

        Resolution[] resolutions = Screen.resolutions;
        foreach (Resolution res in resolutions)
        {
            float aspectRatio = (float)res.width / res.height;
            if (Mathf.Approximately(aspectRatio, 16f / 9f))
            {
                resolutions169.Add(res);
            }
        }
        resolutions = null;

        nowRes = PlayerPrefs.GetString("Resolution");
        Debug.Log(nowRes);
        int width = Int32.Parse(nowRes.Split(" x ")[0]);
        int height = Int32.Parse(nowRes.Split(" x ")[1]);

        currentResolution = 0;
        for (int i = 0; i < resolutions169.Count; i++)
        {
            if (resolutions169[i].width == width && resolutions169[i].height == height)
            {
                currentResolution = i;
                break;
            }
        }
        SettingUIManager.Instance.SetText(0, 1, resolutions169[currentResolution].width + " x " + resolutions169[currentResolution].height);

        nowMode = PlayerPrefs.GetString("DisplayMode");
        currentDisplayMode = 0;
        for (int i = 0; i < displayMode.Length; i++)
        {
            if (displayMode[i] == nowMode)
            {
                currentDisplayMode = i;
                break;
            }
        }
        SettingUIManager.Instance.SetText(0, 0, displayMode[currentDisplayMode]);


        nowHz = PlayerPrefs.GetInt("Hz");
        currentHz = 0;
        string testHz;
        if (nowHz == 0)
            testHz = "제한 없음";
        else
            testHz = nowHz.ToString();

        for (int i = 0; i < Hz.Length; i++)
        {
            if (Hz[i] == testHz)
            {
                currentHz = i;
                break;
            }
        }
        if (testHz != "제한 없음")
            testHz += " Hz";
        SettingUIManager.Instance.SetText(0, 2, testHz);

        waitingForInput.SetActive(false);
        line1 = playerInputAsset.FindAction("NoteLine1");
        line2 = playerInputAsset.FindAction("NoteLine2");
        line3 = playerInputAsset.FindAction("NoteLine3");
        line4 = playerInputAsset.FindAction("NoteLine4");
        SettingUIManager.Instance.SetText(1, 0, InputControlPath.ToHumanReadableString(line1.bindings[0].effectivePath,
                    InputControlPath.HumanReadableStringOptions.OmitDevice));
        SettingUIManager.Instance.SetText(1, 1, InputControlPath.ToHumanReadableString(line2.bindings[0].effectivePath,
                    InputControlPath.HumanReadableStringOptions.OmitDevice));
        SettingUIManager.Instance.SetText(1, 2, InputControlPath.ToHumanReadableString(line3.bindings[0].effectivePath,
                    InputControlPath.HumanReadableStringOptions.OmitDevice));
        SettingUIManager.Instance.SetText(1, 3, InputControlPath.ToHumanReadableString(line4.bindings[0].effectivePath,
                    InputControlPath.HumanReadableStringOptions.OmitDevice));

        sync = PlayerPrefs.GetInt("Sync");
        SettingUIManager.Instance.SetText(1, 4, sync.ToString() + " ms");

        currentMaster = PlayerPrefs.GetInt("MasterVolume");
        currentBGM = PlayerPrefs.GetInt("BGMVolume");
        currentSFX = PlayerPrefs.GetInt("SFXVolume");
        currentHitSFX = PlayerPrefs.GetInt("HitSFXVolume");
        SettingUIManager.Instance.SetText(2, 0, currentMaster.ToString());
        SettingUIManager.Instance.SetText(2, 1, currentBGM.ToString());
        SettingUIManager.Instance.SetText(2, 2, currentSFX.ToString());
        SettingUIManager.Instance.SetText(2, 3, currentHitSFX.ToString());
    }

    private bool CheckDuplicateBindings(InputAction action)
    {
        InputBinding newBinding = action.bindings[0];
        foreach (InputBinding binding in action.actionMap.bindings)
        {
            if (binding.action == newBinding.action)
            {
                continue;
            }

            if (binding.effectivePath == newBinding.effectivePath)
            {
                Debug.Log("Dup found");
                return true;
            }
        }
        return false;
    }

    public string SettingChange(string SettingName, int dir)
    {
        string result = "";
        switch (SettingName)
        {
            case "DisplayMode":
                result = ChangeDisplayMode(dir);
                break;
            case "Resolution":
                result = ChangeResolution(dir);
                break;
            case "Hz":
                result = ChangeHz(dir);
                break;
            case "SyncTimingItem":
                result = ChangeSync(dir);
                break;
            case "Master":
                result = SoundCal(dir, 0);
                break;
            case "BGM":
                result = SoundCal(dir, 1); ;
                break;
            case "SFX":
                result = SoundCal(dir, 2); ;
                break;
            case "HITSFX":
                result = SoundCal(dir, 3); ;
                break;
        }

        return result;
    }

    public void SettingEnter(string SettingName, Action<string> onComplete)
    {
        switch (SettingName)
        {
            case "Line1":
                StartRebinding(0, onComplete);
                break;
            case "Line2":
                StartRebinding(1, onComplete);
                break;
            case "Line3":
                StartRebinding(2, onComplete);
                break;
            case "Line4":
                StartRebinding(3, onComplete);
                break;
        }
    }
    public string SoundSliderChange(string SettingName, int num)
    {
        string result = "";
        switch (SettingName)
        {
            case "Master":
                result = SoundChange(num, 0);
                break;
            case "BGM":
                result = SoundChange(num, 1);
                break;
            case "SFX":
                result = SoundChange(num, 2);
                break;
            case "HITSFX":
                result = SoundChange(num, 3);
                break;
        }
        return result;
    }

    string ChangeDisplayMode(int dir)
    {
        if (dir == 1)
        {
            if (currentDisplayMode < displayMode.Length - 1)
            {
                currentDisplayMode += 1;
                if (currentDisplayMode == 1)
                    Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                else if (currentDisplayMode == 2)
                    Screen.fullScreenMode = FullScreenMode.Windowed;
                PlayerPrefs.SetString("DisplayMode", displayMode[currentDisplayMode]);
            }
            return displayMode[currentDisplayMode];
        }
        else if (dir == -1)
        {
            if (currentDisplayMode > 0)
            {
                currentDisplayMode -= 1;
                if (currentDisplayMode == 1)
                    Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                else if (currentDisplayMode == 0)
                    Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                PlayerPrefs.SetString("DisplayMode", displayMode[currentDisplayMode]);
            }
            return displayMode[currentDisplayMode];
        }
        return "Error!";
    }

    string ChangeResolution(int dir)
    {
        if (dir == 1)
        {
            if (currentResolution < resolutions169.Count - 1)
            {
                currentResolution += 1;
                Screen.SetResolution(resolutions169[currentResolution].width, resolutions169[currentResolution].height, Screen.fullScreenMode);
                PlayerPrefs.SetString("Resolution", resolutions169[currentResolution].width + " x " + resolutions169[currentResolution].height);
            }
            return resolutions169[currentResolution].width + " x " + resolutions169[currentResolution].height;
        }
        else if (dir == -1)
        {
            if (currentResolution > 0)
            {
                currentResolution -= 1;
                Screen.SetResolution(resolutions169[currentResolution].width, resolutions169[currentResolution].height, Screen.fullScreenMode);
                PlayerPrefs.SetString("Resolution", resolutions169[currentResolution].width + " x " + resolutions169[currentResolution].height);
            }
            return resolutions169[currentResolution].width + " x " + resolutions169[currentResolution].height;
        }
        return "Error!";
    }

    string ChangeHz(int dir)
    {
        if (dir == 1)
        {
            if (currentHz < Hz.Length - 1)
            {
                currentHz += 1;
                int setHz;
                if (Hz[currentHz] == "제한 없음")
                {
                    setHz = 0;
                }
                else
                {
                    setHz = Int32.Parse(Hz[currentHz]);
                }
                Application.targetFrameRate = setHz;
                PlayerPrefs.SetInt("Hz", setHz);
            }
            string result;
            result = Hz[currentHz];
            if (result == "제한 없음")
            {
                return result;
            }
            else
            {
                result += " Hz";
                return result;
            }
        }
        else if (dir == -1)
        {
            if (currentHz > 0)
            {
                currentHz -= 1;
                int setHz;
                setHz = Int32.Parse(Hz[currentHz]);
                Application.targetFrameRate = setHz;
                PlayerPrefs.SetInt("Hz", setHz);
            }
            return Hz[currentHz] + " Hz";
        }
        return "Error!";
    }

    public static void SaveKeyBindings(InputActionAsset actions)
    {
        if (actions == null) return;

        PlayerPrefs.SetString(
            KeyBindingSaveKey,
            actions.SaveBindingOverridesAsJson()
        );

        PlayerPrefs.Save();
    }

    public static void LoadKeyBindings(InputActionAsset actions)
    {
        if (actions == null) return;

        string json = PlayerPrefs.GetString(KeyBindingSaveKey, "");
        if (string.IsNullOrEmpty(json)) return;

        try
        {
            actions.LoadBindingOverridesFromJson(json);
        }
        catch (Exception e)
        {
            Debug.LogWarning("키 설정을 불러오지 못했습니다: " + e.Message);
        }
    }

    public void StartRebinding(int lineNum, Action<string> onComplete)
    {
        waitingForInput.SetActive(true);
        playerInput.SwitchCurrentActionMap("KeyBinding");

        string returnStr = "";

        InputAction selectedLine = null;

        switch (lineNum)
        {
            case 0:
                selectedLine = line1;
                break;
            case 1:
                selectedLine = line2;
                break;
            case 2:
                selectedLine = line3;
                break;
            case 3:
                selectedLine = line4;
                break;
        }

        InputBinding prevBinding = selectedLine.bindings[0];

        rebindingOperation = selectedLine.PerformInteractiveRebinding()
            .WithControlsExcluding("Mouse")
            .WithCancelingThrough("<Keyboard>/escape")
            .OnMatchWaitForAnother(0.1f)
            .OnCancel(operation =>
            {
                waitingForInput.SetActive(false);

                playerInput.SwitchCurrentActionMap("Options");
                returnStr = InputControlPath.ToHumanReadableString(selectedLine.bindings[0].effectivePath,
                    InputControlPath.HumanReadableStringOptions.OmitDevice);
                rebindingOperation.Dispose();
                onComplete?.Invoke(returnStr);
            })
            .OnComplete(operation =>
            {
                waitingForInput.SetActive(false);

                if (CheckDuplicateBindings(selectedLine))
                {
                    rebindingOperation.Dispose();
                    selectedLine.ApplyBindingOverride(0, prevBinding);
                    StartRebinding(lineNum, onComplete);
                    return;
                }

                playerInput.SwitchCurrentActionMap("Options");
                SaveKeyBindings(playerInputAsset);
                Debug.Log("Start");
                returnStr = InputControlPath.ToHumanReadableString(selectedLine.bindings[0].effectivePath,
                    InputControlPath.HumanReadableStringOptions.OmitDevice);
                Debug.Log(returnStr);
                rebindingOperation.Dispose();
                onComplete?.Invoke(returnStr);
            })
            .Start();
    }

    string ChangeSync(int dir)
    {
        if (dir == 1)
        {
            if (sync < 225)
            {
                sync += 1;
                PlayerPrefs.SetInt("Sync", sync);
            }
            return sync + " ms";
        }
        else if (dir == -1)
        {
            if (sync > -225)
            {
                sync -= 1;
                PlayerPrefs.SetInt("Sync", sync);
            }
            return sync + " ms";
        }
        return "Error!";
    }

    public string SoundCal(int dir, int sound)
    {
        if (dir == 1)
        {
            switch (sound)
            {
                case 0:
                    if (currentMaster < 100)
                    {
                        currentMaster += 1;
                    }
                    return currentMaster.ToString();
                case 1:
                    if (currentBGM < 100)
                    {
                        currentBGM += 1;
                    }
                    return currentBGM.ToString();

                case 2:
                    if (currentSFX < 100)
                    {
                        currentSFX += 1;
                    }
                    return currentSFX.ToString();
                case 3:
                    if (currentHitSFX < 100)
                    {
                        currentHitSFX += 1;
                    }
                    return currentHitSFX.ToString();
            }
        }
        else if (dir == -1)
        {
            switch (sound)
            {
                case 0:
                    if (currentMaster > 0)
                    {
                        currentMaster -= 1;
                    }
                    return currentMaster.ToString();
                case 1:
                    if (currentBGM > 0)
                    {
                        currentBGM -= 1;
                    }
                    return currentBGM.ToString();
                case 2:
                    if (currentSFX > 0)
                    {
                        currentSFX -= 1;
                    }
                    return currentSFX.ToString();
                case 3:
                    if (currentHitSFX > 0)
                    {
                        currentHitSFX -= 1;
                    }
                    return currentHitSFX.ToString();
            }
        }
        return "Error!";
    }
    public string SoundChange(int num, int sound)
    {
        switch (sound)
        {
            case 0:
                currentMaster = num;
                PlayerPrefs.SetInt("MasterVolume", currentMaster);
                MusicPlayer.Instance.UpdateMusicVolume();
                return currentMaster.ToString();
            case 1:
                currentBGM = num;
                PlayerPrefs.SetInt("BGMVolume", currentBGM);
                MusicPlayer.Instance.UpdateMusicVolume();
                return currentBGM.ToString();
            case 2:
                currentSFX = num;
                PlayerPrefs.SetInt("SFXVolume", currentSFX);
                MusicPlayer.Instance.UpdateMusicVolume();
                return currentSFX.ToString();
            case 3:
                currentHitSFX = num;
                PlayerPrefs.SetInt("HitSFXVolume", currentHitSFX);
                MusicPlayer.Instance.UpdateMusicVolume();
                return currentHitSFX.ToString();
        }
        return "Error!";
    }

    public void ToGearSetting()
    {
        GameManager.Instance.InGearPosition();
    }


}
