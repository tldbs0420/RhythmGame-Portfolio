using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    static SceneLoader instance;
    public static SceneLoader Instance
    {
        get
        {
            return instance;
        }
    }

    void Awake()
    {
        if(instance == null)
            instance = this;
    }

    public void LoadMusicSelect()
    {
        
    }
}
