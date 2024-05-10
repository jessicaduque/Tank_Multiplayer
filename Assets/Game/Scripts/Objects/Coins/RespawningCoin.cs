using System;

public class RespawningCoin : Coin
{
    public event Action<RespawningCoin> OnCollected;
    public override int Collect()
    {
        if (!IsServer)
        {
            Show(false);
            return 0;
        }

        if (_alreadyCollected)
        {
            return 0;
        }

        _alreadyCollected = true;

        OnCollected?.Invoke(this);

        return _coinValue;
    }

    public void Reset()
    {
        _alreadyCollected = false;
    }

}