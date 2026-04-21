using UnityEngine;

public class HeavyEquipmentPurchaseZoneActivator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerEquipmentController playerEquipmentController;
    [SerializeField] private EquipmentData drillEquipmentData;
    [SerializeField] private EquipmentData heavyEquipmentData;
    [SerializeField] private GameObject heavyEquipmentPurchaseZoneObject;

    private void Awake()
    {
        if (heavyEquipmentPurchaseZoneObject != null)
        {
            heavyEquipmentPurchaseZoneObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        if (playerEquipmentController != null)
        {
            playerEquipmentController.OnEquipmentAcquired += HandleEquipmentAcquired;
        }
    }

    private void OnDisable()
    {
        if (playerEquipmentController != null)
        {
            playerEquipmentController.OnEquipmentAcquired -= HandleEquipmentAcquired;
        }
    }

    private void Start()
    {
        if (heavyEquipmentPurchaseZoneObject == null)
        {
            return;
        }

        if (playerEquipmentController == null)
        {
            return;
        }

        bool hasDrill = playerEquipmentController.HasEquipment(drillEquipmentData);
        bool hasHeavy = playerEquipmentController.HasEquipment(heavyEquipmentData);

        if (hasHeavy)
        {
            heavyEquipmentPurchaseZoneObject.SetActive(false);
            return;
        }

        heavyEquipmentPurchaseZoneObject.SetActive(hasDrill);
    }

    private void HandleEquipmentAcquired(EquipmentData acquiredEquipment)
    {
        if (acquiredEquipment == null || heavyEquipmentPurchaseZoneObject == null || playerEquipmentController == null)
        {
            return;
        }

        if (heavyEquipmentData != null && acquiredEquipment.equipmentId == heavyEquipmentData.equipmentId)
        {
            heavyEquipmentPurchaseZoneObject.SetActive(false);
            return;
        }

        if (drillEquipmentData != null && acquiredEquipment.equipmentId == drillEquipmentData.equipmentId)
        {
            bool hasHeavy = playerEquipmentController.HasEquipment(heavyEquipmentData);

            if (!hasHeavy)
            {
                heavyEquipmentPurchaseZoneObject.SetActive(true);
            }
        }
    }

    private void OnValidate()
    {
        if (playerEquipmentController == null)
        {

        }

        if (heavyEquipmentPurchaseZoneObject == null)
        {

        }
    }
}