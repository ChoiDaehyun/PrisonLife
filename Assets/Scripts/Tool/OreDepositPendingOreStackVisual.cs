using System.Collections.Generic;
using UnityEngine;

public class OreDepositPendingOreStackVisual : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CommonOreToHandcuffConverter commonOreToHandcuffConverter;
    [SerializeField] private GameObject oreVisualPrefab;

    [Header("Stack Settings")]
    [SerializeField] private Vector3 baseLocalPosition = new Vector3(0f, 50f, 0f);
    [SerializeField] private float yStep = 55f;

    private readonly Stack<GameObject> oreVisualStack = new Stack<GameObject>();

    private void OnEnable()
    {
        if (commonOreToHandcuffConverter != null)
        {
            commonOreToHandcuffConverter.OnPendingOreQueueChanged += HandlePendingOreQueueChanged;
        }
    }

    private void OnDisable()
    {
        if (commonOreToHandcuffConverter != null)
        {
            commonOreToHandcuffConverter.OnPendingOreQueueChanged -= HandlePendingOreQueueChanged;
        }
    }

    private void Start()
    {
        Refresh();
    }

    private void HandlePendingOreQueueChanged(int pendingOreQueueCount)
    {
        SyncToCount(pendingOreQueueCount);
    }

    private void Refresh()
    {
        if (commonOreToHandcuffConverter == null)
        {
            return;
        }

        SyncToCount(commonOreToHandcuffConverter.PendingOreQueueCount);
    }

    private void SyncToCount(int targetCount)
    {
        targetCount = Mathf.Max(0, targetCount);

        while (oreVisualStack.Count < targetCount)
        {
            PushOreVisual();
        }

        while (oreVisualStack.Count > targetCount)
        {
            PopOreVisual();
        }
    }

    private void PushOreVisual()
    {
        if (oreVisualPrefab == null)
        {
            return;
        }

        GameObject instance = Instantiate(oreVisualPrefab, transform);

        Vector3 localPosition = baseLocalPosition;

        if (oreVisualStack.Count > 0)
        {
            GameObject topObject = oreVisualStack.Peek();

            if (topObject != null)
            {
                localPosition = topObject.transform.localPosition + Vector3.up * yStep;
            }
            else
            {
                localPosition = baseLocalPosition + Vector3.up * (oreVisualStack.Count * yStep);
            }
        }

        instance.transform.localPosition = localPosition;
        instance.transform.localRotation = Quaternion.identity;
        instance.transform.localScale = new Vector3(0.781530023f, 96.4391174f, 0.587988317f);

        oreVisualStack.Push(instance);
    }

    private void PopOreVisual()
    {
        if (oreVisualStack.Count <= 0)
        {
            return;
        }

        GameObject topObject = oreVisualStack.Pop();

        if (topObject != null)
        {
            Destroy(topObject);
        }
    }

    public void ClearAll()
    {
        while (oreVisualStack.Count > 0)
        {
            PopOreVisual();
        }
    }
}