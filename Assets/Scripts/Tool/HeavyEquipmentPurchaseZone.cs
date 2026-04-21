using UnityEngine;

public class HeavyEquipmentPurchaseZone : InstallmentPurchaseZoneBase
{
    [Header("References")]
    [SerializeField] private EquipmentData heavyEquipmentData;

    protected override int GetRequiredCost()
    {
        return heavyEquipmentData != null ? heavyEquipmentData.cost : 0;
    }

    protected override bool TryCompletePurchase(Collider other)
    {
        if (heavyEquipmentData == null)
        {
            return false;
        }

        PlayerEquipmentController equipmentController =
            other.GetComponent<PlayerEquipmentController>() ??
            other.GetComponentInParent<PlayerEquipmentController>();

        if (equipmentController == null)
        {
            return false;
        }

        if (equipmentController.HasEquipment(heavyEquipmentData))
        {
            return true;
        }

        return equipmentController.TryAcquireEquipment(heavyEquipmentData);
    }
}
