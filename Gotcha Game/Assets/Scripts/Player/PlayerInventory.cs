using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{


    public int coins = 0;
    public List<GachaReward> rewards;

    private TMP_Text coinText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rewards = new List<GachaReward>();
        coinText = GameObject.FindGameObjectWithTag("Coin Countah").GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Collect(int coinValue)
    {
        coins += coinValue;
        PrintBalance();
        UpdateCoinText();
    }

    public void SpendCoins(int cost)
    {
        // checking for negative input
        if (cost < 0)
            cost = -cost;
        coins -= cost;
        
        // removing negative values
        if (coins < 0)
            coins = 0;

        UpdateCoinText();
    }

    private void UpdateCoinText()
    {
        if (!coinText)
            return;
        coinText.text = coins + " Coins";
    }

    public void Collect(GachaReward gachaReward)
    {
        rewards.Add(gachaReward);
        PrintBalance();
    }

    public int GetCoinBalance()
    {
        return coins;
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
