using Unity.Netcode;
using UnityEngine;

public class CoinWallet : NetworkBehaviour
{
    public NetworkVariable<int> totalCoins = new();

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

        totalCoins.Value += coinValue;
    }

    public void SpendPoints(int cost)
    {
        totalCoins.Value -= cost;
    }
}
