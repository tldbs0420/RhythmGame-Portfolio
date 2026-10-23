using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DanceInputManager : MonoBehaviour
{
    DanceJudgement judgement = null;
    MusicPlayer musicPlayer;
    DanceNoteGenerator noteGenerator;

    [SerializeField] private PlayerInput playerInput;

    // Start is called before the first frame update
    void Start()
    {
        judgement = FindObjectOfType<DanceJudgement>();
        musicPlayer = MusicPlayer.Instance;
        noteGenerator = DanceNoteGenerator.Instance;

        if (playerInput == null) playerInput = GetComponent<PlayerInput>();
        if (playerInput != null)
            SettingManager.LoadKeyBindings(playerInput.actions);
        else
            Debug.LogError("노트 입력용 PlayerInput을 연결해주세요.", this);
    }

    public void OnNoteLine1(InputAction.CallbackContext context)
    {
        if (PauseManager.Instance.IsPaused) return;

        if (context.started)
        {
            judgement.Judge(0);
            Tabi.Active?.OnNotePressed(0);
        }
        else if (context.canceled)
        {
            judgement.CheckLongNote(0);
        }
    }

    public void OnNoteLine2(InputAction.CallbackContext context)
    {
        if (PauseManager.Instance.IsPaused) return;

        if (context.started)
        {
            judgement.Judge(1);
            Tabi.Active?.OnNotePressed(1);
        }
        else if (context.canceled)
        {
            judgement.CheckLongNote(1);
        }
    }

    public void OnNoteLine3(InputAction.CallbackContext context)
    {
        if (PauseManager.Instance.IsPaused) return;

        if (context.started)
        {
            judgement.Judge(2);
            Tabi.Active?.OnNotePressed(2);
        }
        else if (context.canceled)
        {
            judgement.CheckLongNote(2);
        }
    }

    public void OnNoteLine4(InputAction.CallbackContext context)
    {
        if (PauseManager.Instance.IsPaused) return;

        if (context.started)
        {
            judgement.Judge(3);
            Tabi.Active?.OnNotePressed(3);
        }
        else if (context.canceled)
        {
            judgement.CheckLongNote(3);
        }
    }

    public void StartSong ( InputAction.CallbackContext context) // 임시 노래 시작 (디버그용)
    {
        if ( context.started )
        {
            MusicListManager.Instance.sheets["낮에 뜨는 달"].Init();
            GameManager.Instance.title = "낮에 뜨는 달";
            MusicPlayer.Instance.Insert("낮에 뜨는 달", "hit_keyboard");
            DanceJudgement.Instance.Init();
            UIController.Instance.Init();
            Score.Instance.Init();
            Score.Instance.Clear();
            HPManager.Instance.Init();
            musicPlayer.Play();
            noteGenerator.StartGen();
            DanceEffect.Instance.SetHiderSpeed();
        }

    }

    public void SpeedUp ( InputAction.CallbackContext context )
    {
        if (context.started)
        {
            noteGenerator.ChangeNoteSpeed(+0.1f);

            UIText speedUI = UIController.Instance.find.Invoke("UI_Speed").uiObject as UIText;
            speedUI.SetText("Speed : " + DanceNoteGenerator.Instance.Speed.ToString("0.0"));
        }
    }

    public void SpeedDown ( InputAction.CallbackContext context )
    {
        if (context.started)
        {
            noteGenerator.ChangeNoteSpeed(-0.1f);

            UIText speedUI = UIController.Instance.find.Invoke("UI_Speed").uiObject as UIText;
            speedUI.SetText("Speed : " + noteGenerator.Speed.ToString("0.0"));
        }
    }

    public void Escape ( InputAction.CallbackContext context )
    {
        if (context.started)
        {
            PauseManager.Instance.PauseGame();
        }
    }

    public void StopMusic ( InputAction.CallbackContext context )
    {
        if (context.started)
        {
            MusicPlayer.Instance.Stop();
        }
    }
}
