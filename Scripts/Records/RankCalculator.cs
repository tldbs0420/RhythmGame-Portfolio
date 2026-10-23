public enum RankType
{
    None,
    D,
    C,
    B,
    A,
    S
}

public static class RankCalculator
{
    public static RankType GetRank(float rate)
    {
        if (rate >= 95f) return RankType.S;
        if (rate >= 90f) return RankType.A;
        if (rate >= 80f) return RankType.B;
        if (rate >= 70f) return RankType.C;

        return RankType.D;
    }
}