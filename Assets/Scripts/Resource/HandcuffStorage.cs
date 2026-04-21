using System;
using UnityEngine;

public class HandcuffStorage : MonoBehaviour
{
    [Header("Runtime")]
    [SerializeField] private int claimStoredHandcuffCount;
    [SerializeField] private int submittedStoredHandcuffCount;

    public event Action<int> OnClaimStoredHandcuffChanged;
    public event Action<int> OnSubmittedStoredHandcuffChanged;

    public int ClaimStoredHandcuffCount => claimStoredHandcuffCount;
    public int SubmittedStoredHandcuffCount => submittedStoredHandcuffCount;

    private void Start()
    {
        NotifyClaimChanged();
        NotifySubmittedChanged();
    }

    public void AddClaimHandcuffs(int amount)
    {
        if (amount <= 0)
        {
            return;
        }

        claimStoredHandcuffCount += amount;

        NotifyClaimChanged();
    }

    public int ClaimAllHandcuffs()
    {
        if (claimStoredHandcuffCount <= 0)
        {
            return 0;
        }

        int claimedAmount = claimStoredHandcuffCount;
        claimStoredHandcuffCount = 0;

        NotifyClaimChanged();

        return claimedAmount;
    }

    public void AddSubmittedHandcuffs(int amount)
    {
        if (amount <= 0)
        {
            return;
        }

        submittedStoredHandcuffCount += amount;

        NotifySubmittedChanged();
    }

    public bool TryConsumeClaimHandcuffs(int amount)
    {
        if (amount <= 0)
        {
            return false;
        }

        if (claimStoredHandcuffCount < amount)
        {
            return false;
        }

        claimStoredHandcuffCount -= amount;

        NotifyClaimChanged();

        return true;
    }

    public bool TryConsumeSubmittedHandcuffs(int amount)
    {
        if (amount <= 0)
        {
            return false;
        }

        if (submittedStoredHandcuffCount < amount)
        {
            return false;
        }

        submittedStoredHandcuffCount -= amount;

        NotifySubmittedChanged();

        return true;
    }

    private void NotifyClaimChanged()
    {
        OnClaimStoredHandcuffChanged?.Invoke(claimStoredHandcuffCount);
    }

    private void NotifySubmittedChanged()
    {
        OnSubmittedStoredHandcuffChanged?.Invoke(submittedStoredHandcuffCount);
    }
}
