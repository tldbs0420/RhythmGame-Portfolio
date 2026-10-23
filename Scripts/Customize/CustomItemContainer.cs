using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomItemContainer : MonoBehaviour
{
    public ThemeData[] themeDatas;
    public GearData[] gearDatas;
    public NotesData[] notesDatas;
    public LineData[] lineDatas;
    public CharacterData[] characterDatas;
    public HitLineData[] hitLineDatas;
    public HitEffectData[] hitEffectDatas;
    public HitSoundData[] hitSoundDatas;
    public string[] customNames;

    public CustomDataSO[] GetList(int category)
    {
        if (category == 1) return themeDatas;
        else if (category == 2) return gearDatas;
        else if (category == 3) return notesDatas;
        else if (category == 4) return lineDatas;
        else if (category == 5) return lineDatas;
        else if (category == 6) return lineDatas;
        else if (category == 7) return lineDatas;
        else if (category == 8) return characterDatas;
        else if (category == 9) return hitLineDatas;
        else if (category == 10) return hitEffectDatas;
        else if (category == 11) return hitSoundDatas;
        else return null;
    }
}
