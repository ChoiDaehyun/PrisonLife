using System;
using System.Collections;
using UnityEngine;

public class CommonOreToHandcuffConverter : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerResourceInventory playerResourceInventory;
    [SerializeField] private WarehouseInventory warehouseInventory;
    [SerializeField] private HandcuffStorage claimhandcuffStorage;

    [Header("Settings")]
    [SerializeField] private float processInterval = 1f;
    [SerializeField] private int orePerProcess = 1;
    [SerializeField] private int handcuffPerProcess = 1;

    [Header("Runtime")]
    [SerializeField] private int pendingOreQueueCount;
    [SerializeField] private bool isProcessing;

    [Header("P/A")]
    [SerializeField] private ParticleSystem oreTohandcuffParticle;
    [SerializeField] private ParticleSystem oreClaimParticle;
    [SerializeField] private AudioSource[] arrayAudio;

    private Coroutine processingRoutine;
    public event Action<int> OnPendingOreQueueChanged;

    public int PendingOreQueueCount => pendingOreQueueCount;
    public bool IsProcessing => isProcessing;

    private void OnEnable()
    {
        if (playerResourceInventory != null)
        {
            playerResourceInventory.OnOreSupplied += HandleOreSupplied;
        }

        if (warehouseInventory != null)
        {
            warehouseInventory.OnOreSupplied += HandleOreSupplied;
        }
    }

    private void OnDisable()
    {
        if (playerResourceInventory != null)
        {
            playerResourceInventory.OnOreSupplied -= HandleOreSupplied;
        }

        if (warehouseInventory != null)
        {
            warehouseInventory.OnOreSupplied -= HandleOreSupplied;
        }
    }

    private void Start()
    {
        NotifyPendingOreQueueChanged();
    }

    private void HandleOreSupplied(int amount)
    {
        if (amount <= 0)
        {
            return;
        }

        pendingOreQueueCount += amount;

        oreClaimParticle.Play();
        arrayAudio[1].Play();
        NotifyPendingOreQueueChanged();

        TryStartProcessing();
    }

    private void TryStartProcessing()
    {
        if (isProcessing)
        {
            return;
        }

        if (pendingOreQueueCount <= 0)
        {
            return;
        }

        processingRoutine = StartCoroutine(ProcessQueueRoutine());
    }

    private IEnumerator ProcessQueueRoutine()
    {
        isProcessing = true;

        while (pendingOreQueueCount > 0)
        {
            yield return new WaitForSeconds(processInterval);

            if (pendingOreQueueCount < orePerProcess)
            {
                break;
            }

            pendingOreQueueCount -= orePerProcess;
            NotifyPendingOreQueueChanged();

            oreTohandcuffParticle.Play();
            arrayAudio[0].Play();

            claimhandcuffStorage.AddClaimHandcuffs(handcuffPerProcess);
        }

        isProcessing = false;
        processingRoutine = null;

        if (pendingOreQueueCount > 0)
        {
            TryStartProcessing();
        }
    }

    private void NotifyPendingOreQueueChanged()
    {
        OnPendingOreQueueChanged?.Invoke(pendingOreQueueCount);
    }
}
