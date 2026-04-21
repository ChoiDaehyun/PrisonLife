using System.Collections.Generic;
using UnityEngine;

public class MoneyStoredStackVisual : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MoneyStorage moneyStorage;
    [SerializeField] private GameObject moneyVisualPrefab;

    [Header("Stack Settings")]
    [SerializeField] private Vector3 baseLocalPosition = Vector3.zero;
    [SerializeField] private float yStep = 0.25f;

    private readonly Stack<GameObject> moneyVisualStack = new Stack<GameObject>();

    private void OnEnable()
    {
        if (moneyStorage != null)
        {
            moneyStorage.OnStoredMoneyChanged += HandleStoredMoneyChanged;
        }
    }

    private void OnDisable()
    {
        if (moneyStorage != null)
        {
            moneyStorage.OnStoredMoneyChanged -= HandleStoredMoneyChanged;
        }
    }

    private void Start()
    {
        Refresh();
    }

    private void HandleStoredMoneyChanged(int storedMoney)
    {
        int targetVisualCount = storedMoney / 10;
        SyncToCount(targetVisualCount);
    }

    private void Refresh()
    {
        if (moneyStorage == null)
        {
            return;
        }

        int targetVisualCount = moneyStorage.StoredMoney / 10;
        SyncToCount(targetVisualCount);
    }

    private void SyncToCount(int targetCount)
    {
        targetCount = Mathf.Max(0, targetCount);

        while (moneyVisualStack.Count < targetCount)
        {
            PushMoneyVisual();
        }

        while (moneyVisualStack.Count > targetCount)
        {
            PopMoneyVisual();
        }
    }

    private void PushMoneyVisual()
    {
        if (moneyVisualPrefab == null)
        {
            return;
        }

        GameObject instance = Instantiate(moneyVisualPrefab, transform);

        Vector3 localPosition = baseLocalPosition;

        if (moneyVisualStack.Count > 0)
        {
            GameObject topObject = moneyVisualStack.Peek();

            if (topObject != null)
            {
                localPosition = topObject.transform.localPosition + Vector3.up * yStep;
            }
            else
            {
                localPosition = baseLocalPosition + Vector3.up * (moneyVisualStack.Count * yStep);
            }
        }

        instance.transform.localPosition = localPosition;
        instance.transform.localRotation = Quaternion.identity;
        instance.transform.localScale = new Vector3(0.617410004f, 100f, 0.360448509f);

        moneyVisualStack.Push(instance);
    }

    private void PopMoneyVisual()
    {
        if (moneyVisualStack.Count <= 0)
        {
            return;
        }

        GameObject topObject = moneyVisualStack.Pop();

        if (topObject != null)
        {
            Destroy(topObject);
        }
    }

    public void ClearAll()
    {
        while (moneyVisualStack.Count > 0)
        {
            PopMoneyVisual();
        }
    }
}