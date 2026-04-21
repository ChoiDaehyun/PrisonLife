using UnityEngine;

[RequireComponent(typeof(Collider))]
public class EquipmentVisualTriggerZone : MonoBehaviour
{
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

        PlayerEquipmentVisualController visualController =
            other.GetComponent<PlayerEquipmentVisualController>() ??
            other.GetComponentInParent<PlayerEquipmentVisualController>();

        if (visualController == null)
        {
            return;
        }

        visualController.EnterVisualZone();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(playerTag))
        {
            return;
        }

        PlayerEquipmentVisualController visualController =
            other.GetComponent<PlayerEquipmentVisualController>() ??
            other.GetComponentInParent<PlayerEquipmentVisualController>();

        if (visualController == null)
        {
            return;
        }

        visualController.ExitVisualZone();
    }
}
