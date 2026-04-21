using UnityEngine;

public class CarryMachineZone : InstallmentPurchaseZoneBase
{
    [Header("References")]
    [SerializeField] private CarryMachineData carryMachineData;
    [SerializeField] private CarryMachineManager carryMachineManager;

    protected override int GetRequiredCost()
    {
        return carryMachineData != null ? carryMachineData.purchaseCost : 0;
    }

    protected override bool TryCompletePurchase(Collider other)
    {
        if (carryMachineManager == null)
        {
            return false;
        }

        if (carryMachineManager.HasPurchased)
        {
            return true;
        }

        return carryMachineManager.PurchaseAndSpawn();
    }
}
