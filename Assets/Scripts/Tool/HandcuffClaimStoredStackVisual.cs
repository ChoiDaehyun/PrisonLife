using System.Collections.Generic;
using UnityEngine;

public class HandcuffClaimStoredStackVisual : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private HandcuffStorage claimHandcuffStorage;
    [SerializeField] private GameObject handcuffVisualPrefab;

    [Header("Stack Settings")]
    [SerializeField] private Vector3 baseLocalPosition = new Vector3(0f, 50f, 0f);
    [SerializeField] private float yStep = 55f;

    private readonly Stack<GameObject> handcuffVisualStack = new Stack<GameObject>();

    private void OnEnable()
    {
        if (claimHandcuffStorage != null)
        {
            claimHandcuffStorage.OnClaimStoredHandcuffChanged += HandleClaimStoredHandcuffChanged;
        }
    }

    private void OnDisable()
    {
        if (claimHandcuffStorage != null)
        {
            claimHandcuffStorage.OnClaimStoredHandcuffChanged -= HandleClaimStoredHandcuffChanged;
        }
    }

    private void Start()
    {
        Refresh();
    }

    private void HandleClaimStoredHandcuffChanged(int claimStoredHandcuffCount)
    {
        SyncToCount(claimStoredHandcuffCount);
    }

    private void Refresh()
    {
        if (claimHandcuffStorage == null)
        {
            return;
        }

        SyncToCount(claimHandcuffStorage.ClaimStoredHandcuffCount);
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
        instance.transform.localScale = new Vector3(0.478624642f, 96.4391174f, 0.438933283f);

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