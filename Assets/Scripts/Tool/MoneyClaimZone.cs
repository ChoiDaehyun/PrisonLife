using UnityEngine;

[RequireComponent(typeof(Collider))]
public class MoneyClaimZone : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MoneyStorage moneyStorage;

    [Header("Settings")]
    [SerializeField] private string playerTag = "Player";

    private void Reset()
    {
        Collider zoneCollider = GetComponent<Collider>();
        zoneCollider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag))
        {
            return;
        }

        PlayerMoneyInventory moneyInventory = other.GetComponent<PlayerMoneyInventory>();

        //혹시나 자식 오브젝트에 Collider가 있다면
        if (moneyInventory == null)
        {
            moneyInventory = other.GetComponentInParent<PlayerMoneyInventory>();
        }
        if (moneyStorage == null)
        {
            return;
        }
        if (moneyInventory == null)
        {
            return;
        }

        int claimedMoney = moneyStorage.ClaimAllMoney();

        if (claimedMoney <= 0)
        {
            return;
        }

        moneyInventory.AddMoney(claimedMoney);
    }
}
