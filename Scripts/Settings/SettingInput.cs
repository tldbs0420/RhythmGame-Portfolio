using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SettingInput : MonoBehaviour
{
    public void Escape ( InputAction.CallbackContext context )
    {
        if (context.started)
        {
            GameManager.Instance.OptionToSelect();
        }
    }

    public void NextCategory ( InputAction.CallbackContext context )
    {
        if (context.performed)
        {
            SettingUIManager.Instance.NextCategory();
        }
    }

    public void PrevCategory ( InputAction.CallbackContext context )
    {
        if (context.performed)
        {
            SettingUIManager.Instance.PrevCategory();
        }
    }

    public void NextItem ( InputAction.CallbackContext context )
    {
        if (context.performed)
        {
            SettingUIManager.Instance.NextItem();
        }
    }

    public void PrevItem ( InputAction.CallbackContext context )
    {
        if (context.performed)
        {
            SettingUIManager.Instance.PrevItem();
        }
    }

    public void SettingUp ( InputAction.CallbackContext context )
    {
        if (context.performed)
        {
            SettingUIManager.Instance.SettingValueUp();
        }
    }

    public void SettingDown ( InputAction.CallbackContext context )
    {
        if (context.performed)
        {
            SettingUIManager.Instance.SettingValueDown();
        }
    }

    public void SettingEnter ( InputAction.CallbackContext context )
    {
        if (context.performed)
        {
            SettingUIManager.Instance.SettingEnter();
        }
    }

}