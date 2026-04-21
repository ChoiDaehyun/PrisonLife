using UnityEngine;

public class WorkerHireZoneActivator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerEquipmentController playerEquipmentController;
    [SerializeField] private EquipmentData drillEquipmentData;
    [SerializeField] private WorkerManager workerManager;
    [SerializeField] private GameObject workerHireZoneObject;

    private void Awake()
    {
        if (workerHireZoneObject != null)
        {
            workerHireZoneObject.SetActive(false);
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
        if (workerHireZoneObject == null ||
            playerEquipmentController == null ||
            workerManager == null)
        {
            return;
        }

        if (workerManager.HasHiredDefaultWorkers)
        {
            workerHireZoneObject.SetActive(false);
            return;
        }

        bool hasDrill = playerEquipmentController.HasEquipment(drillEquipmentData);
        workerHireZoneObject.SetActive(hasDrill);
    }

    private void HandleEquipmentAcquired(EquipmentData acquiredEquipment)
    {
        if (acquiredEquipment == null || workerHireZoneObject == null || workerManager == null)
        {
            return;
        }

        if (workerManager.HasHiredDefaultWorkers)
        {
            workerHireZoneObject.SetActive(false);
            return;
        }

        if (drillEquipmentData != null && acquiredEquipment.equipmentId == drillEquipmentData.equipmentId)
        {
            workerHireZoneObject.SetActive(true);
        }
    }
}