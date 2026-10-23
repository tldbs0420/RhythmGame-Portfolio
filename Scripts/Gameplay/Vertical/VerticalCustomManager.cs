using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VerticalCustomManager : MonoBehaviour
{
    [SerializeField] SpriteRenderer gear;
    [SerializeField] Image timer;
    [SerializeField] Image hp;
    [SerializeField] SpriteRenderer hitLine;
    [SerializeField] Transform characterPosition;
    [SerializeField] NotePool notePool;
    [SerializeField] HitEffectPool hitEffectPool;

    [SerializeField] Transform gearPosition;
    [SerializeField] RectTransform ui;
    [SerializeField] RectTransform hpUI;

    [SerializeField] SpriteRenderer bg;
    public void Init()
    {
        GameManager gameManager = GameManager.Instance;

        gearPosition.localPosition = new Vector3(PlayerPrefs.GetFloat("GearPosition"), gearPosition.localPosition.y, gearPosition.localPosition.z);
        ui.localPosition = new Vector3(PlayerPrefs.GetFloat("UIPosition"), ui.localPosition.y, ui.localPosition.z);
        hpUI.localPosition = new Vector3(PlayerPrefs.GetFloat("HPPosition"), hpUI.localPosition.y, hpUI.localPosition.z);
        characterPosition.localPosition = new Vector3(PlayerPrefs.GetFloat("CharaPosition"), characterPosition.localPosition.y);

        gear.sprite = gameManager.GetPlayerCustomize().gearData.gear;
        timer.sprite = GameManager.Instance.GetPlayerCustomize().gearData.timer;
        hp.color = GameManager.Instance.GetPlayerCustomize().gearData.hpColor;
        hitLine.sprite = GameManager.Instance.GetPlayerCustomize().hitLineData.hitLine;
        GameObject temp = Instantiate(GameManager.Instance.GetPlayerCustomize().characterData.character, characterPosition);
        Sprite[] notes = new Sprite[4] { GameManager.Instance.GetPlayerCustomize().line1.note,
            GameManager.Instance.GetPlayerCustomize().line2.note,
            GameManager.Instance.GetPlayerCustomize().line3.note,
            GameManager.Instance.GetPlayerCustomize().line4.note };

        Color[] colors = new Color[4] { GameManager.Instance.GetPlayerCustomize().line1.lineColor,
            GameManager.Instance.GetPlayerCustomize().line2.lineColor,
            GameManager.Instance.GetPlayerCustomize().line3.lineColor,
            GameManager.Instance.GetPlayerCustomize().line4.lineColor };
        notePool.SetNotesAndColors(notes, colors);
        hitEffectPool.SetHitEffect(GameManager.Instance.GetPlayerCustomize().hitEffectData.hitEffect);

        bg.color = new Color(bg.color.r, bg.color.g, bg.color.b, PlayerPrefs.GetFloat("GearTransparency"));
        temp.GetComponent<SpriteRenderer>().color = new Color(temp.GetComponent<SpriteRenderer>().color.r, temp.GetComponent<SpriteRenderer>().color.g, temp.GetComponent<SpriteRenderer>().color.b, PlayerPrefs.GetFloat("CharaTransparency"));
    }
}
