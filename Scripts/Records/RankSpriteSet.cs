using UnityEngine;

[CreateAssetMenu(fileName = "RankSpriteSet", menuName = "Game/Rank Sprite Set")]
public class RankSpriteSet : ScriptableObject
{
    [SerializeField] private Sprite none;
    [SerializeField] private Sprite d;
    [SerializeField] private Sprite c;
    [SerializeField] private Sprite b;
    [SerializeField] private Sprite a;
    [SerializeField] private Sprite s;

    public Sprite GetSprite(RankType rank)
    {
        return rank switch
        {
            RankType.D => d,
            RankType.C => c,
            RankType.B => b,
            RankType.A => a,
            RankType.S => s,
            _ => none
        };
    }
}