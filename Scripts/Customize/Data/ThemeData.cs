using UnityEngine;

[CreateAssetMenu(fileName = "Theme_", menuName = "CustomItems/Theme")]
public class ThemeData : CustomDataSO
{
    public GearData gear;
    public NotesData notes;
    public CharacterData character;
    public HitLineData hitLine;
    public HitEffectData hitEffect;
}