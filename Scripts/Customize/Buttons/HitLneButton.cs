using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HitLineButton", menuName = "CustomButtons/HitLineButton")]
public class HitLineButton : ButtonActionSO
{
    public override void ButtonAction(CustomDataSO customData)
    {
        GameManager.Instance.GetPlayerCustomize().SetHitLine((HitLineData)customData);
    }
}