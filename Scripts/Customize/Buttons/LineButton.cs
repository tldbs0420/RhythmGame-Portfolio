using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LineButton", menuName = "CustomButtons/LineButton")]
public class LineButton : ButtonActionSO
{
    public int line;
    public override void ButtonAction(CustomDataSO customData)
    {
        GameManager.Instance.GetPlayerCustomize().SetLine((LineData)customData, line);
    }
}
