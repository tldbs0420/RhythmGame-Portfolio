using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HitSoundButton", menuName = "CustomButtons/HitSoundButton")]
public class HitSoundButton : ButtonActionSO
{
    public override void ButtonAction(CustomDataSO customData)
    {
        GameManager.Instance.GetPlayerCustomize().SetHitSound((HitSoundData)customData);
    }
}
