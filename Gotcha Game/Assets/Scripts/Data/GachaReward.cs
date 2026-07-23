using UnityEngine;
using UnityEngine.UI;

public enum Rarity {Common, Epic, Rare, Homosexual};

[System.Serializable]
public class GachaReward
{
    public string rewardName;
    public Rarity rewardRarity;
    public Sprite rewardSprite;

    public string GetName()
    {
        return rewardName;
    }

    public Sprite GetSprite()
    {
        return rewardSprite;
    }

    public Color GetColor()
    {
        switch (rewardRarity)
        {
            case Rarity.Common:
                return Color.beige;
            case Rarity.Rare:
                return Color.royalBlue;
            case Rarity.Epic:
                return Color.rebeccaPurple;
            case Rarity.Homosexual:
                return Color.softRed;
            default:
                return Color.beige;
        }
    }

    public string GetRarity()
    {
        return rewardRarity.ToString();
    }
}
