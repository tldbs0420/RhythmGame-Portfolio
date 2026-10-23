using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIButton : UIObject
{
    Button btn;
    Image image;
    TextMeshProUGUI text;

    void Start()
    {
        btn = GetComponent<Button>();
        text = transform.GetComponentInChildren<TextMeshProUGUI>();
        btn.onClick.AddListener(OnClick);
        image = GetComponent<Image>();
    }

    public void SetImage(Sprite sprite)
    {
        image.sprite = sprite;
    }

    public void SetText(string txt)
    {
        text.text = txt;
    }

    public void OnClick()
    {
        UIController.Instance.find.Invoke(Name);
    }
}
