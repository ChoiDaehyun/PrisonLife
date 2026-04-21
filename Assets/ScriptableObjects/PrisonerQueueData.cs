using UnityEngine;

[CreateAssetMenu(fileName = "PrisonerQueueData", menuName = "Game/Prisoner Queue Data")]
public class PrisonerQueueData : ScriptableObject
{
    [Header("Waypoints")]
    public Vector3 outsideWaitPosition = new Vector3(-11.3199997f, 0.591000021f, -4.73999977f);   // 1
    public Vector3 insideWaitPosition = new Vector3(-9.77999973f, 0.591000021f, -6.11999989f);    // 2
    public Vector3 serviceWaitPosition = new Vector3(-8.48999977f, 0.591000021f, -7.28999996f);   // 3
    public Vector3 turnPointPosition = new Vector3(-3.1099999f, 0.591000021f, -12.29f);            // 4
    public Vector3 jailBasePosition = new Vector3(0.170000002f, 0.591000021f, -8.76000023f);       // 5

    [Header("Movement")]
    public float moveSpeed = 2f;

    [Header("Handcuff Give")]
    public float handcuffGiveInterval = 0.25f;
    public int minRequiredHandcuffs = 1;
    public int maxRequiredHandcuffs = 4;
    public int moneyPerHandcuff = 10;

    [Header("Jail")]
    public int maxJailedCount = 20;
    public int jailRowSize = 5;
    public float jailSpacingX = 0.8f;
    public float jailSpacingZ = 0.8f;
}