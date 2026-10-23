using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GearPositionManager : MonoBehaviour
{
    public Slider gearSlider;
    public Slider charaSlider;
    public Slider bgSlider;
    public Slider charaTransSlider;
    public Transform gear;
    public Transform chara;
    public RectTransform ui;
    public RectTransform hp;

    public SpriteRenderer bg;
    public SpriteRenderer createdChara;

    void Awake()
    {
        gear.transform.localPosition = new Vector3(PlayerPrefs.GetFloat("GearPosition"), gear.transform.localPosition.y);
        ui.localPosition = new Vector3(PlayerPrefs.GetFloat("UIPosition"), ui.localPosition.y, ui.localPosition.z);
        hp.localPosition = new Vector3(PlayerPrefs.GetFloat("HPPosition"), hp.localPosition.y, hp.localPosition.z);
        chara.localPosition = new Vector3(PlayerPrefs.GetFloat("CharaPosition"), chara.localPosition.y, chara.localPosition.z);

        gearSlider.value = PlayerPrefs.GetFloat("GearPosition");
        charaSlider.value = PlayerPrefs.GetFloat("CharaPosition");

        bg.color = new Color(bg.color.r, bg.color.g, bg.color.b, PlayerPrefs.GetFloat("GearTransparency"));

        GameObject temp = Instantiate(GameManager.Instance.GetPlayerCustomize().characterData.character, chara);
        createdChara = temp.GetComponent<SpriteRenderer>();
        createdChara.color = new Color(createdChara.color.r, createdChara.color.g, createdChara.color.b, PlayerPrefs.GetFloat("CharaTransparency"));

        bgSlider.value = bg.color.a;
        charaTransSlider.value = createdChara.color.a;
    }

    void Update()
    {
        gear.transform.localPosition = new Vector3(gearSlider.value, gear.transform.localPosition.y);
        chara.transform.localPosition = new Vector3(charaSlider.value, chara.transform.localPosition.y);
        bg.color = new Color(bg.color.r, bg.color.g, bg.color.b, bgSlider.value);
        createdChara.color = new Color(createdChara.color.r, createdChara.color.g, createdChara.color.b, charaTransSlider.value);

        float sliderValue = gearSlider.value;

        // (1) 슬라이더 값을 [0,1] 구간으로 변환
        float t = Mathf.InverseLerp(gearSlider.minValue, gearSlider.maxValue, sliderValue);

        // (2) [0,1] 구간을 UI 좌표 범위로 매핑
        float targetX = Mathf.Lerp(-830f, 525f, t);
        float target2X = Mathf.Lerp(-265f, 1082f, t);

        // (3) UI 위치 갱신 (X만 변경)
        Vector3 current = ui.localPosition;
        ui.localPosition = new Vector3(targetX, current.y, current.z);

        Vector3 current2 = hp.localPosition;
        hp.localPosition = new Vector3(target2X, current2.y, current2.z);
    }

    public void ExitGearPosition()
    {
        GameManager.Instance.OutGearPosition();
    }

    public void Apply()
    {
        PlayerPrefs.SetFloat("GearPosition", gear.transform.localPosition.x);
        PlayerPrefs.SetFloat("UIPosition", ui.localPosition.x);
        PlayerPrefs.SetFloat("HPPosition", hp.localPosition.x);
        PlayerPrefs.SetFloat("CharaPosition", chara.transform.localPosition.x);
        PlayerPrefs.SetFloat("GearTransparency", bg.color.a);
        PlayerPrefs.SetFloat("CharaTransparency", createdChara.color.a);
    }

    public void Reset()
    {
        gear.localPosition = new Vector3(-0.13f, gear.localPosition.y);
        PlayerPrefs.SetFloat("GearPosition", gear.localPosition.x);

        ui.localPosition = new Vector3(-585f, ui.localPosition.y, ui.localPosition.z);
        PlayerPrefs.SetFloat("UIPosition", ui.localPosition.x);

        hp.localPosition = new Vector3(-20f, hp.localPosition.y, hp.localPosition.z);
        PlayerPrefs.SetFloat("HPPosition", hp.localPosition.x);

        chara.transform.localPosition = new Vector3(4.17f, chara.localPosition.y);
        PlayerPrefs.SetFloat("CharaPosition", chara.localPosition.x);

        bg.color = new Color(bg.color.r, bg.color.g, bg.color.b, 0.9882353f);
        PlayerPrefs.SetFloat("GearTransparency", bg.color.a);

        createdChara.color = new Color(createdChara.color.r, createdChara.color.g, createdChara.color.b, 1);
        PlayerPrefs.SetFloat("CharaTransparency", createdChara.color.a);

        gearSlider.value = gear.localPosition.x;
        charaSlider.value = chara.localPosition.x;
    }
}