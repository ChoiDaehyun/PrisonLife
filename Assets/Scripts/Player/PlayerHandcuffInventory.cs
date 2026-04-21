using System;
using UnityEngine;

public class PlayerHandcuffInventory : MonoBehaviour
{
    [Header("Runtime")]
    [SerializeField] private int currentHandcuffCount;

    public event Action<int> OnHandcuffChanged; //수갑 개수 바뀔 때

    public int CurrentHandcuffCount => currentHandcuffCount;

    [SerializeField] private ParticleSystem handcuffClaimParticle;
    [SerializeField] private ParticleSystem handcuffSubmitParticle;
    [SerializeField] private AudioSource handcuffAudio;

    private void Start()
    {
        NotifyHandcuffChanged();
    }

    public void AddHandCuffs(int amount)
    {
        if (amount <= 0)
        {
            return;
        }

        currentHandcuffCount += amount; //수갑 수량 증가

        handcuffClaimParticle.Play();
        handcuffAudio.Play();
        NotifyHandcuffChanged();
    }

    public int RemoveAllHandcuffs()
    {
        int removedAmount = currentHandcuffCount;
        currentHandcuffCount = 0;

        handcuffSubmitParticle.Play();
        handcuffAudio.Play();
        NotifyHandcuffChanged();

        return removedAmount;
    }

    private void NotifyHandcuffChanged()
    {
        OnHandcuffChanged?.Invoke(currentHandcuffCount);
    }
}
