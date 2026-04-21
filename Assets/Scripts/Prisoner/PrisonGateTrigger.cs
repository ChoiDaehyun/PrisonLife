using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PrisonGateTrigger : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PrisonerJailInventory jailInventory;

    private void Reset()
    {
        Collider col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        PrisonerUnit prisoner = other.GetComponent<PrisonerUnit>();

        if (prisoner == null)
        {
            prisoner = other.GetComponentInParent<PrisonerUnit>();
        }

        if (prisoner == null)
        {
            return;
        }

        if (prisoner.IsJailRegistered)
        {
            return;
        }

        if (jailInventory == null)
        {
            return;
        }

        bool registered = jailInventory.TryRegisterPrisoner();

        if (registered)
        {
            prisoner.MarkJailRegistered();
            prisoner.SetPhysicsActive(true);
        }
    }

    private void OnValidate()
    {
        Collider col = GetComponent<Collider>();
    }
}