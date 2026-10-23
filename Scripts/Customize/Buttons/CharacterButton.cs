using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterButton", menuName = "CustomButtons/CharacterButton")]
public class CharacterButton : ButtonActionSO
{
    public override void ButtonAction(CustomDataSO customData)
    {
        GameManager.Instance.GetPlayerCustomize().SetCharacter((CharacterData)customData);
    }
}
