using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NoteButton", menuName = "CustomButtons/NoteButton")]
public class NoteButton : ButtonActionSO
{
    public override void ButtonAction(CustomDataSO customData)
    {
        GameManager.Instance.GetPlayerCustomize().SetNotes((NotesData)customData);
    }
}
