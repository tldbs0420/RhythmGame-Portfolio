using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RecordSaveData
{
    public int version = 1;
    public List<PlayRecord> records = new List<PlayRecord>();
}

public class RecordRepository
{
    private RecordSaveData data = new RecordSaveData();
    private readonly string filePath = Path.Combine(Application.persistentDataPath, "records.json");

    public void Load()
    {
        if (!File.Exists(filePath)) return;

        try
        {
            string json = File.ReadAllText(filePath);
            RecordSaveData loaded = JsonUtility.FromJson<RecordSaveData>(json);

            if (loaded != null && loaded.records != null)
                data = loaded;
        }
        catch (Exception e)
        {
            Debug.LogError($"기록 불러오기 실패: {e.Message}");
        }
    }

    public PlayRecord GetRecord(string songId, int difficulty, int gameMode)
    {
        return data.records.Find(record =>
            record.songId == songId &&
            record.difficulty == difficulty &&
            record.gameMode == gameMode
        );
    }

    public void UpdateRecord(string songId, int difficulty, int gameMode, int score, int combo, float accuracy)
    {
        PlayRecord record = GetRecord(songId, difficulty, gameMode);

        bool isNew = false;

        if (record == null)
        {
            record = new PlayRecord
            {
                songId = songId,
                difficulty = difficulty,
                gameMode = gameMode
            };

            data.records.Add(record);
            isNew = true;
        }

        bool changed = record.UpdateBest(score, combo, accuracy);

        if (changed || isNew)
            Save();
    }

    private void Save()
    {
        try
        {
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(filePath, json);
        }
        catch (Exception e)
        {
            Debug.LogError($"기록 저장 실패: {e.Message}");
        }
    }
}