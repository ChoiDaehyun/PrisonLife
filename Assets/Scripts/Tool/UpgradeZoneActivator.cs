using UnityEngine;

public class UpgradeZoneActivator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PrisonerJailInventory prisonerJailInventory;
    [SerializeField] private GameObject upgradeZoneObject;

    private void Awake()
    {
        if (upgradeZoneObject != null)
        {
            upgradeZoneObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        if (prisonerJailInventory != null)
        {
            prisonerJailInventory.OnJailedCountChanged += HandleJailedCountChanged;
        }
    }

    private void OnDisable()
    {
        if (prisonerJailInventory != null)
        {
            prisonerJailInventory.OnJailedCountChanged -= HandleJailedCountChanged;
        }
    }

    private void Start()
    {
        RefreshZoneState();
    }

    private void HandleJailedCountChanged(int currentCount, int maxCount)
    {
        RefreshZoneState();
    }

    private void RefreshZoneState()
    {
        if (upgradeZoneObject == null || prisonerJailInventory == null)
        {
            return;
        }

        bool shouldActivate = prisonerJailInventory.IsFull;
        upgradeZoneObject.SetActive(shouldActivate);
    }
}
