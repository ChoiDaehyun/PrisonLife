using UnityEngine;

//에셋 생성
[CreateAssetMenu(
    fileName = "PlayerMovementData",
    menuName = "Game/Player Movement Data"
)]
public class PlayerMovementData : ScriptableObject
{
    [Header("Move")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 12f;

    [Header("Input")]
    [Range(0f, 1f)]
    public float inputDeadZone = 0.15f; //입력 무시 기준
}
