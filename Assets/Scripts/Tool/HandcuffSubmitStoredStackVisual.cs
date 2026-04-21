using System.Collections.Generic;
using UnityEngine;

public class HandcuffSubmitStoredStackVisual : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private HandcuffStorage submitHandcuffStorage;
    [SerializeField] private GameObject handcuffVisualPrefab;

    [Header("Stack Settings")]
    [SerializeField] private Vector3 baseLocalPosition = Vector3.zero;
    [SerializeField] private float yStep = 0.25f;

    private readonly Stack<GameObject> handcuffVisualStack = new Stack<GameObject>();

    private void OnEnable()
    {
        if (submitHandcuffStorage != null)
        {
            submitHandcuffStorage.OnSubmittedStoredHandcuffChanged += HandleSubmittedStoredHandcuffChanged;
        }
    }

    private void OnDisable()
    {
        if (submitHandcuffStorage != null)
        {
            submitHandcuffStorage.OnSubmittedStoredHandcuffChanged -= HandleSubmittedStoredHandcuffChanged;
        }
    }

    private void Start()
    {
        Refresh();
    }

    private void HandleSubmittedStoredHandcuffChanged(int submittedStoredHandcuffCount)
    {
        SyncToCount(submittedStoredHandcuffCount);
    }

    private void Refresh()
    {
        if (submitHandcuffStorage == null)
        {
            return;
        }

        SyncToCount(submitHandcuffStorage.SubmittedStoredHandcuffCount);
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
        instance.transform.localScale = new Vector3(0.295213252f, 96.4391174f, 0.28514424f);

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