using System.Collections;
using TMPro;
using Utils.Singleton;

public class CoinsDisplay : Singleton<CoinsDisplay>
{
    private TextMeshProUGUI t_coinsAmount;

    protected override void Awake()
    {
        t_coinsAmount = GetComponent<TextMeshProUGUI>();
    }

    public void UpdateCoinsAmountDisplayer(int value) 
    {
        t_coinsAmount.text = value.ToString();
    }
}
