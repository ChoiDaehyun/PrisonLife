using System.Collections;
using UnityEngine;

public class HandcuffToMoneyConverter : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private HandcuffStorage handcuffStorage;
    [SerializeField] private MoneyStorage moneyStorage;

    [Header("Settings")]
    [SerializeField] private float processInterval = 1f;
    [SerializeField] private int handcuffPerProcess = 1;
    [SerializeField] private int moneyPerProcess = 10;

    [Header("Runtime")]
    [SerializeField] private int pendingHandcuffQueueCount;
    [SerializeField] private bool isProcessing;

    private Coroutine processingRoutine;

    public int PendingHandcuffQueueCount => pendingHandcuffQueueCount;
    public bool IsProcessing => isProcessing;

    private void OnEnable()
    {
        if (handcuffStorage != null)
        {
            handcuffStorage.OnSubmittedStoredHandcuffChanged += HandleStoredHandcuffChanged;
        }
    }

    private void OnDisable()
    {
        if (handcuffStorage != null)
        {
            handcuffStorage.OnSubmittedStoredHandcuffChanged -= HandleStoredHandcuffChanged;
        }
    }

    private void Start()
    {

    }

    private int lastObservedHandcuffCount;

    private void HandleStoredHandcuffChanged(int currentStoredCount)
    {
        int addedAmount = currentStoredCount - lastObservedHandcuffCount;
        lastObservedHandcuffCount = currentStoredCount;

        if (addedAmount <= 0)
        {
            return;
        }

        pendingHandcuffQueueCount += addedAmount;

        TryStartProcessing();
    }

    private void TryStartProcessing()
    {
        if (isProcessing)
        {
            return;
        }

        if (pendingHandcuffQueueCount <= 0)
        {
            return;
        }

        processingRoutine = StartCoroutine(ProcessQueueRoutine());
    }

    private IEnumerator ProcessQueueRoutine()
    {
        isProcessing = true;

        while (pendingHandcuffQueueCount > 0)
        {
            yield return new WaitForSeconds(processInterval);

            if (pendingHandcuffQueueCount < handcuffPerProcess)
            {
                break;
            }

            pendingHandcuffQueueCount -= handcuffPerProcess;
            moneyStorage.AddMoney(moneyPerProcess);

        }

        isProcessing = false;
        processingRoutine = null;

        if (pendingHandcuffQueueCount > 0)
        {
            TryStartProcessing();
        }
    }
}