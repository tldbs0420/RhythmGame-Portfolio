using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCustomize : MonoBehaviour
{
    public ThemeData themeData;
    public GearData gearData;
    public NotesData notesData;
    public LineData line1;
    public LineData line2;
    public LineData line3;
    public LineData line4;
    public CharacterData characterData;
    public HitLineData hitLineData;
    public HitEffectData hitEffectData;
    public HitSoundData hitSoundData;

    public void Init()
    {
        CustomItemContainer customItemContainer = GameObject.Find("CustomItemContainer").GetComponent<CustomItemContainer>();

        if (PlayerPrefs.HasKey("Theme"))
            themeData = customItemContainer.themeDatas[PlayerPrefs.GetInt("Theme")];
        else
        {
            themeData = customItemContainer.themeDatas[0];
            PlayerPrefs.SetInt("Theme", 0);
        }

        if (PlayerPrefs.HasKey("Gear"))
            gearData = customItemContainer.gearDatas[PlayerPrefs.GetInt("Gear")];
        else
        {
            gearData = customItemContainer.gearDatas[0];
            PlayerPrefs.SetInt("Gear", 0);
        }

        if (PlayerPrefs.HasKey("Notes"))
            notesData = customItemContainer.notesDatas[PlayerPrefs.GetInt("Notes")];
        else
        {
            notesData = customItemContainer.notesDatas[0];
            PlayerPrefs.SetInt("Notes", 0);
        }

        if (PlayerPrefs.HasKey("NoteLine1"))
            line1 = customItemContainer.lineDatas[PlayerPrefs.GetInt("NoteLine1")];
        else
        {
            line1 = customItemContainer.lineDatas[0];
            PlayerPrefs.SetInt("NoteLine1", 0);
        }

        if (PlayerPrefs.HasKey("NoteLine2"))
            line2 = customItemContainer.lineDatas[PlayerPrefs.GetInt("NoteLine2")];
        else
        {
            line2 = customItemContainer.lineDatas[1];
            PlayerPrefs.SetInt("NoteLine2", 1);
        }

        if (PlayerPrefs.HasKey("NoteLine3"))
            line3 = customItemContainer.lineDatas[PlayerPrefs.GetInt("NoteLine3")];
        else
        {
            line3 = customItemContainer.lineDatas[1];
            PlayerPrefs.SetInt("NoteLine3", 1);
        }

        if (PlayerPrefs.HasKey("NoteLine4"))
            line4 = customItemContainer.lineDatas[PlayerPrefs.GetInt("NoteLine4")];
        else
        {
            line4 = customItemContainer.lineDatas[0];
            PlayerPrefs.SetInt("NoteLine4", 0);
        }

        if (PlayerPrefs.HasKey("Character"))
            characterData = customItemContainer.characterDatas[PlayerPrefs.GetInt("Character")];
        else
        {
            characterData = customItemContainer.characterDatas[0];
            PlayerPrefs.SetInt("Character", 0);
        }

        if (PlayerPrefs.HasKey("HitLine"))
            hitLineData = customItemContainer.hitLineDatas[PlayerPrefs.GetInt("HitLine")];
        else
        {
            hitLineData = customItemContainer.hitLineDatas[1];
            PlayerPrefs.SetInt("HitLine", 1);
        }

        if (PlayerPrefs.HasKey("HitEffect"))
            hitEffectData = customItemContainer.hitEffectDatas[PlayerPrefs.GetInt("HitEffect")];
        else
        {
            hitEffectData = customItemContainer.hitEffectDatas[0];
            PlayerPrefs.SetInt("HitEffect", 0);
        }

        if (PlayerPrefs.HasKey("HitSound"))
            hitSoundData = customItemContainer.hitSoundDatas[PlayerPrefs.GetInt("HitSound")];
        else
        {
            hitSoundData = customItemContainer.hitSoundDatas[0];
            PlayerPrefs.SetInt("HitSound", 0);
        }
        // 자동화 가능 : 추후에 개선할 것

        if (!PlayerPrefs.HasKey("UseTheme")) PlayerPrefs.SetInt("UseTheme", 1);
        if (!PlayerPrefs.HasKey("UseNoteSet")) PlayerPrefs.SetInt("UseNoteSet", 1);
    }

    public void SetTheme(ThemeData newThemeData)
    {
        themeData = newThemeData;
        PlayerPrefs.SetInt("Theme", newThemeData.id);

        gearData = themeData.gear;
        PlayerPrefs.SetInt("Gear", newThemeData.gear.id);
        notesData = themeData.notes;
        PlayerPrefs.SetInt("Notes", newThemeData.notes.id);
        line1 = themeData.notes.line1;
        PlayerPrefs.SetInt("NoteLine1", notesData.line1.id);
        line2 = themeData.notes.line2;
        PlayerPrefs.SetInt("NoteLine2", notesData.line2.id);
        line3 = themeData.notes.line3;
        PlayerPrefs.SetInt("NoteLine3", notesData.line3.id);
        line4 = themeData.notes.line4;
        PlayerPrefs.SetInt("NoteLine4", notesData.line4.id);
        characterData = themeData.character;
        PlayerPrefs.SetInt("Character", newThemeData.character.id);
        hitLineData = themeData.hitLine;
        PlayerPrefs.SetInt("HitLine", newThemeData.hitLine.id);
        hitEffectData = themeData.hitEffect;
        PlayerPrefs.SetInt("HitEffect", newThemeData.hitEffect.id);
    }

    public void SetGear(GearData newGearData)
    {
        gearData = newGearData;
        PlayerPrefs.SetInt("Gear", newGearData.id);
    }

    public void SetNotes(NotesData newNotesData)
    {
        notesData = newNotesData;
        PlayerPrefs.SetInt("Notes", newNotesData.id);

        line1 = notesData.line1;
        PlayerPrefs.SetInt("NoteLine1", notesData.line1.id);
        line2 = notesData.line2;
        PlayerPrefs.SetInt("NoteLine2", notesData.line2.id);
        line3 = notesData.line3;
        PlayerPrefs.SetInt("NoteLine3", notesData.line3.id);
        line4 = notesData.line4;
        PlayerPrefs.SetInt("NoteLine4", notesData.line4.id);
    }

    public void SetLine(LineData newLineData, int lineNum)
    {
        switch (lineNum)
        {
            case 1:
                line1 = newLineData;
                PlayerPrefs.SetInt("NoteLine1", newLineData.id);
                break;
            case 2:
                line2 = newLineData;
                PlayerPrefs.SetInt("NoteLine2", newLineData.id);
                break;
            case 3:
                line3 = newLineData;
                PlayerPrefs.SetInt("NoteLine3", newLineData.id);
                break;
            case 4:
                line4 = newLineData;
                PlayerPrefs.SetInt("NoteLine4", newLineData.id);
                break;
        }
    }

    public void SetCharacter(CharacterData newCharacterData)
    {
        characterData = newCharacterData;
        PlayerPrefs.SetInt("Character", newCharacterData.id);
    }

    public void SetHitLine(HitLineData newHitLine)
    {
        hitLineData = newHitLine;
        PlayerPrefs.SetInt("HitLine", newHitLine.id);
    }
    public void SetHitEffect(HitEffectData newHitEffectData)
    {
        hitEffectData = newHitEffectData;
        PlayerPrefs.SetInt("HitEffect", newHitEffectData.id);
    }
    public void SetHitSound(HitSoundData newHitSoundData)
    {
        hitSoundData = newHitSoundData;
        PlayerPrefs.SetInt("HitSound", newHitSoundData.id);
    }

    public CustomDataSO GetCustomData(int category)
    {
        if (category == 1) return themeData;
        else if (category == 2) return gearData;
        else if (category == 3) return notesData;
        else if (category == 4) return line1;
        else if (category == 5) return line2;
        else if (category == 6) return line3;
        else if (category == 7) return line4;
        else if (category == 8) return characterData;
        else if (category == 9) return hitLineData;
        else if (category == 10) return hitEffectData;
        else if (category == 11) return hitSoundData;
        else return null;
    }
}
