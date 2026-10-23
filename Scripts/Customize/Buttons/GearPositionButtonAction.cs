using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GearPositionButtonAction", menuName = "CustomButtons/GearPositionButtonAction")]
public class GearPositionButtonAction : ButtonActionSO
{
    public override void ButtonAction(CustomDataSO customData)
    {
        GameManager.Instance.InGearPosition();
    }
}
