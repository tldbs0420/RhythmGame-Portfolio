using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GearButton", menuName = "CustomButtons/GearButton")]
public class GearButton : ButtonActionSO
{
    public override void ButtonAction(CustomDataSO customData)
    {
        GameManager.Instance.GetPlayerCustomize().SetGear((GearData)customData);
    }
}
