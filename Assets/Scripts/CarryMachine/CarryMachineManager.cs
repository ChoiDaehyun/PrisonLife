using System;
using UnityEngine;

public class CarryMachineManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CarryMachineData carryMachineData;
    [SerializeField] private CarryMachineUnit carryMachinePrefab;
    [SerializeField] private HandcuffStorage claimHandcuffStorage;
    [SerializeField] private HandcuffStorage submitHandcuffStorage;

    [SerializeField] private ParticleSystem carryPart;
    [SerializeField] private AudioSource carryAudio;

    [Header("Runtime")]
    [SerializeField] private bool hasPurchased;
    [SerializeField] private CarryMachineUnit spawnedUnit;

    public event Action OnCarryMachinePurchased;

    public bool HasPurchased => hasPurchased;

    public bool PurchaseAndSpawn()
    {
        if (hasPurchased)
        {
            return false;
        }

        if (carryMachineData == null ||
            carryMachinePrefab == null ||
            claimHandcuffStorage == null ||
            submitHandcuffStorage == null)
        {
            return false;
        }
        carryPart.Play();
        carryAudio.Play();
        spawnedUnit = Instantiate(carryMachinePrefab, transform);
        spawnedUnit.Initialize(carryMachineData, claimHandcuffStorage, submitHandcuffStorage);

        hasPurchased = true;

        OnCarryMachinePurchased?.Invoke();

        return true;
    }
}