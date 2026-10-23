using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GearPositionButton : CustomItem
{
    protected override void Update()
    {
        return;
    }

    public override void OnPointerEnter()
    {
        CustomManager.Instance.ChangeSelectedButton(this);
        select = 1;
        transform.GetChild(1).GetComponent<Image>().color = GameManager.Instance.GetPlayerCustomize().gearData.personalColor;
        transform.GetChild(1).gameObject.SetActive(true);
    }

    public override void Deselect()
    {
        select = 0;
        transform.GetChild(1).gameObject.SetActive(false);
    }
}
