using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SyncInputManager : MonoBehaviour
{
    SyncJudgment judgement;
    MusicPlayer musicPlayer;
    SyncNoteGenerator noteGenerator;

    [SerializeField] private PlayerInput playerInput;

    void Start()
    {
        judgement = SyncJudgment.Instance;
        musicPlayer = MusicPlayer.Instance;
        noteGenerator = SyncNoteGenerator.Instance;

        if (playerInput == null) playerInput = GetComponent<PlayerInput>();
        if (playerInput != null)
            SettingManager.LoadKeyBindings(playerInput.actions);
        else
            Debug.LogError("노트 입력용 PlayerInput을 연결해주세요.", this);
    }

    public void OnNoteLine1 (InputAction.CallbackContext context)
    {
        if (context.started)
        {
            judgement.Judge(0);
            Effect.Instance.KeyEffectOn(0);
        }
        else if (context.canceled)
        {
            Effect.Instance.KeyEffectOff(0);
        }
    }

    public void StartSong ( InputAction.CallbackContext context) // 임시 노래 시작 (디버그용)
    {
        if ( context.started )
        {
            Sheet.Instance.Init();
            Judgement.Instance.Init();
            //musicPlayer.Insert();
            UIController.Instance.Init();
            Score.Instance.Init();
            Score.Instance.Clear();
            HPManager.Instance.Init();
            musicPlayer.Play();
            noteGenerator.StartGen();
            Effect.Instance.SetHiderSpeed();
        }

    }

    public void SpeedUp ( InputAction.CallbackContext context )
    {
        if (context.started)
        {
            SyncNoteGenerator.Instance.ChangeNoteSpeed(+0.1f);
        }
    }

    public void SpeedDown ( InputAction.CallbackContext context )
    {
        if (context.started)
        {
            SyncNoteGenerator.Instance.ChangeNoteSpeed(-0.1f);
        }
    }

    public void Escape ( InputAction.CallbackContext context )
    {
        if (context.started)
        {
            
        }
    }
}
