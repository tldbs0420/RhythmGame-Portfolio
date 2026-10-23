using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanceEffect : MonoBehaviour
{
    static DanceEffect instance;
    public static DanceEffect Instance { get { return instance; } }

    void Awake()
    {
        if ( instance == null )
            instance = this;

        for(int i = 0; i < 4; i++)
        {
            longNoteHider = longNoteHiders[i].GetComponent<DanceLongNoteHider>();
            if ( i == 0 || i == 1 )
                longNoteHider.moveDirection = 1;
            else
                longNoteHider.moveDirection = -1;
            longNoteHiders[i].gameObject.SetActive(false);
        }
    }

    public GameObject[] longNoteHiders = new GameObject[4]; // 롱노트 마스크
    public DanceLongNoteHider longNoteHider; // 조작용 longNoteHider


    public void ShowHider( int i )
    {
        longNoteHider = longNoteHiders[i].GetComponent<DanceLongNoteHider>();
        longNoteHider.ShowHider();
    }

    public void HideHider( int i )
    {
        longNoteHider = longNoteHiders[i].GetComponent<DanceLongNoteHider>();
        longNoteHider.HideHider();
    }

    public void MoveHider ( int i )
    {
        longNoteHider = longNoteHiders[i].GetComponent<DanceLongNoteHider>();
        longNoteHider.MoveHider();
    }

    public void SetHiderSpeed() // Hider 속도 동기화
    {
        for ( int i = 0; i < 4; i++ )
        {
            longNoteHider = longNoteHiders[i].GetComponent<DanceLongNoteHider>();
            longNoteHider.speed = DanceNoteGenerator.Instance.noteSpeed;
        }
    }
}
