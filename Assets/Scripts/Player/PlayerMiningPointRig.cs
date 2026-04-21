using System.Collections.Generic;
using UnityEngine;

public class PlayerMiningPointRig : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerEquipmentController equipmentController;

    [Header("Layout")]
    [SerializeField] private int maxPointCount = 4;
    [SerializeField] private float forwardOffset = 0.6f;
    [SerializeField] private float forwardOffsetHeavy = 1.5f;
    [SerializeField] private float heightOffset = 0.3f;

    [Header("Runtime")]
    [SerializeField] private List<Transform> miningPoints = new List<Transform>();

    public int ActivePointCount { get; private set; }

    private void Awake()
    {
        EnsurePoints();
        RebuildLayout();
    }

    private void OnEnable()
    {
        if (equipmentController != null)
        {
            equipmentController.OnMiningStatsChanged += HandleMiningStatsChanged;
        }
    }

    private void OneDisable()
    {
        if (equipmentController != null)
        {
            equipmentController.OnMiningStatsChanged -= HandleMiningStatsChanged;
        }
    }

    public Transform GetPoint(int index)
    {
        if (index < 0 || index >= ActivePointCount)
        {
            return null;
        }

        return miningPoints[index];
    }

    private void HandleMiningStatsChanged()
    {
        RebuildLayout();
    }

    private void EnsurePoints()
    {
        while (miningPoints.Count < maxPointCount)
        {
            GameObject pointObejct = new GameObject($"MiningPoint_{miningPoints.Count}");
            pointObejct.transform.SetParent(transform, false);
            miningPoints.Add(pointObejct.transform);
        }
    }

    public void RebuildLayout()
    {
        EnsurePoints();

        int pointCount = 1;
        float spacing = 0.68f;

        if (equipmentController != null)
        {
            pointCount = Mathf.Clamp(equipmentController.CurrentMiningPointCount, 1, maxPointCount);
            spacing = equipmentController.CurrentMiningPointSpacing;
        }

        ActivePointCount = pointCount;

        float centerOffset = (pointCount - 1) * 0.5f;

        for (int i = 0; i < miningPoints.Count; i++)
        {
            bool shouldBeActive = i < pointCount;
            miningPoints[i].gameObject.SetActive(shouldBeActive);

            if (!shouldBeActive)
            {
                continue;
            }

            float x = (i - centerOffset) * spacing;
            Vector3 localPosition = new Vector3(x, heightOffset, forwardOffsetHeavy);

            miningPoints[i].localPosition = localPosition;
            miningPoints[i].localRotation = Quaternion.identity;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        for (int i = 0; i < miningPoints.Count; i++)
        {
            if (miningPoints[i] == null || !miningPoints[i].gameObject.activeSelf)
            {
                continue;
            }
            Gizmos.DrawSphere(miningPoints[i].position, 0.08f);
        }
    }
}
