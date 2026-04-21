using System;
using UnityEngine;

public class WarehouseInventory : MonoBehaviour, IOreSupplier
{
    [Header("Runtime")]
    [SerializeField] private int storedOreCount;

    public event Action<int> OnStoredOreChanged;
    public event Action<int> OnOreAdded;
    public event Action<int> OnOreSupplied;

    public int StoredOreCount => storedOreCount;

    private void Start()
    {
        NotifyStoredOreChanged();
    }

    public void AddOre(int amount)
    {
        if (amount <= 0)
        {
            return;
        }

        storedOreCount += amount;

        NotifyStoredOreChanged();
        OnOreAdded?.Invoke(amount);
        OnOreSupplied?.Invoke(amount);
    }

    private void NotifyStoredOreChanged()
    {
        OnStoredOreChanged?.Invoke(storedOreCount);
    }
}
