using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SyncSheetParser : MonoBehaviour
{
    static SyncSheetParser instance;
    public static SyncSheetParser Instance
    {
        get
        {
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
    string basePath = $"{Application.dataPath}/Resources";

    public string clip;
    public Sprite img;

    string songName;

    void Awake() // 음악 받아오고 텍스트 파일 불러오기
    {
        if ( instance == null )
            instance = this;
    }

    public void ParsePlease()
    {
        // StartCoroutine(IEParse("SyncSheet"));
        string title = "SyncSheet";
        sheet = new Sheet();
        string readLine = string.Empty;

        using (StreamReader sr = new StreamReader($"{basePath}/{title}/{title}.sheet"))
        {
            readLine = sr.ReadLine();

            while ( readLine != null )
            {
                if ( readLine.StartsWith("[Description]"))
                {
                    currentStep = Step.Description;
                    readLine = sr.ReadLine();
                }
                else if ( readLine.StartsWith("[Audio]"))
                {
                    currentStep = Step.Audio;
                    readLine = sr.ReadLine();
                }
                else if ( readLine.StartsWith("[Note]"))
                {
                    currentStep = Step.Note;
                    readLine = sr.ReadLine();
                }

                if ( currentStep == Step.Description )
                {
                    if (readLine.StartsWith("Title"))
                        sheet.Title = readLine.Split('=')[1].Trim();
                    else if (readLine.StartsWith("Artist"))
                        sheet.Artist = readLine.Split('=')[1].Trim();
                    else if (readLine.StartsWith("Original"))
                        sheet.Original = readLine.Split('=')[1].Trim();
                }
                else if ( currentStep == Step.Audio )
                {
                    if ( readLine.StartsWith("BPM"))
                        sheet.BPM = int.Parse(readLine.Split('=')[1].Trim());
                    else if ( readLine.StartsWith("Offset"))
                        sheet.offset = int.Parse(readLine.Split('=')[1].Trim());
                    else if (readLine.StartsWith("Signature"))
                    {
                        sheet.Signature = readLine.Split('=')[1].Trim();
                        string[] s = sheet.Signature.Split('/');
                        sheet.sign = int.Parse(s[0].Trim());
                    }
                }
                else if ( currentStep == Step.Note )
                {
                    if ( string.IsNullOrEmpty(readLine))
                        break;

                    string[] s = readLine.Split(',');
                    int time = int.Parse(s[0].Trim());
                    int type = int.Parse(s[1].Trim());
                    int line = int.Parse(s[2].Trim());
                    int tail = -1;
                    if ( s.Length > 3 )
                         tail = int.Parse(s[3].Trim());
                    sheet.notes.Add(new Note(time, type, line, tail));
                }

                readLine = sr.ReadLine();

            }
        }

        sheet.clip = $"{basePath}/{title}/{title}.mp4";
    }

    //불러온 텍스트 파일 읽어내기

    /*public IEnumerator IEParse(string title)
    {
        //sheet = GameObject.Find("Sheet").GetComponent<Sheet>();
        sheet = new Sheet();
        string readLine = string.Empty;

        using (StreamReader sr = new StreamReader($"{basePath}/{title}/{title}.sheet"))
        {
            readLine = sr.ReadLine();

                while ( readLine != null )
                {
                    if ( readLine.StartsWith("[Description]"))
                    {
                        currentStep = Step.Description;
                        readLine = sr.ReadLine();
                }
                else if ( readLine.StartsWith("[Audio]"))
                {
                    currentStep = Step.Audio;
                    readLine = sr.ReadLine();
                }
                else if ( readLine.StartsWith("[Note]"))
                {
                    currentStep = Step.Note;
                    readLine = sr.ReadLine();
                }

                if ( currentStep == Step.Description )
                {
                    if (readLine.StartsWith("Title"))
                        sheet.Title = readLine.Split('=')[1].Trim();
                    else if (readLine.StartsWith("Artist"))
                        sheet.Artist = readLine.Split('=')[1].Trim();
                    else if (readLine.StartsWith("Original"))
                        sheet.Original = readLine.Split('=')[1].Trim();
                }
                else if ( currentStep == Step.Audio )
                {
                    if ( readLine.StartsWith("BPM"))
                        sheet.BPM = int.Parse(readLine.Split('=')[1].Trim());
                    else if ( readLine.StartsWith("Offset"))
                        sheet.offset = int.Parse(readLine.Split('=')[1].Trim());
                    else if (readLine.StartsWith("Signature"))
                    {
                        string[] s = readLine.Split('=');
                        s = s[1].Split('/');
                        sheet.sign = int.Parse(s[0].Trim());
                    }
                }
                else if ( currentStep == Step.Note )
                {
                    if ( string.IsNullOrEmpty(readLine))
                        break;

                    string[] s = readLine.Split(',');
                    int time = int.Parse(s[0].Trim());
                    int type = int.Parse(s[1].Trim());
                    int line = int.Parse(s[2].Trim());
                    int tail = -1;
                    if ( s.Length > 3 )
                         tail = int.Parse(s[3].Trim());
                    sheet.notes.Add(new Note(time, type, line, tail));
                }

                readLine = sr.ReadLine();

            }
        }

        sheet.clip = $"{basePath}/{title}/{title}.mp4";
        yield return null;
    }*/
    private void OnDestroy() {
        instance = null;
    }
}
