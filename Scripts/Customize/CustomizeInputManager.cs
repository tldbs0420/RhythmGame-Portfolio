using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CustomizeInputManager : MonoBehaviour
{
    public void Right(InputAction.CallbackContext context)
    {
        if (context.started)
            CustomManager.Instance.KeySelectCategoryItem(1);
    }

    public void Left(InputAction.CallbackContext context)
    {
        if (context.started)
            CustomManager.Instance.KeySelectCategoryItem(-1);
    }

    public void Up(InputAction.CallbackContext context)
    {
        if (context.started)
            CustomManager.Instance.KeySelectCategory(-1);
    }

    public void Down(InputAction.CallbackContext context)
    {
        if (context.started)
            CustomManager.Instance.KeySelectCategory(1);
    }

    public void Enter(InputAction.CallbackContext context)
    {
        if (context.started)
            CustomManager.Instance.GetSelectedButton().ClickAction();
    }

    public void Escape(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            GameManager.Instance.CustomizeToSelect();
        }
    }
    
}
