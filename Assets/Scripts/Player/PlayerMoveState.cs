using UnityEngine;

//Move State
public class PlayerMoveState : IPlayerMoveState
{
    private readonly PlayerMovementController controller; //중앙 제어

    public PlayerMoveState(PlayerMovementController controller)
    {
        this.controller = controller;
    }

    // -> Move
    public void Enter()
    {

    }

    //Move 상태 유지 동안
    public void Update()
    {
        if (!controller.HasMoveInput())
        {
            controller.ChangeState(controller.IdleState);
            return;
        }
        controller.Move();
    }

    //벗어날 때
    public void Exit()
    {

    }
}
