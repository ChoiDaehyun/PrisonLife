using UnityEngine;

public class DrillPurchaseZoneActivator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerMoneyInventory playerMoneyInventory;
    [SerializeField] private PlayerEquipmentController playerEquipmentController;
    [SerializeField] private EquipmentData drillEquipmentData;
    [SerializeField] private GameObject drillPurchaseZoneObject;

    private void Awake()
    {
        if (drillPurchaseZoneObject != null)
        {
            drillPurchaseZoneObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        if (playerMoneyInventory != null)
        {
            playerMoneyInventory.OnFirstMoneyReceived += HandleFirstMoneyReceived;
        }
    }
    private void OneDisable()
    {
        if (playerMoneyInventory != null)
        {
            playerMoneyInventory.OnFirstMoneyReceived -= HandleFirstMoneyReceived;
        }
    }

    private void Start()
    {
        if (drillPurchaseZoneObject == null)
        {
            return;
        }
        if (playerEquipmentController != null &&
            drillEquipmentData != null &&
            playerEquipmentController.HasEquipment(drillEquipmentData))
        {
            drillPurchaseZoneObject.SetActive(false);
            return;
        }

        if (playerMoneyInventory != null && playerMoneyInventory.HasReceivedMoneyAtLeastOnce)
        {
            drillPurchaseZoneObject.SetActive(true);
        }
        else
        {
            drillPurchaseZoneObject.SetActive(false);
        }
    }

    private void HandleFirstMoneyReceived()
    {
        if (drillPurchaseZoneObject == null)
        {
            return;
        }
        if (playerEquipmentController != null &&
            drillEquipmentData != null &&
            playerEquipmentController.HasEquipment(drillEquipmentData))
        {
            drillPurchaseZoneObject.SetActive(false);
            return;
        }
        drillPurchaseZoneObject.SetActive(true);
    }
}
