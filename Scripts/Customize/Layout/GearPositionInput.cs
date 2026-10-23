using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GearPositionInput : MonoBehaviour
{
    public void ExitGearPosition(InputAction.CallbackContext context)
    {
        if (context.started)
            GameManager.Instance.OutGearPosition();
    }
}
