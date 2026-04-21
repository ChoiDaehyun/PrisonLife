using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipmentController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerMiningData baseMiningData;
    [SerializeField] private List<EquipmentData> registeredEquipments = new List<EquipmentData>();

    [Header("Runtime")]
    [SerializeField] private List<string> ownedEquipmentIds = new List<string>();

    private readonly HashSet<string> ownedEquipmentSet = new HashSet<string>();
    private readonly Dictionary<string, EquipmentData> equipmentLookup = new Dictionary<string, EquipmentData>();

    public event Action<EquipmentData> OnEquipmentAcquired;
    public event Action OnMiningStatsChanged;

    [SerializeField] private ParticleSystem equipParticle;
    [SerializeField] private AudioSource equipAudio;

    public float CurrentMiningInterval { get; private set; }
    public float CurrentMiningRadius { get; private set; }
    public int CurrentMiningPointCount { get; private set; }
    public float CurrentMiningPointSpacing { get; private set; }

    private void Awake()
    {
        BuildEquipmentLookup();
        RebuildOwnedSet();
        RecalculateMiningStats();
    }

    public bool HasEquipment(EquipmentData equipmentData)
    {
        if (equipmentData == null || string.IsNullOrWhiteSpace(equipmentData.equipmentId))
        {
            return false;
        }

        return ownedEquipmentSet.Contains(equipmentData.equipmentId);
    }

    public bool TryAcquireEquipment(EquipmentData equipmentData)
    {
        if (equipmentData == null)
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(equipmentData.equipmentId))
        {
            return false;
        }

        if (ownedEquipmentSet.Contains(equipmentData.equipmentId))
        {
            return false;
        }

        if (!equipmentLookup.ContainsKey(equipmentData.equipmentId))
        {
            equipmentLookup.Add(equipmentData.equipmentId, equipmentData);
        }

        ownedEquipmentSet.Add(equipmentData.equipmentId);
        ownedEquipmentIds.Add(equipmentData.equipmentId);

        equipParticle.Play();
        equipAudio.Play();
        RecalculateMiningStats();
        OnEquipmentAcquired?.Invoke(equipmentData);
        OnMiningStatsChanged?.Invoke();

        return true;
    }

    private void BuildEquipmentLookup()
    {
        equipmentLookup.Clear();

        for (int i = 0; i < registeredEquipments.Count; i++)
        {
            EquipmentData equipmentData = registeredEquipments[i];

            if (equipmentData == null || string.IsNullOrWhiteSpace(equipmentData.equipmentId))
            {
                continue;
            }

            if (!equipmentLookup.ContainsKey(equipmentData.equipmentId))
            {
                equipmentLookup.Add(equipmentData.equipmentId, equipmentData);
            }
        }
    }

    private void RebuildOwnedSet()
    {
        ownedEquipmentSet.Clear();

        for (int i = 0; i < ownedEquipmentIds.Count; i++)
        {
            string equipmentId = ownedEquipmentIds[i];

            if (string.IsNullOrWhiteSpace(equipmentId))
            {
                continue;
            }

            ownedEquipmentSet.Add(equipmentId);
        }
    }

    private void RecalculateMiningStats()
    {
        if (baseMiningData == null)
        {
            CurrentMiningInterval = 1.0f;
            CurrentMiningRadius = 0.45f;
            CurrentMiningPointCount = 1;
            CurrentMiningPointSpacing = 0.68f;
            return;
        }

        CurrentMiningInterval = baseMiningData.miningInterval;
        CurrentMiningRadius = baseMiningData.miningRadius;
        CurrentMiningPointCount = Mathf.Max(1, baseMiningData.defaultMiningPointCount);
        CurrentMiningPointSpacing = baseMiningData.defaultMiningPointSpacing;

        for (int i = 0; i < ownedEquipmentIds.Count; i++)
        {
            string equipmentId = ownedEquipmentIds[i];

            if (!equipmentLookup.TryGetValue(equipmentId, out EquipmentData equipmentData) || equipmentData == null)
            {
                continue;
            }
            ApplyEquipment(equipmentData);
        }
    }

    private void ApplyEquipment(EquipmentData equipmentData)
    {
        if (equipmentData.overrideMiningInterval)
        {
            CurrentMiningInterval = equipmentData.miningInterval;
        }

        if (equipmentData.overrideMiningRadius)
        {
            CurrentMiningRadius = equipmentData.miningRadius;
        }

        if (equipmentData.overrideMiningPointCount)
        {
            CurrentMiningPointCount = Mathf.Max(1, equipmentData.miningPointCount);
        }
        if (equipmentData.overrideMiningPointSpacing)
        {
            CurrentMiningPointSpacing = equipmentData.miningPointSpacing;
        }
    }
}
