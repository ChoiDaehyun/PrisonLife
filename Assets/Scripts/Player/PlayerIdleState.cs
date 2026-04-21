using UnityEngine;

//Idle State
public class PlayerIdleState : IPlayerMoveState
{
    private readonly PlayerMovementController controller; //중앙 제어

    public PlayerIdleState(PlayerMovementController controller)
    {
        this.controller = controller;
    }

    // ->Idle
    public void Enter()
    {

    }

    //Idle 상태 유지 동안
    public void Update()
    {
        if (controller.HasMoveInput())
        {
            controller.ChangeState(controller.MoveState);
        }
    }

    //벗어날 때
    public void Exit()
    {

    }
}
