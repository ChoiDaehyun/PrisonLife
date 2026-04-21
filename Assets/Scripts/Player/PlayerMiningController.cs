using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMiningController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerMiningData miningData;
    [SerializeField] private OreSpawnController oreSpawnController;
    [SerializeField] private PlayerResourceInventory resourceInventory; //인벤토리 시스템
    [SerializeField] private PlayerEquipmentController equipmentController;
    [SerializeField] private PlayerMiningPointRig miningPointRig; //채굴 범위 중심
    [SerializeField] private PlayerInventoryFullTextUI inventoryFullTextUI;

    private Collider[] overlapResults; //채굴 범위 안에서 감지된 Collider 담는 버퍼 배열
    private float miningTimer;

    public event Action<OreNode> OnOreMined;


    [SerializeField] private ParticleSystem mineParticle;
    [SerializeField] private AudioSource mineAudio;

    private void Awake()
    {
        InitializeBuffer();
    }

    private void Update()
    {
        if (miningData == null ||
            oreSpawnController == null ||
            resourceInventory == null ||
            equipmentController == null ||
            miningPointRig == null)
        {
            return;
        }

        float currentMiningInterval = equipmentController.CurrentMiningInterval;

        miningTimer += Time.deltaTime;

        if (currentMiningInterval > 0f && miningTimer < currentMiningInterval)
        {
            return;
        }

        miningTimer = 0f;
        TryMineFromAllPoints();
    }
    private void InitializeBuffer()
    {
        if (miningData == null)
        {
            overlapResults = new Collider[16]; //보험(실패 방지)
            return;
        }

        int bufferSize = Mathf.Max(1, miningData.overlapBufferSize);
        overlapResults = new Collider[bufferSize];
    }

    private void TryMineFromAllPoints()
    {
        int pointCount = miningPointRig.ActivePointCount;
        float CurrentMiningRadius = equipmentController.CurrentMiningRadius;
        HashSet<OreNode> minedThisTick = new HashSet<OreNode>();

        for (int i = 0; i < pointCount; i++)
        {
            Transform point = miningPointRig.GetPoint(i);

            if (point == null)
            {
                continue;
            }

            OreNode closestOre = FindClosestOre(point.position, CurrentMiningRadius, minedThisTick);

            if (closestOre == null)
            {
                continue;
            }

            minedThisTick.Add(closestOre);
            MineOre(closestOre);
        }
    }

    private OreNode FindClosestOre(Vector3 pointPosition, float miningRadius, HashSet<OreNode> excludedOres)
    {

        int hitCount = Physics.OverlapSphereNonAlloc(
            pointPosition, //플레이어 앞 쪽
            miningRadius,
            overlapResults,
            miningData.oreLayerMask //탐색 대상 Layer
        );

        if (hitCount <= 0)
        {
            ClearOverlapBuffer(hitCount);
            return null;
        }

        //가장 가까운 광물 찾기 준비
        OreNode closestOre = null;
        float closestDistanceSqr = float.MaxValue;

        //감지된 광물 순회
        for (int i = 0; i < hitCount; i++)
        {
            //비어 있는 슬롯 건너뜀
            Collider hitCollider = overlapResults[i];

            if (hitCollider == null)
            {
                continue;
            }

            OreNode oreNode = hitCollider.GetComponent<OreNode>();

            if (oreNode == null || !oreNode.IsSpawned)
            {
                continue;
            }

            if (excludedOres != null && excludedOres.Contains(oreNode))
            {
                continue;
            }

            float distanceSqr = (oreNode.transform.position - pointPosition).sqrMagnitude;

            //더 가까우면 최신화
            if (distanceSqr < closestDistanceSqr)
            {
                closestDistanceSqr = distanceSqr;
                closestOre = oreNode;
            }
        }

        ClearOverlapBuffer(hitCount);
        return closestOre;
    }

    private void MineOre(OreNode oreNode)
    {
        if (oreNode == null)
        {
            return;
        }

        //채굴은 항상
        oreSpawnController.MineOre(oreNode, miningData.oreRespawnDelay);

        mineParticle.Play();
        mineAudio.Play();
        //저장은 공간이 있을 때만
        bool addedToInventory = resourceInventory.TryAddOre(1);

        if (addedToInventory)
        {

        }
        else
        {
            if (inventoryFullTextUI != null)
            {
                inventoryFullTextUI.Show();
            }

        }

        OnOreMined?.Invoke(oreNode); //MAX!!"UI 구현 때 사용
                                     //광물이 단순히 사라진건지
                                     //인벤토리에 들어간건지(Max)
    }

    private void ClearOverlapBuffer(int hitCount)
    {
        for (int i = 0; i < hitCount; i++)
        {
            overlapResults[i] = null;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (equipmentController == null || miningPointRig == null)
        {
            return;
        }

        Gizmos.color = Color.cyan;

        for (int i = 0; i < miningPointRig.ActivePointCount; i++)
        {
            Transform point = miningPointRig.GetPoint(i);
            if (point == null)
            {
                continue;
            }

            Gizmos.DrawWireSphere(point.position, equipmentController.CurrentMiningRadius);
        }
    }
}
