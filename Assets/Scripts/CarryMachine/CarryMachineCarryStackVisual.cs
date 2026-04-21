using System.Collections.Generic;
using UnityEngine;

public class CarryMachineCarryStackVisual : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CarryMachineUnit carryMachineUnit;
    [SerializeField] private GameObject handcuffVisualPrefab;

    [Header("Stack Settings")]
    [SerializeField] private Vector3 baseLocalPosition = Vector3.zero;
    [SerializeField] private float yStep = 0.25f;

    private readonly Stack<GameObject> handcuffVisualStack = new Stack<GameObject>();

    private void OnEnable()
    {
        if (carryMachineUnit != null)
        {
            carryMachineUnit.OnCarriedAmountChanged += HandleCarriedAmountChanged;
        }
    }

    private void OnDisable()
    {
        if (carryMachineUnit != null)
        {
            carryMachineUnit.OnCarriedAmountChanged -= HandleCarriedAmountChanged;
        }
    }

    private void Start()
    {
        Refresh();
        Debug.Log("[CarryMachineCarryStackVisual] Initialized.");
    }

    private void HandleCarriedAmountChanged(int carriedAmount)
    {
        SyncToCount(carriedAmount);
    }

    private void Refresh()
    {
        if (carryMachineUnit == null)
        {
            return;
        }

        SyncToCount(carryMachineUnit.CarriedAmount);
    }

    private void SyncToCount(int targetCount)
    {
        targetCount = Mathf.Max(0, targetCount);

        while (handcuffVisualStack.Count < targetCount)
        {
            PushHandcuffVisual();
        }

        while (handcuffVisualStack.Count > targetCount)
        {
            PopHandcuffVisual();
        }
    }

    private void PushHandcuffVisual()
    {
        if (handcuffVisualPrefab == null)
        {
            return;
        }

        GameObject instance = Instantiate(handcuffVisualPrefab, transform);

        Vector3 localPosition = baseLocalPosition;

        if (handcuffVisualStack.Count > 0)
        {
            GameObject topObject = handcuffVisualStack.Peek();

            if (topObject != null)
            {
                localPosition = topObject.transform.localPosition + Vector3.up * yStep;
            }
            else
            {
                localPosition = baseLocalPosition + Vector3.up * (handcuffVisualStack.Count * yStep);
            }
        }

        instance.transform.localPosition = localPosition;
        instance.transform.localRotation = Quaternion.identity;
        instance.transform.localScale = new Vector3(1f, 0.2f, 0.43938747f);

        handcuffVisualStack.Push(instance);
    }

    private void PopHandcuffVisual()
    {
        if (handcuffVisualStack.Count <= 0)
        {
            return;
        }

        GameObject topObject = handcuffVisualStack.Pop();

        if (topObject != null)
        {
            Destroy(topObject);
        }
    }

    public void ClearAll()
    {
        while (handcuffVisualStack.Count > 0)
        {
            PopHandcuffVisual();
        }
    }
}