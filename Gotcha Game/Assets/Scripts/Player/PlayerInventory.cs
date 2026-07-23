using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{


    public int coins = 0;
    public HashSet<GachaReward> rewards;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rewards = new HashSet<GachaReward>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Collect(int coinValue)
    {
        coins += coinValue;
        PrintBalance();
    }

    public void Collect(GachaReward gachaReward)
    {
        rewards.Add(gachaReward);
        PrintBalance();
    }

    public void PrintBalance()
    {
        string balanceString = "[CURRENT BALANCE]\n";
        balanceString += "Coins: " + coins + "\n\n";
        if(rewards.Count != 0)
        {
            balanceString += "Gacha Rewards: \n";
            foreach(GachaReward reward in rewards)
            {
                balanceString += " - " + reward.GetName() + ", (" + reward.GetRarity() + ")\n";
            }
        }

        Debug.Log(balanceString);
    }
}
