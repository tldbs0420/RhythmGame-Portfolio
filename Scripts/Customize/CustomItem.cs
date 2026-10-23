using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Composites;
using UnityEngine.UI;

public class CustomItem : MonoBehaviour
{
    [SerializeField] ButtonActionSO buttonAction;
    private CustomDataSO customData;
    float targetScale = 1f;
    public short select = 0;
    public float scaleSpeed = 5f;
    public string category;

    protected virtual void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(targetScale, targetScale, targetScale), scaleSpeed * Time.deltaTime);
    }

    public virtual void OnPointerEnter()
    {
        CustomManager.Instance.ChangeSelectedButton(this);
        targetScale = 1.1f;
        select = 1;
        transform.GetChild(1).GetComponent<Image>().color = GameManager.Instance.GetPlayerCustomize().gearData.personalColor;
        transform.GetChild(1).gameObject.SetActive(true);
    }

    public virtual void Deselect()
    {
        targetScale = 1f;
        select = 0;
        transform.GetChild(1).gameObject.SetActive(false);
    }

    public void CheckItem()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }

    public void DecheckItem()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void ClickAction()
    {
        buttonAction.ButtonAction(customData);
        transform.GetChild(1).GetComponent<Image>().color = GameManager.Instance.GetPlayerCustomize().gearData.personalColor;
        if (category != "GearPosition")
            CustomManager.Instance.ApplySelectedItem();
    }

    public void SetOutlineColor(Color newColor)
    {
        transform.GetChild(1).GetComponent<Image>().color = newColor;
    }

    public ButtonActionSO GetButtonAction()
    {
        return buttonAction;
    }

    public void SetCustomData(CustomDataSO customData)
    {
        this.customData = customData;
        GetComponent<Image>().sprite = customData.thumbNail;
    }

    public CustomDataSO GetCustomData()
    {
        return customData;
    }
}
