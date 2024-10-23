using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class CoinReward : Singleton<CoinReward>
{
    public RewardManager rewardManager;
    
    [SerializeField] private GameObject pileOfCoins;
    [SerializeField] private TextMeshProUGUI counter;
    [SerializeField] private float moveDuration = 0.8f;
    [SerializeField] private float scaleDuration = 0.3f;
    [SerializeField] private float delayIncrement = 0.1f;
    [SerializeField] private Ease moveEase = Ease.InBack;
    [SerializeField] private Ease scaleEase = Ease.OutBack;

    public Transform target; // Target to follow
    public List<Vector3> coinsOriginalPositions = new List<Vector3>();
    public bool canDoAnim; // true
    public void CountCoins()
    {
        if(canDoAnim == false) return;
        canDoAnim = false;
        
        pileOfCoins.SetActive(true);

        AnimateHand();
        AnimateCoins();
        AnimateCounter();

        rewardManager.hand.StopStartHandAnimation(false);
    }

    private void AnimateHand()
    {
        rewardManager.hand.DORewind ();
        rewardManager.hand.transform.DOPunchScale(new Vector3 (1, 1, 1), .25f);
    }

    private void AnimateCoins()
    {
        float delay = 0f;
        int childCount = pileOfCoins.transform.childCount;
        float valueOfCoin = rewardManager.hand.foundBonus / childCount;

        for (int i = 0; i < childCount; i++)
        {
            Transform coin = pileOfCoins.transform.GetChild(i);
            coinsOriginalPositions.Add(coin.position);
            
            AnimateCoin(coin, delay, valueOfCoin);
            delay += delayIncrement;
        }
    }

    private void AnimateCoin(Transform coin, float delay, float valueOfCoin)
    {
        // Scale up
        coin.DOScale(1f, scaleDuration).SetDelay(delay).SetEase(scaleEase);

        // Move to the target position
        coin.DOMove(target.position, moveDuration)
            .SetDelay(delay + 0.5f)
            .SetEase(moveEase).onComplete = () => { ChangeCoinText(valueOfCoin);};

        // Rotate to zero
        coin.DORotate(Vector3.zero, 0.5f)
            .SetDelay(delay + 0.5f)
            .SetEase(Ease.Flash);

        // Scale down
        coin.DOScale(0f, scaleDuration)
            .SetDelay(delay + 1.5f)
            .SetEase(scaleEase);
        
    }

    private void AnimateCounter()
    {
        counter.transform.parent.GetChild(0).transform.DOScale(1.1f, 0.1f)
            .SetLoops(10, LoopType.Yoyo)
            .SetEase(Ease.InOutSine)
            .SetDelay(1.2f).onComplete = ResetCountCoin;
    }
    private void ChangeCoinText(float valueOfCoin)
    {
        rewardManager.AddReward(valueOfCoin);
    }

    private void ResetCountCoin()
    {
        canDoAnim = true;
        pileOfCoins.SetActive(false);

        int childCount = pileOfCoins.transform.childCount;

        for (int i = 0; i < childCount; i++)
        {
            Transform coin = pileOfCoins.transform.GetChild(i);
            coin.position = coinsOriginalPositions[i];
        }
        coinsOriginalPositions.Clear(); // u dont have to clean and add every time :)
        rewardManager.hand.StopStartHandAnimation(true);
    }
}
