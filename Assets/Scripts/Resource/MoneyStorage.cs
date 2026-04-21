using System;
using UnityEngine;

public class MoneyStorage : MonoBehaviour
{
    [Header("Runtime")]
    [SerializeField] private int storedMoney;

    public event Action<int> OnStoredMoneyChanged;

    public int StoredMoney => storedMoney;

    private void Start()
    {
        NotifyStoredMoneyChanged();
    }

    public void AddMoney(int amount)
    {
        if (amount <= 0)
        {
            return;
        }

        storedMoney += amount;

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

        return claimedMoney;
    }

    public bool TryConsumeMoney(int amount)
    {
        if (amount <= 0)
        {
            return false;
        }

        if (storedMoney < amount)
        {
            return false;
        }

        storedMoney -= amount;

        NotifyStoredMoneyChanged();

        return true;
    }


    private void NotifyStoredMoneyChanged()
    {
        OnStoredMoneyChanged?.Invoke(storedMoney);
    }
}
