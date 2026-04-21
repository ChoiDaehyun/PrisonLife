using System;
using UnityEngine;

public class PlayerMoneyWallet : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int moneyPerHandcuff = 10;

    [Header("Runtime")]
    [SerializeField] private int storedMoney;

    public event Action<int> OnStoredMoneyChanged;
    public event Action<int> OnMoneyClaimed;

    public int StoredMoney => storedMoney;

    private void Start()
    {
        NotifyStoredMoneyChanged();
    }

    public void AddMoneyFromHandcuffs(int handcuffAmount)
    {
        if (handcuffAmount <= 0)
        {
            return;
        }

        int addedMoney = handcuffAmount * moneyPerHandcuff;
        storedMoney += addedMoney;

        NotifyStoredMoneyChanged();
    }

    public int ClaimAllMoney()
    {
        if (storedMoney <= 0)
        {
            return 0;
        }

        int claimedMoney = storedMoney;
        storedMoney = 0;

        NotifyStoredMoneyChanged();
        OnMoneyClaimed?.Invoke(claimedMoney);

        return claimedMoney;
    }

    private void NotifyStoredMoneyChanged()
    {
        OnStoredMoneyChanged?.Invoke(StoredMoney);
    }
}
