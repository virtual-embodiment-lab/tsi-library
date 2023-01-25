using UnityEngine;

public class IdleState : State
{
    private AvatarInstance _ai;

    public IdleState(AvatarInstance avatarInstance) : base("Idle", avatarInstance)
    {
        _ai = avatarInstance;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (_ai.GetRoomState().Equals(RoomState.Teaching))
        {
            AvatarInstance.ChangeState(Random.value > 0.2 ? _ai.focusedState : _ai.unFocusedState);
        }
    }
}
