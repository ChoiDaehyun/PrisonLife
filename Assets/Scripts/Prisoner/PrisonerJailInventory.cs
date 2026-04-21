using System;
using UnityEngine;

public class PrisonerJailInventory : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int maxJailedCount = 20;

    [Header("Runtime")]
    [SerializeField] private int currentJailedCount;

    public event Action<int, int> OnJailedCountChanged;

    public int CurrentJailedCount => currentJailedCount;
    public int MaxJailedCount => maxJailedCount;

    public bool IsFull => currentJailedCount >= maxJailedCount;

    public void InitializeMaxCount(int maxCount)
    {
        maxJailedCount = Mathf.Max(1, maxCount);
        NotifyChanged();
    }

    public bool TryRegisterPrisoner()
    {
        if (IsFull)
        {
            return false;
        }

        currentJailedCount++;
        NotifyChanged();

        return true;
    }

    private void NotifyChanged()
    {
        OnJailedCountChanged?.Invoke(currentJailedCount, maxJailedCount);
    }
}