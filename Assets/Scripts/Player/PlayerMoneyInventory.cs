using System;
using UnityEngine;

public class PlayerMoneyInventory : MonoBehaviour
{
    [Header("Runtime")]
    [SerializeField] private int currentMoney;
    [SerializeField] private bool hasReceivedMoneyAtLeastOnce;

    public event Action<int> OnMoneyChanged;
    public event Action OnFirstMoneyReceived;
    public int CurrentMoney => currentMoney;
    public bool HasReceivedMoneyAtLeastOnce => hasReceivedMoneyAtLeastOnce;

    [SerializeField] private AudioSource submitMoneyAudio;

    private void Start()
    {
        NotifyMoneyChanged();
    }

    public void AddMoney(int amount)
    {
        if (amount <= 0)
        {
            return;
        }

        bool shouldFireFirstMoneyEvent = !hasReceivedMoneyAtLeastOnce && currentMoney <= 0;

        currentMoney += amount;

        if (!hasReceivedMoneyAtLeastOnce && currentMoney > 0)
        {
            hasReceivedMoneyAtLeastOnce = true;
        }

        NotifyMoneyChanged();

        if (shouldFireFirstMoneyEvent && hasReceivedMoneyAtLeastOnce)
        {
            OnFirstMoneyReceived?.Invoke();
        }
    }

    public bool TrySpendMoney(int amount)
    {
        if (amount <= 0)
        {
            return false;
        }

        if (currentMoney < amount)
        {
            return false;
        }

        currentMoney -= amount;
        submitMoneyAudio.Play();

        NotifyMoneyChanged();

        return true;
    }

    private void NotifyMoneyChanged()
    {
        OnMoneyChanged?.Invoke(currentMoney);
    }
}
