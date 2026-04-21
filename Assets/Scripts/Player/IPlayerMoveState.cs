public interface IPlayerMoveState
{
    void Enter(); //상태 변화 진입시
    void Update(); //현재 상태 유지시
    void Exit(); //상태 벗어날 때
}