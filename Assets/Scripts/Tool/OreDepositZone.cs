using UnityEngine;

[RequireComponent(typeof(Collider))]
public class OreDepositZone : MonoBehaviour
{
    [Header("Setting")]
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

        PlayerResourceInventory resourceInventory = other.GetComponent<PlayerResourceInventory>();

        //혹시나 자식 오브젝트에 Collider가 있다면
        if (resourceInventory == null)
        {
            resourceInventory = other.GetComponentInParent<PlayerResourceInventory>();
        }
        //인벤토리 못 찾으면
        if (resourceInventory == null)
        {
            return;
        }

        int depositedAmount = resourceInventory.DepositAllOre();

        if (depositedAmount <= 0)
        {
            return;
        }
    }
}
