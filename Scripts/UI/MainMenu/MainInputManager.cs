using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class MainInputManager : MonoBehaviour
{
    public GameObject[] mainButtons;
    public GameObject[] canvases;
    [SerializeField] private GameObject quitPanel;
    [SerializeField] private GameObject pressanykey;
    bool isPressed = false;

    private bool IsQuitOpen => quitPanel.activeSelf;

    private EventSystem popupEventSystem;
    private GameObject previousSelected;
    private bool previousNavigationEvents;

    private void Awake()
    {
        quitPanel.SetActive(false);
    }

    public void SelectPlay (InputAction.CallbackContext context)
    {
        if (IsQuitOpen) return;
        if (context.started)
        {
            mainButtons[0].GetComponent<MainButton>().OnPointerEnter();
            mainButtons[1].GetComponent<MainButton>().OnPointerExit();
        }
    }
    public void SelectEdit (InputAction.CallbackContext context)
    {
        if (IsQuitOpen) return;
        if (context.started)
        {
            mainButtons[1].GetComponent<MainButton>().OnPointerEnter();
            mainButtons[0].GetComponent<MainButton>().OnPointerExit();
        }
    }

    public void PressAnyKey (InputAction.CallbackContext context)
    {
        if (!context.started || IsQuitOpen || isPressed) return;
        if (Keyboard.current != null && Keyboard.current.escapeKey.isPressed) return;

        if (context.started)
        {
            if(isPressed == false)
            {
                StartCoroutine(IEPressAnyKey());
                isPressed = true;
            }
        }
    }

    IEnumerator IEPressAnyKey()
    {
        canvases[0].GetComponent<Animation>().Play("ButtonsCome");
        canvases[1].GetComponent<Animation>().Play("ButtonsCome2");
        canvases[2].GetComponent<Animation>().Play("LogoOut");
        pressanykey.SetActive(false);

        yield return new WaitForSeconds(0.25f);
        mainButtons[0].GetComponent<MainButton>().isInteractable = true;
        mainButtons[0].GetComponent<MainButton>().btn.interactable = true;
        mainButtons[1].GetComponent<MainButton>().isInteractable = true;
        mainButtons[1].GetComponent<MainButton>().btn.interactable = true;
    }

    public void Escape(InputAction.CallbackContext context)
    {
        if (!context.started)
            return;

        SetQuitPanel(!IsQuitOpen);
    }

    public void ConfirmQuit(InputAction.CallbackContext context)
    {
        if (!context.started || !IsQuitOpen)
            return;

        QuitGame();
    }

    public void QuitGame()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }

    private void SetQuitPanel(bool open)
    {
        if (IsQuitOpen == open) return;
        if (open)
        {
            popupEventSystem = EventSystem.current;

            if (popupEventSystem != null)
            {
                previousSelected = popupEventSystem.currentSelectedGameObject;
                previousNavigationEvents = popupEventSystem.sendNavigationEvents;

                popupEventSystem.sendNavigationEvents = false;
                popupEventSystem.SetSelectedGameObject(null);
            }
        }

        quitPanel.SetActive(open);

        if (!open && popupEventSystem != null)
        {
            popupEventSystem.sendNavigationEvents = previousNavigationEvents;

            popupEventSystem.SetSelectedGameObject(
                previousSelected != null &&
                previousSelected.activeInHierarchy ? previousSelected : null
            );
        }
    }

}
