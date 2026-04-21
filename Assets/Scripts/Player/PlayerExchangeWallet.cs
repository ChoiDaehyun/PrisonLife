using System;
using UnityEngine;

public class PlayerExchangeWallet : MonoBehaviour
{
    [Header("Runtime")]
    [SerializeField] private int storedOreCurrency;

    public event Action<int> OnStoredOreCurrencyChanged;
    public event Action<int> OnHandcuffClaimed;

    public int StoredOreCurrency => storedOreCurrency;

    private void Start()
    {
        NotifyStoredOreCurrencyChanged();
    }

    //제출된 광물을 교환 지갑에 누적
    public void AddOreCurrency(int amount)
    {
        if (amount <= 0)
        {
            return;
        }

        storedOreCurrency += amount;

        NotifyStoredOreCurrencyChanged();
    }

    //수갑 청구
    public int ClaimAllHandcuffs()
    {
        if (storedOreCurrency <= 0)
        {
            return 0;
        }

        int claimedHandcuffs = storedOreCurrency;
        storedOreCurrency = 0;

        NotifyStoredOreCurrencyChanged();
        OnHandcuffClaimed?.Invoke(claimedHandcuffs);

        return claimedHandcuffs;
    }

    private void NotifyStoredOreCurrencyChanged()
    {
        OnStoredOreCurrencyChanged?.Invoke(storedOreCurrency);
    }
}
