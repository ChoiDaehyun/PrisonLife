using UnityEngine;

public class WorkerHireZone : InstallmentPurchaseZoneBase
{
    [Header("References")]
    [SerializeField] private WorkerSystemData workerSystemData;
    [SerializeField] private WorkerManager workerManager;

    protected override int GetRequiredCost()
    {
        return workerSystemData != null ? workerSystemData.hireCost : 0;
    }

    protected override bool TryCompletePurchase(Collider other)
    {
        if (workerManager == null)
        {
            return false;
        }

        if (workerManager.HasHiredDefaultWorkers)
        {
            return true;
        }

        return workerManager.HireDefaultWorkers();
    }

    protected override void OnValidate()
    {
        base.OnValidate();
    }
}
