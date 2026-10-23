using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ThemeButton", menuName = "CustomButtons/ThemeButton")]
public class ThemeButton : ButtonActionSO
{
    public override void ButtonAction(CustomDataSO customData)
    {
        GameManager.Instance.GetPlayerCustomize().SetTheme((ThemeData)customData);
    }
}
