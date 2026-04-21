using UnityEngine;

public class CarryMachineZoneActivator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private WorkerManager workerManager;
    [SerializeField] private CarryMachineManager carryMachineManager;
    [SerializeField] private GameObject carryMachineZoneObject;

    private void Awake()
    {
        if (carryMachineZoneObject != null)
        {
            carryMachineZoneObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        if (workerManager != null)
        {
            workerManager.OnDefaultWorkersHired += HandleWorkersHired;
        }

        if (carryMachineManager != null)
        {
            carryMachineManager.OnCarryMachinePurchased += HandleCarryMachinePurchased;
        }
    }

    private void OnDisable()
    {
        if (workerManager != null)
        {
            workerManager.OnDefaultWorkersHired -= HandleWorkersHired;
        }

        if (carryMachineManager != null)
        {
            carryMachineManager.OnCarryMachinePurchased -= HandleCarryMachinePurchased;
        }
    }

    private void Start()
    {
        if (carryMachineZoneObject == null)
        {
            return;
        }

        if (carryMachineManager != null && carryMachineManager.HasPurchased)
        {
            carryMachineZoneObject.SetActive(false);
            return;
        }

        if (workerManager != null && workerManager.HasHiredDefaultWorkers)
        {
            carryMachineZoneObject.SetActive(true);
        }
        else
        {
            carryMachineZoneObject.SetActive(false);
        }
    }

    private void HandleWorkersHired()
    {
        if (carryMachineZoneObject == null)
        {
            return;
        }

        if (carryMachineManager != null && carryMachineManager.HasPurchased)
        {
            carryMachineZoneObject.SetActive(false);
            return;
        }

        carryMachineZoneObject.SetActive(true);
    }

    private void HandleCarryMachinePurchased()
    {
        if (carryMachineZoneObject == null)
        {
            return;
        }

        carryMachineZoneObject.SetActive(false);
    }
}