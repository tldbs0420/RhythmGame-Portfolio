using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class SheetLoader : MonoBehaviour
{
    static SheetLoader instance;
    public static SheetLoader Instance
    {
        get
        {
            return instance;
        }
    }

    public string pathSheet;
    public int sheetCount;
    public bool bLoadFinish;
    int remain;

    void Awake()
    {
        if(instance == null)
            instance = this;
    }

    public void Init()
    {
        pathSheet = $"{Application.dataPath}/Sheet";

        if (Directory.Exists(pathSheet))
        {
            DirectoryInfo d = new DirectoryInfo(pathSheet);
            sheetCount = remain = d.GetDirectories().Length;
        }

        StartCoroutine(IELoad());

    }

    IEnumerator IELoad()
    {
        DirectoryInfo di = new DirectoryInfo(pathSheet);
        foreach (DirectoryInfo d in di.GetDirectories())
        {
            yield return SheetParser.Instance.IEParse(d.Name);
            if ( !MusicListManager.Instance.sheets.ContainsKey(d.Name) )
                MusicListManager.Instance.sheets.Add(d.Name, SheetParser.Instance.sheet);
            if (--remain <= 0 )
                bLoadFinish = true;
        }
    }
}
