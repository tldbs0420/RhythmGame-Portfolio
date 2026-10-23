using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    static UIController instance;
    public static UIController Instance { get { return instance; } }

    public Func<string, UIActor> find;
    Dictionary<string, UIActor> uiObjectDic = new Dictionary<string, UIActor>();

    private void Awake()
    {
        if ( instance == null )
            instance = this;
    }

    public void Init()
    {
        find = FindUI;

        UIObject[] objs = FindObjectsOfType<UIObject>();
        foreach (UIObject obj in objs)
            uiObjectDic.Add(obj.Name, new UIActor(obj, null));

        if (SceneManager.GetActiveScene().name == "Editor")
        {
            uiObjectDic["Start"].action = Editor.Instance.Play;
            uiObjectDic["Stop"].action = Editor.Instance.Stop;
        }

        if (SceneManager.GetActiveScene().name == "VerticalPlay")
        {
            UIText speedUI = uiObjectDic["UI_Speed"].uiObject as UIText;
            speedUI.SetText("Speed : " + GameManager.Instance.Speed.ToString("0.0"));
        }
        else if (SceneManager.GetActiveScene().name == "DancePlay")
        {
            UIText speedUI = uiObjectDic["UI_Speed"].uiObject as UIText;
            speedUI.SetText("Speed : " + GameManager.Instance.DanceSpeed.ToString("0.0"));
        }
        if (SceneManager.GetActiveScene().name == "VerticalPlay")
            GetComponent<VerticalCustomManager>().Init();
    }

    public UIActor FindUI(string uiName)
    {
        UIActor actor = uiObjectDic[uiName];
        if ( actor.action != null )
            actor.action.Invoke(actor.uiObject);
        return actor;
    }

    public UIActor GetUI(string uiName)
    {
        UIActor actor = uiObjectDic[uiName];
        return actor;
    }

    private void OnDestroy() {
        instance = null;
    }
}
