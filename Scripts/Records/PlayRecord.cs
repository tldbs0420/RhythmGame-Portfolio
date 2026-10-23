using System;

[Serializable]
public class PlayRecord
{
    public string songId;
    public int difficulty;
    public int gameMode;

    public int bestScore;
    public int bestCombo;
    public float bestAccuracy;

    public bool UpdateBest(int score, int combo, float accuracy)
    {
        bool changed = false;

        if (score > bestScore)
        {
            bestScore = score;
            changed = true;
        }

        if (combo > bestCombo)
        {
            bestCombo = combo;
            changed = true;
        }

        if (accuracy > bestAccuracy)
        {
            bestAccuracy = accuracy;
            changed = true;
        }

        return changed;
    }
}
