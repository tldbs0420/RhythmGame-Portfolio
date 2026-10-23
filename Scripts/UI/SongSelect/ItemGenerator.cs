using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemGenerator : MonoBehaviour
{
    static ItemGenerator instance;
    public static ItemGenerator Instance
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

    List<GameObject> items = new List<GameObject>();
    public GameObject itemPrefab;

    public void Init()
    {
        Image cover = itemPrefab.transform.GetChild(0).GetComponent<Image>();
        TextMeshProUGUI Title = itemPrefab.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI Singer = itemPrefab.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        Image favorite = itemPrefab.transform.GetChild(3).GetComponent<Image>();
        Image easy = itemPrefab.transform.GetChild(4).GetComponent<Image>();
        Image normal = itemPrefab.transform.GetChild(5).GetComponent<Image>();
        Image hard = itemPrefab.transform.GetChild(6).GetComponent<Image>();

        foreach (var sheet in MusicListManager.Instance.sheets)
        {
            cover.sprite = sheet.Value.img;
            Title.text = sheet.Value.Title;
            Singer.text = sheet.Value.Artist;

            GameObject finishItem = Instantiate(itemPrefab, transform);
            finishItem.name = sheet.Value.Title;
            items.Add(finishItem);
        }
    }

    private void OnDestroy() {
        instance = null;
    }
}
