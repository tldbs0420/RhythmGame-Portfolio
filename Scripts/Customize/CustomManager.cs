using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomManager : MonoBehaviour
{
    [SerializeField] GameObject[] gameObjects;
    [SerializeField] CustomItem selectedButton = null;
    CustomItemContainer customItemContainer = null;
    [SerializeField] GameObject[] customItems;

    [SerializeField] CustomItem[] gearPositionButton;
    [SerializeField] CustomItem[] themeButtons;
    [SerializeField] CustomItem[] gearButtons;
    [SerializeField] CustomItem[] noteButtons;
    [SerializeField] CustomItem[] line1Buttons;
    [SerializeField] CustomItem[] line2Buttons;
    [SerializeField] CustomItem[] line3Buttons;
    [SerializeField] CustomItem[] line4Buttons;
    [SerializeField] CustomItem[] characterButtons;
    [SerializeField] CustomItem[] hitLineButtons;
    [SerializeField] CustomItem[] hitEffectButtons;
    [SerializeField] CustomItem[] hitSoundButtons;

    List<CustomItem[]> categories = new List<CustomItem[]>();
    private List<int> onlyThemeIndex = new List<int> { 0, 1, 11 };
    private List<int> detailIndex = new List<int> { 0, 2, 3, 8, 9, 10, 11 };
    private List<int> detailIndex2 = new List<int> { 0, 2, 4, 5, 6, 7, 8, 9, 10, 11 };
    [SerializeField] int nowCategory;

    [SerializeField] Scrollbar scrollbar;
    [SerializeField] float[] targetSliderOnlyTheme;
    [SerializeField] float[] targetSliderDetail;
    [SerializeField] float[] targetSliderDetail2;

    [SerializeField] Image[] selectedItemThumbnails;

    [SerializeField] bool themeDetail = false;
    [SerializeField] bool noteDetail = false;


    static CustomManager instance;
    public static CustomManager Instance
    {
        get
        {
            return instance;
        }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void Init()
    {
        customItemContainer = GetComponent<CustomItemContainer>();

        categories.Add(gearPositionButton);
        categories.Add(themeButtons);
        categories.Add(gearButtons);
        categories.Add(noteButtons);
        categories.Add(line1Buttons);
        categories.Add(line2Buttons);
        categories.Add(line3Buttons);
        categories.Add(line4Buttons);
        categories.Add(characterButtons);
        categories.Add(hitLineButtons);
        categories.Add(hitEffectButtons);
        categories.Add(hitSoundButtons);
        nowCategory = 0;

        for (int i = 0; i < 11; i++)
        {
            int index = PlayerPrefs.GetInt(customItemContainer.customNames[i]);
            CustomItem[] customItems = categories[i + 1];
            selectedItemThumbnails[i].sprite = customItemContainer.GetList(i + 1)[index].thumbNail;
            for (int j = 0; j < 4; j++)
            {
                customItems[j].SetCustomData(customItemContainer.GetList(i + 1)[index]);

                if (j == 0) customItems[j].CheckItem();
                else if (customItems[j].GetCustomData().id == customItems[0].GetCustomData().id)
                    customItems[j].CheckItem();

                index = index + 1;
                if (index >= customItemContainer.GetList(i + 1).Length) index = 0;
            }
        }

        themeDetail = !Convert.ToBoolean(PlayerPrefs.GetInt("UseTheme"));
        noteDetail = !Convert.ToBoolean(PlayerPrefs.GetInt("UseNoteSet"));

        if (themeDetail)
        {
            customItems[0].SetActive(false);
            customItems[1].SetActive(true);
            if (!noteDetail)
                customItems[2].SetActive(true);
            else
            {
                customItems[3].SetActive(true);
                customItems[4].SetActive(true);
                customItems[5].SetActive(true);
                customItems[6].SetActive(true);
            }
            customItems[7].SetActive(true);
            customItems[8].SetActive(true);
            customItems[9].SetActive(true);
        }

        selectedButton = categories[0][0];
        selectedButton.OnPointerEnter();
    }

    public void ChangeSelectedButton(CustomItem newSelectedButton)
    {
        selectedButton.Deselect();
        selectedButton = newSelectedButton;
        SetCategory(selectedButton.category);
    }

    public void DeselectButton()
    {
        selectedButton = null;
    }

    public void CustomScroll()
    {
        if (!themeDetail)
            scrollbar.value = targetSliderOnlyTheme[nowCategory];
        else if (themeDetail && !noteDetail)
            scrollbar.value = targetSliderDetail[nowCategory];
        else if (themeDetail && noteDetail)
            scrollbar.value = targetSliderDetail2[nowCategory];
    }

    public void KeySelectCategoryItem(int dir)
    {
        int nowIndex = Array.IndexOf(categories[nowCategory], selectedButton);
        if (dir == 1)
        {
            if (nowIndex < categories[nowCategory].Length - 1)
                categories[nowCategory][nowIndex + 1].OnPointerEnter();
            else
            {
                for (int i = 0; i < 3; i++)
                    categories[nowCategory][i].SetCustomData(categories[nowCategory][i + 1].GetCustomData());
                int itemIndex = Array.IndexOf(customItemContainer.GetList(nowCategory), categories[nowCategory][3].GetCustomData());
                itemIndex += 1;
                if (itemIndex >= customItemContainer.GetList(nowCategory).Length) itemIndex = 0;
                categories[nowCategory][3].SetCustomData(customItemContainer.GetList(nowCategory)[itemIndex]);
                UpdateSelectedItem();
            }
        }
        else if (dir == -1)
        {
            if (nowIndex > 0)
                categories[nowCategory][nowIndex - 1].OnPointerEnter();
            else
            {
                for (int i = 3; i > 0; i--)
                    categories[nowCategory][i].SetCustomData(categories[nowCategory][i - 1].GetCustomData());
                int itemIndex = Array.IndexOf(customItemContainer.GetList(nowCategory), categories[nowCategory][0].GetCustomData());
                itemIndex -= 1;
                if (itemIndex < 0) itemIndex = customItemContainer.GetList(nowCategory).Length - 1;
                categories[nowCategory][0].SetCustomData(customItemContainer.GetList(nowCategory)[itemIndex]);
                UpdateSelectedItem();
            }
        }
    }

    public void KeySelectCategory(int dir)
    {
        int nowIndex = Array.IndexOf(categories[nowCategory], selectedButton);
        nowCategory = GetNextCategory(dir);

        categories[nowCategory][nowIndex].OnPointerEnter();
        CustomScroll();
    }

    private int GetNextCategory(int dir)
    {
        int target = 0;
        List<int> categoryList = new List<int>();
        if (!themeDetail)
            categoryList = onlyThemeIndex;
        else if (themeDetail && !noteDetail)
            categoryList = detailIndex;
        else if (themeDetail && noteDetail)
            categoryList = detailIndex2;

        target = categoryList.IndexOf(nowCategory);

        if (dir == 1)
        {
            if (target == categoryList.Count - 1)
                target = 0;
            else
                target += 1;
        }
        else if (dir == -1)
        {
            if (target == 0)
                target = categoryList.Count - 1;
            else
                target -= 1;
        }

        target = categoryList[target];
        return target;
    }

    private void SetCategory(string category)
    {
        switch (category)
        {
            case "GearPosition":
                nowCategory = 0;
                break;
            case "Theme":
                nowCategory = 1;
                break;
            case "Gear":
                nowCategory = 2;
                break;
            case "Note":
                nowCategory = 3;
                break;
            case "NoteLine1":
                nowCategory = 4;
                break;
            case "NoteLine2":
                nowCategory = 5;
                break;
            case "NoteLine3":
                nowCategory = 6;
                break;
            case "NoteLine4":
                nowCategory = 7;
                break;
            case "Character":
                nowCategory = 8;
                break;
            case "HitLine":
                nowCategory = 9;
                break;
            case "HitEffect":
                nowCategory = 10;
                break;
            case "HitSound":
                nowCategory = 11;
                break;
        }
    }

    public void ApplySelectedItem()
    {
        CustomItem[] customItems = null;
        customItems = categories[nowCategory];

        for (int i = 0; i < 4; i++)
        {
            customItems[i].DecheckItem();
            if (customItems[i].GetCustomData().id == selectedButton.GetCustomData().id) customItems[i].CheckItem();
        }
        selectedItemThumbnails[nowCategory - 1].sprite = selectedButton.GetCustomData().thumbNail;
    }

    public void UpdateSelectedItem()
    {
        CustomItem[] customItems = null;
        customItems = categories[nowCategory];

        for (int i = 0; i < 4; i++)
        {
            customItems[i].DecheckItem();
            if (customItems[i].GetCustomData().id == GameManager.Instance.GetPlayerCustomize().GetCustomData(nowCategory).id) customItems[i].CheckItem();
        }
    }

    public void ToggleThemeDetail()
    {
        if (!themeDetail)
        {
            themeDetail = true;
            PlayerPrefs.SetInt("UseTheme", 0);
            customItems[0].SetActive(false);
            customItems[1].SetActive(true);
            if (!noteDetail)
                customItems[2].SetActive(true);
            else
            {
                customItems[3].SetActive(true);
                customItems[4].SetActive(true);
                customItems[5].SetActive(true);
                customItems[6].SetActive(true);
            }
            customItems[7].SetActive(true);
            customItems[8].SetActive(true);
            customItems[9].SetActive(true);

            for (int i = 0; i < 10; i++)
            {
                int index = PlayerPrefs.GetInt(customItemContainer.customNames[i]);
                selectedItemThumbnails[i].sprite = GameManager.Instance.GetPlayerCustomize().GetCustomData(i + 1).thumbNail;
                CustomItem[] customItem = categories[i + 1];
                for (int j = 0; j < 4; j++)
                {
                    customItem[j].DecheckItem();
                    customItem[j].SetCustomData(customItemContainer.GetList(i + 1)[index]);

                    if (customItem[j].GetCustomData().id == GameManager.Instance.GetPlayerCustomize().GetCustomData(i + 1).id)
                        customItem[j].CheckItem();

                    index = index + 1;
                    if (index >= customItemContainer.GetList(i + 1).Length) index = 0;
                }
            }

            categories[2][0].OnPointerEnter();
        }
        else
        {
            themeDetail = false;
            PlayerPrefs.SetInt("UseTheme", 1);
            customItems[0].SetActive(true);
            customItems[1].SetActive(false);
            if (!noteDetail)
                customItems[2].SetActive(false);
            else
            {
                customItems[3].SetActive(false);
                customItems[4].SetActive(false);
                customItems[5].SetActive(false);
                customItems[6].SetActive(false);
            }
            customItems[7].SetActive(false);
            customItems[8].SetActive(false);
            customItems[9].SetActive(false);
            GameManager.Instance.GetPlayerCustomize().SetTheme(GameManager.Instance.GetPlayerCustomize().themeData);

            for (int i = 0; i < 10; i++)
            {
                int index = PlayerPrefs.GetInt(customItemContainer.customNames[i]);
                selectedItemThumbnails[i].sprite = GameManager.Instance.GetPlayerCustomize().GetCustomData(i + 1).thumbNail;
                CustomItem[] customItem = categories[i + 1];
                for (int j = 0; j < 4; j++)
                {
                    customItem[j].DecheckItem();
                    customItem[j].SetCustomData(customItemContainer.GetList(i + 1)[index]);

                    if (customItem[j].GetCustomData().id == GameManager.Instance.GetPlayerCustomize().GetCustomData(i + 1).id)
                        customItem[j].CheckItem();

                    index = index + 1;
                    if (index >= customItemContainer.GetList(i + 1).Length) index = 0;
                }
            }
            categories[1][0].OnPointerEnter();
        }
    }

    public void ToggleNoteDetail()
    {
        if (!noteDetail)
        {
            noteDetail = true;
            PlayerPrefs.SetInt("UseNoteSet", 0);
            customItems[2].SetActive(false);
            customItems[3].SetActive(true);
            customItems[4].SetActive(true);
            customItems[5].SetActive(true);
            customItems[6].SetActive(true);
            for (int i = 3; i < 7; i++)
            {
                int index = PlayerPrefs.GetInt(customItemContainer.customNames[i]);
                selectedItemThumbnails[i].sprite = GameManager.Instance.GetPlayerCustomize().GetCustomData(i + 1).thumbNail;
                CustomItem[] customItem = categories[i + 1];
                for (int j = 0; j < 4; j++)
                {
                    customItem[j].DecheckItem();
                    customItem[j].SetCustomData(customItemContainer.GetList(i + 1)[index]);

                    if (customItem[j].GetCustomData().id == GameManager.Instance.GetPlayerCustomize().GetCustomData(i + 1).id)
                        customItem[j].CheckItem();

                    index = index + 1;
                    if (index >= customItemContainer.GetList(i + 1).Length) index = 0;
                }
            }
            categories[4][0].OnPointerEnter();
        }
        else
        {
            noteDetail = false;
            PlayerPrefs.SetInt("UseNoteSet", 1);
            customItems[2].SetActive(true);
            customItems[3].SetActive(false);
            customItems[4].SetActive(false);
            customItems[5].SetActive(false);
            customItems[6].SetActive(false);
            GameManager.Instance.GetPlayerCustomize().SetNotes(GameManager.Instance.GetPlayerCustomize().notesData);

            for (int i = 3; i < 7; i++)
            {
                int index = PlayerPrefs.GetInt(customItemContainer.customNames[i]);
                selectedItemThumbnails[i].sprite = GameManager.Instance.GetPlayerCustomize().GetCustomData(i + 1).thumbNail;
                CustomItem[] customItem = categories[i + 1];
                for (int j = 0; j < 4; j++)
                {
                    customItem[j].DecheckItem();
                    customItem[j].SetCustomData(customItemContainer.GetList(i + 1)[index]);

                    if (customItem[j].GetCustomData().id == GameManager.Instance.GetPlayerCustomize().GetCustomData(i + 1).id)
                        customItem[j].CheckItem();

                    index = index + 1;
                    if (index >= customItemContainer.GetList(i + 1).Length) index = 0;
                }
            }

            categories[3][0].OnPointerEnter();
        }
    }

    public void EnterGearPosition()
    {
        foreach( GameObject obj in gameObjects)
        {
            obj.SetActive(false);
        }
    }

    public void ExitGearPosition()
    {
        foreach( GameObject obj in gameObjects)
        {
            obj.SetActive(true);
        }
    }
    
    public CustomItem GetSelectedButton() { return selectedButton; }
}
