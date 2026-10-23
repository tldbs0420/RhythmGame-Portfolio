using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using UnityEngine;

public class SheetSave : MonoBehaviour
{
    private int gridInterval = 32; // 그리드 높이 (Prefab 참조)\

    public void Save()
    {

        List<Note> notes = new List<Note>();
        string noteStr = string.Empty;
        float baseTime = MusicListManager.Instance.sheets[GameManager.Instance.title].BarPerSec / gridInterval;
        foreach ( EditNoteObject note in EditNoteGenerator.Instance.toReleaseList)
        {
            if ( !note.gameObject.activeSelf)
                continue;

            float notePosX = note.transform.position.x;
            int findLine = 0;
            if ( notePosX > -3.27f  && notePosX < -1.81f )
                findLine = 0;
            else if ( notePosX > -1.81f  && notePosX < 0f )
                findLine = 1;
            else if ( notePosX > 0f  && notePosX < 1.81f )
                findLine = 2;
            else if ( notePosX > 1.81f  && notePosX < 3.27f )
                findLine = 3;

            if ( note is EditNoteShort )
            {
                EditNoteShort noteShort = note as EditNoteShort;
                int noteTime = (int)(noteShort.transform.localPosition.y * baseTime * 1000) + (int)MusicListManager.Instance.sheets[GameManager.Instance.title].offset;

                notes.Add(new Note(noteTime, (int)NoteType.Short, findLine + 1, -1));
            }
            else if ( note is EditNoteLong )
            {
                EditNoteLong noteLong = note as EditNoteLong;
                int headTime = (int)(noteLong.transform.localPosition.y * baseTime * 1000 + MusicListManager.Instance.sheets[GameManager.Instance.title].offset);
                int tailTime = (int)((noteLong.transform.localPosition.y + noteLong.tail.transform.localPosition.y) * baseTime * 1000 + MusicListManager.Instance.sheets[GameManager.Instance.title].offset);

                notes.Add(new Note(headTime, (int)NoteType.Long, findLine + 1, tailTime));

            }
        }

        notes = notes.OrderBy( a => a.time).ToList();

        // 런타임 sheet 동기화
        Sheet sheet = MusicListManager.Instance.sheets[GameManager.Instance.title];
        List<Note> selectedNotes = sheet.GetNotes((GameManager.DifficultyType)GameManager.Instance.Difficulty);
        selectedNotes.Clear();
        selectedNotes.AddRange(notes);

        StringBuilder noteSections = new StringBuilder();
        string[] headers =
        {
            "[Note.Easy]",
            "[Note.Normal]",
            "[Note.Hard]"
        };

        for (int i = 0; i < headers.Length; i++)
        {
            noteSections.AppendLine(headers[i]);

            foreach (Note n in sheet.GetNotes((GameManager.DifficultyType)i))
            {
                if (n.type == (int)NoteType.Short)
                    noteSections.AppendLine($"{n.time}, {n.type}, {n.line}");
                else if (n.type == (int)NoteType.Long)
                    noteSections.AppendLine($"{n.time}, {n.type}, {n.line}, {n.tail}");
            }
            noteSections.AppendLine();
        }

        string writer = $"[Description]\n" + 
            $"Title={MusicListManager.Instance.sheets[GameManager.Instance.title].Title}\n" +
            $"Artist={MusicListManager.Instance.sheets[GameManager.Instance.title].Artist}\n" +
            $"Original={MusicListManager.Instance.sheets[GameManager.Instance.title].Original}\n\n" +
            $"[Audio]\n" +
            $"BPM={MusicListManager.Instance.sheets[GameManager.Instance.title].BPM}\n" +
            $"Offset={MusicListManager.Instance.sheets[GameManager.Instance.title].offset}\n" +
            $"Signature={sheet.Signature}\n\n" +
            noteSections.ToString();

        writer.TrimEnd('\r', '\n');

        string pathSheet = $"{Application.dataPath}/Sheet/{MusicListManager.Instance.sheets[GameManager.Instance.title].Title}/{MusicListManager.Instance.sheets[GameManager.Instance.title].Title}.sheet";
        if ( File.Exists(pathSheet) )
        {
            try
            {
                File.Delete(pathSheet);
            }
            catch (IOException e)
            {
                Debug.LogError(e.Message);
                return;
            }
        }

        if ( !File.Exists(pathSheet) )
        {
            using (FileStream fs = File.Create(pathSheet))
            {

            }
        }
        else
        {
            Debug.LogError($"{MusicListManager.Instance.sheets[GameManager.Instance.title].Title}.sheet가 이미 존재합니다.");
            return;
        }

        using (StreamWriter sw = new StreamWriter(pathSheet))
        {
            sw.Write(writer);
        }
    }

    
}
