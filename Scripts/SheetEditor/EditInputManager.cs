using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EditInputManager : MonoBehaviour
{
    GridGenerator gridGenerator;
    EditNoteGenerator editNoteGenerator;
    EditorController editorController;
    Editor editor;
    Sheet sheet;

    public Vector2 mousePos;
    void Start()
    {
        editNoteGenerator = EditNoteGenerator.Instance;
        editorController = EditorController.Instance;
        editor = Editor.Instance;
        gridGenerator = GameObject.Find("Grids").GetComponent<GridGenerator>();
    }

    void Update()
    {
        mousePos = Mouse.current.position.ReadValue();
    }

    public void OnMouseBtn(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            editorController.MouseBtn(context.control.name);                
        }
    }

    public void OnMouseWheel(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            editorController.Scroll(context.ReadValue<float>());
        }
    }

    public void OnSpace(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            editor.Play();
        }
    }

    public void MakeGrid ( InputAction.CallbackContext context) // 임시 에디터 시작 (디버그용)
    {
        if ( context.started )
        {
            MusicListManager.Instance.sheets["낮에 뜨는 달"].Init();
            GameManager.Instance.title = "낮에 뜨는 달";
            MusicPlayer.Instance.Insert("낮에 뜨는 달", "hit_keyboard");
            UIController.Instance.Init();
            gridGenerator.Init();
            editNoteGenerator.Gen2();
            editor.Init();
            
        }

    }

    public void OnCtrl(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            editorController.isCtrl = true;
            editorController.Ctrl();
        }
        else if (context.canceled)
        {
            editorController.isCtrl = false;
        }
    }
    
    public void Escape ( InputAction.CallbackContext context )
    {
        if (context.started)
        {
            MusicPlayer.Instance.Stop();
            GameManager.Instance.EditSelect();
        }
    }
}
