using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HitEffectButton", menuName = "CustomButtons/HitEffectButton")]
public class HitEffectButton : ButtonActionSO
{
    public override void ButtonAction(CustomDataSO customData)
    {
        GameManager.Instance.GetPlayerCustomize().SetHitEffect((HitEffectData)customData);
    }
}
