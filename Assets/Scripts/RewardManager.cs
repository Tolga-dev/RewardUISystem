using System;
using Controllers;
using TMPro;
using UnityEngine;

public class RewardManager : MonoBehaviour
{
    public HandController hand;

    public TextMeshProUGUI currentBonusText;
    public TextMeshProUGUI currentScoreText;
    public float currentBonus;

    public float currentScore; // u can add this to game play state 
    
    public void Start()
    {
        currentBonusText.text = currentBonus.ToString("f0");
        currentScoreText.text = currentScore.ToString("f0");
    }

    public void GetTheReward() // call from button
    {
        CoinReward.Instance.CountCoins();
    }
    public void AddReward(float reward)
    {
        currentScore += reward;
        currentScoreText.text = currentScore.ToString("f0");
    }
    
}
