using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ResultInput : MonoBehaviour
{
    public void ReturnToSelect(InputAction.CallbackContext context)
    {
        GameManager.Instance.ReturnSelect();
    }
}
