using UnityEngine;

[CreateAssetMenu(fileName = "Gear_", menuName = "CustomItems/Gear")]
public class GearData : CustomDataSO
{
    public Sprite gear;
    public Sprite timer;
    public string perfect;
    public Color hpColor;
    public Color personalColor;
}