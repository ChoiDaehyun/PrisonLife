using System.Collections.Generic;
using UnityEngine;

public class PlayerCarryStackVisualController : MonoBehaviour
{
    [System.Serializable]
    private class VisualStackData
    {
        [Header("Prefab")]
        public GameObject prefab;

        [Header("Base Local Position")]
        public Vector3 baseLocalPosition;

        [Header("Stack Settings")]
        public float yStep = 0.25f;

        private Stack<GameObject> stack = new Stack<GameObject>();

        public int Count => stack.Count;

        public void SyncToCount(Transform parent, int targetCount)
        {
            targetCount = Mathf.Max(0, targetCount);

            while (stack.Count < targetCount)
            {
                Push(parent);
            }

            while (stack.Count > targetCount)
            {
                Pop();
            }
        }

        private void Push(Transform parent)
        {
            if (prefab == null)
            {
                return;
            }

            GameObject instance = Object.Instantiate(prefab, parent);

            Vector3 localPosition = baseLocalPosition;

            if (stack.Count > 0)
            {
                GameObject topObject = stack.Peek();

                if (topObject != null)
                {
                    localPosition = topObject.transform.localPosition + Vector3.up * yStep;
                }
                else
                {
                    localPosition = baseLocalPosition + Vector3.up * (stack.Count * yStep);
                }
            }

            instance.transform.localPosition = localPosition;
            instance.transform.localRotation = Quaternion.identity;
            instance.transform.localScale = new Vector3(1f, 0.2f, 0.43938747f);

            stack.Push(instance);
        }

        private void Pop()
        {
            if (stack.Count <= 0)
            {
                return;
            }

            GameObject topObject = stack.Pop();

            if (topObject != null)
            {
                Object.Destroy(topObject);
            }
        }

        public void Clear()
        {
            while (stack.Count > 0)
            {
                Pop();
            }
        }
    }

    [Header("References")]
    [SerializeField] private PlayerResourceInventory playerResourceInventory;
    [SerializeField] private PlayerMoneyInventory playerMoneyInventory;
    [SerializeField] private PlayerHandcuffInventory playerHandcuffInventory;

    [Header("Ore Visual Stack")]
    [SerializeField]
    private VisualStackData oreStack = new VisualStackData
    {
        baseLocalPosition = new Vector3(0f, 0f, -0.9f),
        yStep = 0.25f
    };

    [Header("Money Visual Stack")]
    [SerializeField]
    private VisualStackData moneyStack = new VisualStackData
    {
        baseLocalPosition = new Vector3(0f, 0f, -1.5f),
        yStep = 0.25f
    };

    [Header("Handcuff Visual Stack")]
    [SerializeField]
    private VisualStackData handcuffStack = new VisualStackData
    {
        baseLocalPosition = new Vector3(0f, 0f, 1.2f),
        yStep = 0.25f
    };

    private void OnEnable()
    {
        if (playerResourceInventory != null)
        {
            playerResourceInventory.OnOreChanged += HandleOreChanged;
        }

        if (playerMoneyInventory != null)
        {
            playerMoneyInventory.OnMoneyChanged += HandleMoneyChanged;
        }

        if (playerHandcuffInventory != null)
        {
            playerHandcuffInventory.OnHandcuffChanged += HandleHandcuffChanged;
        }
    }

    private void OnDisable()
    {
        if (playerResourceInventory != null)
        {
            playerResourceInventory.OnOreChanged -= HandleOreChanged;
        }

        if (playerMoneyInventory != null)
        {
            playerMoneyInventory.OnMoneyChanged -= HandleMoneyChanged;
        }

        if (playerHandcuffInventory != null)
        {
            playerHandcuffInventory.OnHandcuffChanged -= HandleHandcuffChanged;
        }
    }

    private void Start()
    {
        RefreshAll();
    }

    private void HandleOreChanged(int currentOreCount, int maxOreCapacity)
    {
        oreStack.SyncToCount(transform, currentOreCount);
    }

    private void HandleMoneyChanged(int currentMoney)
    {
        int visualMoneyCount = currentMoney / 10;
        moneyStack.SyncToCount(transform, visualMoneyCount);
    }

    private void HandleHandcuffChanged(int currentHandcuffCount)
    {
        handcuffStack.SyncToCount(transform, currentHandcuffCount);
    }

    private void RefreshAll()
    {
        if (playerResourceInventory != null)
        {
            oreStack.SyncToCount(transform, playerResourceInventory.CurrentOreCount);
        }

        if (playerMoneyInventory != null)
        {
            moneyStack.SyncToCount(transform, playerMoneyInventory.CurrentMoney / 10);
        }

        if (playerHandcuffInventory != null)
        {
            handcuffStack.SyncToCount(transform, playerHandcuffInventory.CurrentHandcuffCount);
        }
    }
}