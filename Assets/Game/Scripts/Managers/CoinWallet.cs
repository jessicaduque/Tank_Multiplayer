using Unity.Netcode;
using UnityEngine;

public class CoinWallet : NetworkBehaviour
{
    public NetworkVariable<int> totalCoins = new();
    private CoinsDisplay _coinsDisplay => CoinsDisplay.I;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.TryGetComponent<Coin>(out Coin coin))
        {
            return;
        }

        int coinValue = coin.Collect();

        if (!IsServer)
        {
            return;
        }

        if(totalCoins.Value < 999)
        {
            totalCoins.Value += coinValue;
            _coinsDisplay.UpdateCoinsAmountDisplayer(totalCoins.Value);
        }
            
    }

    public void SpendPoints(int cost)
    {
        totalCoins.Value -= cost;
        _coinsDisplay.UpdateCoinsAmountDisplayer(totalCoins.Value);
    }
}
