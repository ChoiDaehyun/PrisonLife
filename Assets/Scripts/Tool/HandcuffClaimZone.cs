using UnityEngine;

[RequireComponent(typeof(Collider))]
public class HandcuffClaimZone : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private HandcuffStorage handcuffStorage;

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

        if (handcuffStorage == null)
        {
            return;
        }

        PlayerHandcuffInventory handcuffInventory =
            other.GetComponent<PlayerHandcuffInventory>() ??
            other.GetComponentInParent<PlayerHandcuffInventory>();

        if (handcuffInventory == null)
        {
            return;
        }

        int claimedAmount = handcuffStorage.ClaimAllHandcuffs();

        if (claimedAmount <= 0)
        {
            return;
        }

        handcuffInventory.AddHandCuffs(claimedAmount);

    }
}