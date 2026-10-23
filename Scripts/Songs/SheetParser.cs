using System.Collections;
using System.Collections.Generic;
using System.IO;
//using System;
using UnityEngine;
//using UnityEditor.iOS;
//using Unity.VisualScripting.Antlr3.Runtime;
//using Unity.VisualScripting;
using UnityEngine.Networking;

public class SheetParser : MonoBehaviour
{
    static SheetParser instance;
    public static SheetParser Instance
    {
        get
        {
            if ( instance == null )
                instance = new SheetParser();
            return instance;
        }
    }



    enum Step
    {
        Description,
        Audio,
        Note,
    }
    Step currentStep = Step.Description;

    public Sheet sheet;
    string basePath = $"{Application.dataPath}/Sheet";

    public string clip;
    public Sprite img;

    string songName;

    void Awake() // 음악 받아오고 텍스트 파일 불러오기
    {
        if ( instance == null )
            instance = this;
    }

    //불러온 텍스트 파일 읽어내기

    public IEnumerator IEParse(string title)
    {
        //sheet = GameObject.Find("Sheet").GetComponent<Sheet>();
        sheet = new Sheet();
        currentStep = Step.Description;
        GameManager.DifficultyType currentDifficulty = GameManager.DifficultyType.NORMAL;

        using (StreamReader sr = new StreamReader($"{basePath}/{title}/{title}.sheet"))
        {
            string readLine;

            while ((readLine = sr.ReadLine()) != null)
            {
                readLine = readLine.Trim();
                if (string.IsNullOrEmpty(readLine)) continue;

                switch (readLine)
                {
                    case "[Description]":
                        currentStep = Step.Description;
                        continue;

                    case "[Audio]":
                        currentStep = Step.Audio;
                        continue;

                    case "[Note.Easy]":
                        currentStep = Step.Note;
                        currentDifficulty = GameManager.DifficultyType.EASY;
                        continue;

                    case "[Note]":
                    case "[Note.Normal]":
                        currentStep = Step.Note;
                        currentDifficulty = GameManager.DifficultyType.NORMAL;
                        continue;

                    case "[Note.Hard]":
                        currentStep = Step.Note;
                        currentDifficulty = GameManager.DifficultyType.HARD;
                        continue;
                }

                if (currentStep == Step.Description)
                {
                    if (readLine.StartsWith("Title"))
                        sheet.Title = readLine.Split('=')[1].Trim();
                    else if (readLine.StartsWith("Artist"))
                        sheet.Artist = readLine.Split('=')[1].Trim();
                    else if (readLine.StartsWith("Original"))
                        sheet.Original = readLine.Split('=')[1].Trim();
                }
                else if (currentStep == Step.Audio)
                {
                    if (readLine.StartsWith("BPM"))
                        sheet.BPM = int.Parse(readLine.Split('=')[1].Trim());
                    else if (readLine.StartsWith("Offset"))
                        sheet.offset = int.Parse(readLine.Split('=')[1].Trim());
                    else if (readLine.StartsWith("Signature"))
                    {
                        sheet.Signature = readLine.Split('=')[1].Trim();
                        string[] s = sheet.Signature.Split('/');
                        sheet.sign = int.Parse(s[0].Trim());
                    }
                }
                else if (currentStep == Step.Note)
                {
                    string[] s = readLine.Split(',');

                    int time = int.Parse(s[0].Trim());
                    int type = int.Parse(s[1].Trim());
                    int line = int.Parse(s[2].Trim());
                    int tail = -1;

                    if (s.Length > 3)
                        tail = int.Parse(s[3].Trim());

                    sheet.GetNotes(currentDifficulty).Add(new Note(time, type, line, tail));
                }
            }
        }

        yield return IEGetImg(title);
        sheet.img = img;
        sheet.clip = $"{basePath}/{title}/{title}.mp4";
    }

    public IEnumerator IEGetImg(string title)
    {
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture($"{basePath}/{title}/{title}.jpg"))
        {
            yield return request.SendWebRequest();
            Texture2D t = DownloadHandlerTexture.GetContent(request);
            img = Sprite.Create(t, new Rect(0, 0, t.width, t.height), new Vector2(0.5f, 0.5f));
            img.name = title;
        }
    }
    private void OnDestroy() {
        instance = null;
    }
}
