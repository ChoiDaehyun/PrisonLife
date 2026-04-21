using UnityEngine;

public class PlayerEquipmentVisualController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerEquipmentController playerEquipmentController;

    [Header("Character Visuals")]
    [SerializeField] private GameObject baseCharacter;
    [SerializeField] private GameObject pickVisual;
    [SerializeField] private GameObject drillVisual;
    [SerializeField] private GameObject heavyEquipmentVisual;

    [Header("Visual Animation Controllers")]
    [SerializeField] private PickaxeSwingVisual pickaxeSwingVisual;
    [SerializeField] private DrillMoveVisual drillMoveVisual;

    [Header("Equipment Data")]
    [SerializeField] private EquipmentData drillEquipmentData;
    [SerializeField] private EquipmentData heavyEquipmentData;

    [Header("Runtime")]
    [SerializeField] private bool isInsideEquipmentVisualZone;

    private void Start()
    {
        ApplyDefaultState();
    }

    public void EnterVisualZone()
    {
        isInsideEquipmentVisualZone = true;
        RefreshVisual();
    }

    public void ExitVisualZone()
    {
        isInsideEquipmentVisualZone = false;
        ApplyDefaultState();
    }
    public void RefreshVisual()
    {
        if (!isInsideEquipmentVisualZone)
        {
            ApplyDefaultState();
            return;
        }

        HideAllEquipmentVisuals();

        if (playerEquipmentController == null)
        {
            ApplyPickVisualState();
            return;
        }

        bool hasHeavyEquipment = heavyEquipmentData != null && playerEquipmentController.HasEquipment(heavyEquipmentData);
        bool hasDrill = drillEquipmentData != null && playerEquipmentController.HasEquipment(drillEquipmentData);

        if (hasHeavyEquipment)
        {
            ApplyHeavyEquipmentVisualState();
            return;
        }

        if (hasDrill)
        {
            ApplyDrillVisualState();
            return;
        }

        ApplyPickVisualState();
    }

    private void ApplyDefaultState()
    {
        HideAllEquipmentVisuals();

        if (baseCharacter != null)
        {
            baseCharacter.SetActive(true);
        }

        StopPickaxeSwing();
        StopDrillMove();
    }

    private void ApplyPickVisualState()
    {
        if (baseCharacter != null)
        {
            baseCharacter.SetActive(true);
        }

        if (pickVisual != null)
        {
            pickVisual.SetActive(true);
        }

        if (drillVisual != null)
        {
            drillVisual.SetActive(false);
        }

        if (heavyEquipmentVisual != null)
        {
            heavyEquipmentVisual.SetActive(false);
        }

        StartPickaxeSwing();
        StopDrillMove();
    }

    private void ApplyDrillVisualState()
    {
        if (baseCharacter != null)
        {
            baseCharacter.SetActive(true);
        }

        if (pickVisual != null)
        {
            pickVisual.SetActive(false);
        }

        if (drillVisual != null)
        {
            drillVisual.SetActive(true);
        }

        if (heavyEquipmentVisual != null)
        {
            heavyEquipmentVisual.SetActive(false);
        }

        StopPickaxeSwing();
        StartDrillMove();
    }

    private void ApplyHeavyEquipmentVisualState()
    {
        if (baseCharacter != null)
        {
            baseCharacter.SetActive(false);
        }

        if (pickVisual != null)
        {
            pickVisual.SetActive(false);
        }

        if (drillVisual != null)
        {
            drillVisual.SetActive(false);
        }

        if (heavyEquipmentVisual != null)
        {
            heavyEquipmentVisual.SetActive(true);
        }

        StopPickaxeSwing();
        StopDrillMove();
    }

    private void HideAllEquipmentVisuals()
    {
        if (pickVisual != null)
        {
            pickVisual.SetActive(false);
        }

        if (drillVisual != null)
        {
            drillVisual.SetActive(false);
        }

        if (heavyEquipmentVisual != null)
        {
            heavyEquipmentVisual.SetActive(false);
        }
    }

    private void StartPickaxeSwing()
    {
        if (pickaxeSwingVisual != null)
        {
            pickaxeSwingVisual.StartSwing();
        }
    }

    private void StopPickaxeSwing()
    {
        if (pickaxeSwingVisual != null)
        {
            pickaxeSwingVisual.StopSwing();
        }
    }

    private void StartDrillMove()
    {
        if (drillMoveVisual != null)
        {
            drillMoveVisual.StartMove();
        }
    }

    private void StopDrillMove()
    {
        if (drillMoveVisual != null)
        {
            drillMoveVisual.StopMove();
        }
    }
}