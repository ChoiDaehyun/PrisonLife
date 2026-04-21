using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovementController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerInputReader inputReader; //PlayerInputReader가 전달하는 입력 받기
    [SerializeField] private PlayerMovementData movementData; //데이터 참조
    [SerializeField] private Transform cameraTransform; //화면 기준 이동

    private CharacterController characterController;
    private Vector2 moveInput;

    private IPlayerMoveState currentState; //현재 상태 저장
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        IdleState = new PlayerIdleState(this);
        MoveState = new PlayerMoveState(this);
    }

    //오브젝트 활성화
    private void OnEnable()
    {
        if (inputReader != null)
        {
            inputReader.OnMoveInputChanged += HandleMoveInputChanged;
        }
    }

    //오브젝트 비활성화
    private void OnDisable()
    {
        if (inputReader != null)
        {
            inputReader.OnMoveInputChanged -= HandleMoveInputChanged;
        }
    }

    private void Start()
    {
        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }

        ChangeState(IdleState); //초기 상태
    }

    private void Update()
    {
        currentState?.Update(); //현재 상태 존재 시 그 상태의 Update() 실행
    }

    private void HandleMoveInputChanged(Vector2 input)
    {
        moveInput = input;
    }

    public bool HasMoveInput()
    {
        if (movementData == null) return false;
        return moveInput.sqrMagnitude > movementData.inputDeadZone * movementData.inputDeadZone;
    }

    public void Move()
    {
        if (movementData == null || cameraTransform == null) return;

        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        //수평 보정
        camForward.y = 0f;
        camRight.y = 0f;

        camForward.Normalize();
        camRight.Normalize();

        //2D -> 3D
        Vector3 moveDirection = camForward * moveInput.y + camRight * moveInput.x;
        moveDirection = Vector3.ClampMagnitude(moveDirection, 1f);
        //차원 변환 후 이동
        characterController.Move(moveDirection * movementData.moveSpeed * Time.deltaTime);

        //회전
        if (moveDirection.sqrMagnitude > 0.0001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                movementData.rotationSpeed * Time.deltaTime
            );
        }
    }

    //상태 전환 흐름 처리
    public void ChangeState(IPlayerMoveState newState)
    {
        if (newState == null) return;
        if (currentState == newState) return;

        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }
}
