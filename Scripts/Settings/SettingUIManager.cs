using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SettingUIManager : MonoBehaviour
{
    static SettingUIManager instance;
    public static SettingUIManager Instance
    {
        get
        {
            return instance;
        }
    }

    void Awake()
    {
        if (instance == null)
            instance = this;
    }



    int nowSelect = 0; // 0 = display | 1 = gameplay | 2 = sound
    int uiSelected = 0;
    Color uiColor;

    public TextMeshProUGUI[] Categories;
    public GameObject[] ItemsOfCategories;
    public SettingItem[] DisplayItems;
    public SettingItem[] GameplayItems;
    public SoundItem[] SoundItems;

    [SerializeField]
    private TextMeshProUGUI descriptionText;

    public void Init()
    {
        uiColor = Category.Instance.colors[Category.Instance.GetCategoryIndex(MusicListManager.Instance.sheets[GameManager.Instance.title].Artist)];
        for (int i = 0; i < DisplayItems.Length; i++)
        {
            DisplayItems[i].buttonNum = i;
            DisplayItems[i].ChangeColor(uiColor);
        }
        for (int i = 0; i < GameplayItems.Length; i++)
        {
            GameplayItems[i].buttonNum = i;
            GameplayItems[i].ChangeColor(uiColor);
        }
        for (int i = 0; i < SoundItems.Length; i++)
        {
            SoundItems[i].buttonNum = i;
            SoundItems[i].ChangeColor(uiColor);
        }
        ItemsOfCategories[0].SetActive(true);
        ItemsOfCategories[1].SetActive(false);
        ItemsOfCategories[2].SetActive(false);

        Categories[0].color = uiColor;
        DisplayItems[0].Selected();
        uiSelected = 0;
    }
    public void SetText(int category, int item, string text)
    {
        switch (category)
        {
            case 0:
                DisplayItems[item].ChangeText(text);
                break;
            case 1:
                GameplayItems[item].ChangeText(text);
                break;
            case 2:
                SoundItems[item].ChangeText(text);
                break;
        }
    }

    public void NextCategory()
    {
        if (nowSelect < 2)
        {
            Categories[nowSelect].color = Color.white;
            if (nowSelect == 0)
                DisplayItems[uiSelected].RemoveOutline();
            else if (nowSelect == 1)
                GameplayItems[uiSelected].RemoveOutline();
            ItemsOfCategories[nowSelect].SetActive(false);
            nowSelect += 1;
            uiSelected = 0;
            Categories[nowSelect].color = uiColor;
            ItemsOfCategories[nowSelect].SetActive(true);

            if (nowSelect == 1)
                GameplayItems[uiSelected].Selected();
            else if (nowSelect == 2)
                SoundItems[uiSelected].Selected();
        }
    }

    public void PrevCategory()
    {
        if (nowSelect > 0)
        {
            Categories[nowSelect].color = Color.white;
            if (nowSelect == 1)
                GameplayItems[uiSelected].RemoveOutline();
            else if (nowSelect == 2)
                SoundItems[uiSelected].RemoveOutline();
            ItemsOfCategories[nowSelect].SetActive(false);
            nowSelect -= 1;
            uiSelected = 0;
            Categories[nowSelect].color = uiColor;
            ItemsOfCategories[nowSelect].SetActive(true);

            if (uiSelected == 0)
            {
                if (nowSelect == 1)
                    GameplayItems[uiSelected].Selected();
                else if (nowSelect == 0)
                    DisplayItems[uiSelected].Selected();
            }
        }
    }

    public void SelectCategory(int i)
    {
        if ( i == 0 )
        {
            Categories[0].color = uiColor;
            ItemsOfCategories[0].SetActive(true);
            Categories[1].color = Color.white;
            ItemsOfCategories[1].SetActive(false);
            Categories[2].color = Color.white;
            ItemsOfCategories[2].SetActive(false);

            if (nowSelect == 1)
                GameplayItems[uiSelected].RemoveOutline();
            else if (nowSelect == 2)
                SoundItems[uiSelected].RemoveOutline();

            nowSelect = 0;
            uiSelected = 0;
            DisplayItems[uiSelected].Selected();
        }
        else if ( i == 1 )
        {
            Categories[0].color = Color.white;
            ItemsOfCategories[0].SetActive(false);
            Categories[1].color = uiColor;
            ItemsOfCategories[1].SetActive(true);
            Categories[2].color = Color.white;
            ItemsOfCategories[2].SetActive(false);

            if (nowSelect == 0)
                DisplayItems[uiSelected].RemoveOutline();
            else if (nowSelect == 2)
                SoundItems[uiSelected].RemoveOutline();

            nowSelect = 1;
            uiSelected = 0;
            GameplayItems[uiSelected].Selected();
        }
        else if ( i == 2 )
        {
            Categories[0].color = Color.white;
            ItemsOfCategories[0].SetActive(false);
            Categories[1].color = Color.white;
            ItemsOfCategories[1].SetActive(false);
            Categories[2].color = uiColor;
            ItemsOfCategories[2].SetActive(true);

            if (nowSelect == 0)
                DisplayItems[uiSelected].RemoveOutline();
            else if (nowSelect == 1)
                GameplayItems[uiSelected].RemoveOutline();

            nowSelect = 2;
            uiSelected = 0;
            SoundItems[uiSelected].Selected();
        }
    }

    public void NextItem()
    {
        int len = 0;

        switch (nowSelect)
        {
            case 0:
                len = DisplayItems.Length - 1;
                break;
            case 1:
                len = GameplayItems.Length - 1;
                break;
            case 2:
                len = SoundItems.Length - 1;
                break;
        }

        if (uiSelected < len)
        {
            switch (nowSelect)
            {
                case 0:
                    DisplayItems[uiSelected].DeSelected();
                    uiSelected += 1;
                    DisplayItems[uiSelected].Selected();
                    break;
                case 1:
                    GameplayItems[uiSelected].DeSelected();
                    uiSelected += 1;
                    GameplayItems[uiSelected].Selected();
                    break;
                case 2:
                    SoundItems[uiSelected].DeSelected();
                    uiSelected += 1;
                    SoundItems[uiSelected].Selected();
                    break;
            }
        }
    }

    public void PrevItem()
    {
        if (uiSelected > 0)
        {
            switch (nowSelect)
            {
                case 0:
                    DisplayItems[uiSelected].DeSelected();
                    uiSelected -= 1;
                    DisplayItems[uiSelected].Selected();
                    break;
                case 1:
                    GameplayItems[uiSelected].DeSelected();
                    uiSelected -= 1;
                    GameplayItems[uiSelected].Selected();
                    break;
                case 2:
                    SoundItems[uiSelected].DeSelected();
                    uiSelected -= 1;
                    SoundItems[uiSelected].Selected();
                    break;
            }
        }
    }

    public void ItemSelect(int i)
    {
        switch (nowSelect)
        {
            case 0:
                DisplayItems[uiSelected].DeSelected();
                uiSelected = i;
                DisplayItems[uiSelected].Selected();
                break;
            case 1:
                GameplayItems[uiSelected].DeSelected();
                uiSelected = i;
                GameplayItems[uiSelected].Selected();
                break;
            case 2:
                SoundItems[uiSelected].DeSelected();
                uiSelected = i;
                SoundItems[uiSelected].Selected();
                break;
        }
    }

    public void SettingValueUp()
    {
        switch (nowSelect)
        {
            case 0:
                DisplayItems[uiSelected].ValueUp();
                break;
            case 1:
                if (uiSelected == 4)
                    GameplayItems[uiSelected].ValueUp();
                break;
            case 2:
                SoundItems[uiSelected].ValueUp();
                break;
        }
    }
    public void SettingValueDown()
    {
        switch (nowSelect)
        {
            case 0:
                DisplayItems[uiSelected].ValueDown();
                break;
            case 1:
                if (uiSelected == 4)
                    GameplayItems[uiSelected].ValueDown();
                break;
            case 2:
                SoundItems[uiSelected].ValueDown();
                break;
        }
    }

    public void SetDescription(string text)
    {
        if (descriptionText == null) return;

        descriptionText.text = text ?? string.Empty;
    }


    public void SettingEnter()
    {
        switch (nowSelect)
        {
            case 1:
                if (uiSelected <= 3)
                    GameplayItems[uiSelected].ValueChange();
                else if (uiSelected == 5)
                    ToSync();
                break;
        }
    }

    public void ToSync()
    {
        GameManager.Instance.InSync();
    }
}
