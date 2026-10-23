using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SelectInputManager : MonoBehaviour
{
    public GameObject[] gameObjects;
    [SerializeField] private InfoPanel infoPanel;

    static SelectInputManager instance;
    public static SelectInputManager Instance
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

    public void Escape(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (infoPanel.IsOpen)
        {
            infoPanel.Close();
            return;
        }

        GameManager.Instance.ReturnMain();
    }

    public void ScrollUp ( InputAction.CallbackContext context )
    {
        if (infoPanel.IsOpen) return;
        if (context.started)
        {
            MusicScroll.Instance.ListUp();
        }
    }

    public void ScrollDown ( InputAction.CallbackContext context )
    {
        if (infoPanel.IsOpen) return;
        if (context.started)
        {
            MusicScroll.Instance.ListDown();
        }
    }

    public void DifficultyUp ( InputAction.CallbackContext context )
    {
        if (infoPanel.IsOpen) return;
        if (context.started)
        {
            GameManager.Instance.DifficultyUp();
        }
    }
    public void DifficultyDown ( InputAction.CallbackContext context )
    {
        if (infoPanel.IsOpen) return;
        if (context.started)
        {
            GameManager.Instance.DifficultyDown();
        }
    }

    public void NextCategory ( InputAction.CallbackContext context )
    {
        if (infoPanel.IsOpen) return;
        if(context.started)
        {
            Category.Instance.Next();
        }
    }

    public void PrevCategory ( InputAction.CallbackContext context )
    {
        if (infoPanel.IsOpen) return;
        if(context.started)
        {
            Category.Instance.Prev();
        }
    }

    public void AddFavorite(InputAction.CallbackContext context)
    {
        if (infoPanel.IsOpen) return;
        if (context.started)
        {
            MusicScroll.Instance.MusicFavorite();
        }
    }

    public void ChangeMode(InputAction.CallbackContext context)
    {
        if (infoPanel.IsOpen) return;
        if (context.started)
        {
            MusicDetail.Instance.ChangeMode();
        }
    }

    public void SelectButton( InputAction.CallbackContext context )
    {
        if (infoPanel.IsOpen) return;
        if(context.started)
        {
            MusicScroll.Instance.MusicSelected();
        }
    }

    public void Option( InputAction.CallbackContext context )
    {
        if (infoPanel.IsOpen) return;
        if(context.started)
        {
            GameManager.Instance.OpenOptionInSelect();
        }
    }

    public void EnterOption()
    {
        foreach( GameObject obj in gameObjects)
        {
            obj.SetActive(false);
        }
    }

    public void ReturnSelelct()
    {
        foreach( GameObject obj in gameObjects)
        {
            obj.SetActive(true);
        }
    }

    public void InCustomize( InputAction.CallbackContext context )
    {
        if (infoPanel.IsOpen) return;
        if(context.started)
        {
            GameManager.Instance.InCustomize();
        }
    }

    public void InCustomizeButton()
    {
        GameManager.Instance.InCustomize();
    }

    public void ToggleInfo(InputAction.CallbackContext context)
    {
        if (!context.started)
            return;

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "MusicSelect")
            return;

        infoPanel.Toggle();
    }
}
