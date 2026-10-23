using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SoundItem : MonoBehaviour
{
    public int buttonNum;
    public Image outLine;
    Color uiColor;
    public Outline outline;

    public TextMeshProUGUI value;
    public Slider slider;

    [SerializeField, TextArea(3, 10)]
    private string description = "";

    public float duration = 0.1f;

    public void Selected()
    {
        StartCoroutine(OutLineGenCoroutine());
        SettingUIManager.Instance.SetDescription(description);
    }

    public void DeSelected()
    {
        StartCoroutine(OutLineDelCoroutine());
    }
    public void RemoveOutline()
    {
        uiColor.a = 0;
        outLine.color = uiColor;
    }
    public void OnPointer()
    {
        SettingUIManager.Instance.ItemSelect(buttonNum);
    }
    public void ChangeColor(Color color)
    {
        uiColor = color;
        outline.effectColor = color;
    }

    public void ChangeText(string text)
    {
        value.SetText(text);
        slider.value = float.Parse(text);
    }

    public void ValueUp()
    {
        string returnValue = SettingManager.Instance.SettingChange(gameObject.name, 1);
        slider.value = float.Parse(returnValue);
    }

    public void ValueDown()
    {
        string returnValue = SettingManager.Instance.SettingChange(gameObject.name, -1);
        slider.value = float.Parse(returnValue);
    }

    public void OnValueChange()
    {
        Debug.Log("OnValueChangeCalled");
        int soundValue = (int)slider.value;
        string returnValue = SettingManager.Instance.SoundSliderChange(gameObject.name, soundValue);
        value.SetText(returnValue);
    }

    IEnumerator OutLineGenCoroutine()
    {
        float startAlpha = 0f;
        float endAlpha = 0.5f;
        float elapsedTime = 0f;

        // 초기 알파값 설정
        uiColor.a = startAlpha;
        outLine.color = uiColor;

        // 알파값을 0에서 0.5로 점진적으로 변경
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            uiColor.a = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            outLine.color = uiColor;
            outline.effectColor = uiColor;
            yield return null;
        }

        uiColor.a = endAlpha;
        outLine.color = uiColor;
    }

    IEnumerator OutLineDelCoroutine()
    {
        float startAlpha = 0.5f;
        float endAlpha = 0f;
        float elapsedTime = 0f;

        // 초기 알파값 설정
        uiColor.a = startAlpha;
        outLine.color = uiColor;

        // 알파값을 0에서 0.5로 점진적으로 변경
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            uiColor.a = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            outLine.color = uiColor;
            outline.effectColor = uiColor;
            yield return null;
        }

        uiColor.a = endAlpha;
        outLine.color = uiColor;
    }
}
