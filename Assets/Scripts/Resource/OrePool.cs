using System.Collections.Generic;
using UnityEngine;

public class OrePool : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private OreNode orePrefab;

    [Header("Pool Settings")]
    [SerializeField] private int initialSize = 136; //초기 생성 개수

    private readonly Queue<OreNode> availableQueue = new Queue<OreNode>(); //사용 가능 광물 리스트
    private readonly List<OreNode> allNodes = new List<OreNode>(); //생성된 광물 리스트

    public int availableCount => availableQueue.Count;
    public int TotalCount => allNodes.Count;

    //Pool 초기 세팅
    public void Initialize()
    {
        if (orePrefab == null)
        {
            return;
        }
        //중복 초기화 방지
        if (allNodes.Count > 0)
        {
            return;
        }
        //초기 생성
        for (int i = 0; i < initialSize; i++)
        {
            OreNode node = Instantiate(orePrefab, transform);
            node.gameObject.SetActive(false);

            allNodes.Add(node);
            availableQueue.Enqueue(node);
        }
    }

    public OreNode Get()
    {
        if (availableQueue.Count == 0)
        {
            ExpandPool(1); //Pool 비어있으면 생성
        }

        OreNode node = availableQueue.Dequeue();
        return node;
    }

    //광물을 풀로 되돌림
    public void Return(OreNode node)
    {
        if (node == null)
        {
            return;
        }
        node.Despawn();
        availableQueue.Enqueue(node);
    }

    //풀 확장
    private void ExpandPool(int count)
    {
        for (int i = 0; i < count; i++)
        {
            OreNode node = Instantiate(orePrefab, transform);
            node.gameObject.SetActive(false);

            allNodes.Add(node);
            availableQueue.Enqueue(node);
        }
    }
}
