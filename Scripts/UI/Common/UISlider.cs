using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISlider : UIObject
{
    public Slider slider;

    void Start()
    {
        slider = GetComponent<Slider>();
    }

    public void OnValue(float value)
    {
        UIController.Instance.find.Invoke(Name);
    }
}
