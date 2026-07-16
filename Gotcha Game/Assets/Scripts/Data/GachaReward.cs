using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class GachaReward
{
    public string rewardName;
    public string rewardRarity;
    public Sprite rewardSprite;

    public string GetName()
    {
        return rewardName;
    }

    public Sprite GetSprite()
    {
        return rewardSprite;
    }

    public string GetRarity()
    {
        return rewardRarity;
    }
}
