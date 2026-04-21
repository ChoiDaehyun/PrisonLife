using System;
using UnityEngine;

public class PlayerResourceInventory : MonoBehaviour, IOreSupplier
{
    [Header("References")]
    [SerializeField] private PlayerInventoryData inventoryData;
    [SerializeField] private PlayerEquipmentController playerEquipmentController;
    [SerializeField] private EquipmentData heavyEquipmentData;

    [Header("Runtime")]
    [SerializeField] private int currentOreCount;

    public event Action<int, int> OnOreChanged; // <현재 개수, 최대 용량>
    public event Action<int> OnOreDeposited;
    public event Action<int> OnOreSupplied;

    public int CurrentOreCount => currentOreCount;

    public int MaxOreCapacity
    {
        get
        {
            if (inventoryData == null)
            {
                return 0;
            }

            bool hasHeavyEquipment =
                playerEquipmentController != null &&
                heavyEquipmentData != null &&
                playerEquipmentController.HasEquipment(heavyEquipmentData);

            if (hasHeavyEquipment)
            {
                return 15;
            }

            return inventoryData.maxOreCapacity;
        }
    }

    private void Start()
    {
        NotifyOreChanged();
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

    private void HandleEquipmentAcquired(EquipmentData acquiredEquipment)
    {
        if (heavyEquipmentData == null || acquiredEquipment == null)
        {
            return;
        }

        if (acquiredEquipment.equipmentId != heavyEquipmentData.equipmentId)
        {
            return;
        }

        NotifyOreChanged();
    }

    // 광물 추가 가능 여부 검사
    public bool CanAddOre(int amount = 1)
    {
        if (inventoryData == null)
        {
            return false;
        }

        return currentOreCount + amount <= MaxOreCapacity;
    }

    // 광물 추가 시도
    public bool TryAddOre(int amount = 1)
    {
        if (inventoryData == null)
        {
            return false;
        }

        if (amount <= 0)
        {
            return false;
        }

        if (!CanAddOre(amount))
        {
            return false;
        }

        currentOreCount += amount;

        NotifyOreChanged();

        return true;
    }

    public int DepositAllOre()
    {
        if (currentOreCount <= 0)
        {
            return 0;
        }

        int depositedAmount = currentOreCount;
        currentOreCount = 0;

        NotifyOreChanged();
        OnOreDeposited?.Invoke(depositedAmount);
        OnOreSupplied?.Invoke(depositedAmount);

        return depositedAmount;
    }

    public int RemoveAllOre()
    {
        int removedAmount = currentOreCount;
        currentOreCount = 0;

        NotifyOreChanged();

        return removedAmount;
    }

    private void NotifyOreChanged()
    {
        OnOreChanged?.Invoke(currentOreCount, MaxOreCapacity);
    }
}
