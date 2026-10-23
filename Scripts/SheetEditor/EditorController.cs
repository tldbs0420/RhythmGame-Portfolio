using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EditorController : MonoBehaviour
{
    static EditorController instance;
    public static EditorController Instance { get { return instance; } }

    public bool isCtrl = false;
    private float scrollValue; // 스크롤 확인
    Coroutine coCtrl;

    Camera cam;
    Vector3 worldPos;

    GameObject selectedNoteObject;
    Vector3 selectedGridPosition;
    Vector3 lastSelectedGridPosition; // 롱노트 만들기용
    Transform headTemp;


    EditInputManager editInputManager;
    EditNoteGenerator editNoteGenerator;
    Editor editor;
    MusicPlayer musicPlayer;
    Sheet sheet;

    public Action<int> GridSnapListener;

    private int selectedLine = 0;
    private float barInterval = 0.5f; // 그리드 줄 간격 ( Prefab 참조 )

    public int longNoteMakingCount = 0; // 롱노트 헤드/꼬리 판별

    private bool isDispose = false; // 배치 가능 여부

    public bool isShortNoteActivated;
    public bool isLongNoteActivated;


    private void Awake()
    {
        if ( instance == null )
            instance = this;
    }

    void Start()
    {
        cam = Camera.main;

        editInputManager = FindObjectOfType<EditInputManager>();
        editor = Editor.Instance;
        musicPlayer = MusicPlayer.Instance;
        editNoteGenerator = EditNoteGenerator.Instance;
    }

    void Update()
    {
        Vector2 mousePos = editInputManager.mousePos;
        worldPos = cam.ScreenToWorldPoint(mousePos);

        RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero); // 광선을 worldPos위치에 cam의 정면방향(화면방향)으로 2f길이만큼 쏜다.
        if ( hit.collider == null )
        {
            isDispose = false;
            selectedNoteObject = null;
            return;
        }
        else if (hit.collider.CompareTag("Note"))
        {
            isDispose = false; // 설치 불가
            selectedNoteObject = hit.transform.gameObject;
        }
        else if (hit.collider.CompareTag("LongNote"))
        {
            isDispose = false; // 설치 불가
            selectedNoteObject = hit.transform.gameObject;
        }
        else if ( hit.collider.CompareTag("Grid")) //grid인 경우
        {
            int beat = int.Parse(hit.transform.name.Split('_')[1]);
            //grid를 생성할 때 line 형식을 $"Line_{i}"로 했었다. (GridObject.cs 참조) 따라서 몇번째 박자(비트)인지 확인하기 위해 뒤에 숫자를 때옴.
            int index = hit.transform.parent.GetComponent<GridObject>().index; //몇번째 마디
            float y = hit.transform.TransformDirection(hit.transform.position).y; //로컬 포지션을 월드 포지션으로?

            //  n번 줄 | index번 그리드 | beat 비트
            if ( worldPos.x < -1.81f && worldPos.x > -3.27f)
                selectedLine = 0;
            else if ( worldPos.x < 0f && worldPos.x > -1.81f)
                selectedLine = 1;
            else if ( worldPos.x < 1.81f && worldPos.x > 0f)
                selectedLine = 2;
            else if ( worldPos.x < 3.27f && worldPos.x > 1.81f)
                selectedLine = 3;

            if ( longNoteMakingCount == 0 )
                headTemp = hit.transform;


            selectedGridPosition = new Vector3(editNoteGenerator.linePos[selectedLine], y, 0f);
            isDispose = true;
        }
    }




    public void Space()
    {
        editor.Play();
    }

    public void MouseBtn(string btnName)
    {
        if ( btnName == "leftButton")
        {

            if ( isDispose )
            {

                if ( isLongNoteActivated )
                {

                    if ( longNoteMakingCount == 0 )
                    {
                        lastSelectedGridPosition = selectedGridPosition;

                        editNoteGenerator.DisposeNoteLong(longNoteMakingCount, new Vector3[] { lastSelectedGridPosition, selectedGridPosition }); // 머리, 꼬리를 같은 위치로 생성시켜 눈속임
                        longNoteMakingCount = 1;
                    }
                    else if ( longNoteMakingCount == 1 )
                    {
                        Vector3 tailPosition = selectedGridPosition;
                        tailPosition.x = lastSelectedGridPosition.x; // 다른곳에 찍어도 x 고정
                        lastSelectedGridPosition.y = headTemp.TransformDirection(headTemp.transform.position).y;
                        
                        if ( lastSelectedGridPosition.y < tailPosition.y )
                            editNoteGenerator.DisposeNoteLong(longNoteMakingCount, new Vector3[] { lastSelectedGridPosition, tailPosition });
                        else
                            editNoteGenerator.DisposeNoteLong(longNoteMakingCount, new Vector3[] { tailPosition, lastSelectedGridPosition });
                        
                        longNoteMakingCount = 0;
                    }
                }


                else if ( isShortNoteActivated )
                    editNoteGenerator.DisposeNoteShort(selectedGridPosition);
            }
        }




        else if ( btnName == "rightButton" )
        {
            Debug.Log(selectedNoteObject.ToString());
            if ( selectedNoteObject != null )
            {
                if ( selectedNoteObject.transform.CompareTag("LongNote") )
                    selectedNoteObject.transform.parent.gameObject.SetActive(false);
                else if ( selectedNoteObject.transform.CompareTag("Note") )
                    selectedNoteObject.SetActive(false);
            }
        }
    }

    public void Scroll ( float value )
    {
        scrollValue = value;

        if ( !isCtrl )
        {
            MusicPlayer.State state = musicPlayer.state;
            if(state == MusicPlayer.State.Playing)
                musicPlayer.Pause();
            float scrollsnap = Editor.Instance.Snap;
            if ( scrollValue > 0 )
            {
                editor.objects.transform.position += Vector3.down * Editor.Instance.Snap * barInterval;
                musicPlayer.MovePosition(MusicListManager.Instance.sheets[GameManager.Instance.title].BeatPerMilliSec * Editor.Instance.Snap );
            }
            else if ( scrollValue < 0 )
            {
                editor.objects.transform.position += Vector3.up * Editor.Instance.Snap * barInterval;
                musicPlayer.MovePosition(-MusicListManager.Instance.sheets[GameManager.Instance.title].BeatPerMilliSec * Editor.Instance.Snap );
            }
            if (state == MusicPlayer.State.Playing)
                musicPlayer.UnPause();
        }
    }

    public void Ctrl()
    {
        if (coCtrl != null )
            StopCoroutine(coCtrl);
        coCtrl = StartCoroutine(IEWaitMouseWheel());
    }

    IEnumerator IEWaitMouseWheel()
    {
        while ( isCtrl )
        {
            if ( scrollValue > 0 )
            {
                editor.Snap /= 2;
                GridSnapListener.Invoke(Editor.Instance.Snap);
            }
            else if ( scrollValue < 0 )
            {
                editor.Snap *= 2;
                GridSnapListener.Invoke(Editor.Instance.Snap);
            }
            scrollValue = 0;

            yield return null;
        }
    }
}
