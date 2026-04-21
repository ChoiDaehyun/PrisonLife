using UnityEngine;

public class DrillPurchaseZone : InstallmentPurchaseZoneBase
{
    [Header("References")]
    [SerializeField] private EquipmentData drillEquipmentData;

    protected override int GetRequiredCost()
    {
        return drillEquipmentData != null ? drillEquipmentData.cost : 0;
    }

    protected override bool TryCompletePurchase(Collider other)
    {
        if (drillEquipmentData == null)
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

        if (equipmentController.HasEquipment(drillEquipmentData))
        {
            return true;
        }

        return equipmentController.TryAcquireEquipment(drillEquipmentData);
    }

    protected override void OnValidate()
    {
        base.OnValidate();
    }
}
