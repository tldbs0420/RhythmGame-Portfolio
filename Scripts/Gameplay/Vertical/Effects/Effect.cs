using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class Effect : MonoBehaviour
{
    static Effect instance;
    public static Effect Instance { get { return instance; } }


    void Awake()
    {
        if ( instance == null )
            instance = this;

        for(int i = 0; i < 4; i++)
        {
            longNoteHiders[i].gameObject.SetActive(false);
            keyEffects[i].gameObject.SetActive(false);
        }
    }

    public GameObject[] longNoteHiders = new GameObject[4]; // 롱노트 마스크
    private LongNoteHider longNoteHider; // 조작용 longNoteHider

    public GameObject[] keyEffects = new GameObject[4]; // 키입력 효과


    public void ShowHider( int i )
    {
        longNoteHider = longNoteHiders[i].GetComponent<LongNoteHider>();
        longNoteHider.ShowHider();
    }

    public void HideHider( int i )
    {
        longNoteHider = longNoteHiders[i].GetComponent<LongNoteHider>();
        longNoteHider.HideHider();
    }

    public void MoveHider ( int i )
    {
        longNoteHider = longNoteHiders[i].GetComponent<LongNoteHider>();
        longNoteHider.MoveHider();
    }

    public void SetHiderSpeed() // Hider 속도 동기화
    {
        for ( int i = 0; i < 4; i++ )
        {
            longNoteHider = longNoteHiders[i].GetComponent<LongNoteHider>();
            longNoteHider.speed = NoteGenerator.Instance.noteSpeed;
        }
    }

    public void KeyEffectOn ( int i )
    {
        keyEffects[i].SetActive(true);
    }

    public void KeyEffectOff ( int i )
    {
        keyEffects[i].SetActive(false);
    }
}
