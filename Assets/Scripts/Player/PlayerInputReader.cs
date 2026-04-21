using System;
using UnityEngine;

//입력 전달만
public class PlayerInputReader : MonoBehaviour
{
    [SerializeField] private JoystickUI joystickUI;
    public event Action<Vector2> OnMoveInputChanged; //입력 바뀌면 알림
    public Vector2 MoveInput { get; private set; } //현재 입력값 저장

    private void Update()
    {
        //joystickUI 연결O , joystickUI 연결X
        Vector2 newInput = joystickUI != null ? joystickUI.InputVector : Vector2.zero;

        //입력값 변동 확인
        if (newInput != MoveInput)
        {
            MoveInput = newInput;
            OnMoveInputChanged?.Invoke(MoveInput);
        }
    }

    private void OnValidate()
    {
        if (joystickUI == null)
        {

        }
    }
}
