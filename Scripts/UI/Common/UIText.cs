using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIText : UIObject
{
    TextMeshProUGUI text;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    public void SetText(string _text)
    {
        text.text = _text;
    }

    public void SetColor ( Color color )
    {
        text.color = color;
    }

    public void ChangeText()
    {
        UIController.Instance.find.Invoke(Name);
    }
}
