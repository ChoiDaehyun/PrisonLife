using System.Collections;
using UnityEngine;

public class OreToHandcuffConverter : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private WarehouseInventory warehouseInventory;
    [SerializeField] private HandcuffStorage handcuffStorage;

    [Header("Settings")]
    [SerializeField] private float processInterval = 1f;
    [SerializeField] private int orePerProcess = 1;
    [SerializeField] private int handcuffPerProcess = 1;

    [Header("Runtime")]
    [SerializeField] private int pendingOreCount;
    [SerializeField] private bool isProcessing;

    private Coroutine processingRoutine;

    public int PendingOreCount => pendingOreCount;
    public bool IsProcessing => isProcessing;

    private void OnEnable()
    {
        if (warehouseInventory != null)
        {
            warehouseInventory.OnOreAdded += HandleOreAdded;
        }
    }

    private void OnDisable()
    {
        if (warehouseInventory != null)
        {
            warehouseInventory.OnOreAdded -= HandleOreAdded;
        }
    }

    private void Start()
    {

    }

    private void HandleOreAdded(int amount)
    {
        if (amount <= 0)
        {
            return;
        }

        pendingOreCount += amount;

        TryStartProcessing();
    }

    private void TryStartProcessing()
    {
        if (isProcessing)
        {
            return;
        }

        if (pendingOreCount <= 0)
        {
            return;
        }

        processingRoutine = StartCoroutine(ProcessQueueRoutine());
    }

    private IEnumerator ProcessQueueRoutine()
    {
        isProcessing = true;

        while (pendingOreCount > 0)
        {
            yield return new WaitForSeconds(processInterval);

            if (pendingOreCount < orePerProcess)
            {
                break;
            }

            pendingOreCount -= orePerProcess;
            handcuffStorage.AddClaimHandcuffs(handcuffPerProcess);

        }

        isProcessing = false;
        processingRoutine = null;

        if (pendingOreCount > 0)
        {
            TryStartProcessing();
        }
    }
}
