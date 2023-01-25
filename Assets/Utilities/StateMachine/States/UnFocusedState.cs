using UnityEngine;

public class UnFocusedState : State
{
    private AvatarInstance _ai;

    public UnFocusedState(AvatarInstance avatarInstance) : base("UnFocused", avatarInstance)
    {
        _ai = avatarInstance;
    }

    public override void Enter()
    {
        base.Enter();
        _ai.AvatarModel.GetComponent<HeadController>().SetActive(false);
        _ai.AvatarModel.GetComponent<Animator>().SetBool("Focused", false);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        float gaze = _ai.GetAverageGaze();
        float proximity = _ai.GetAverageProximity();
        float rand = Random.value;

        if (gaze > 0.2 && proximity > 0.2)
        {
            if (gaze > 0.3)
            {
                if (proximity > 0.5)
                {
                    if (rand < 0.1f)
                    {
                        AvatarInstance.ChangeState(_ai.focusedState);
                    }
                }
                else
                {
                    if (rand < 0.01f)
                    {
                        AvatarInstance.ChangeState(_ai.focusedState);
                    }
                }
            }
            else
            {
                if (proximity > 0.5)
                {
                    if (rand < 0.001f)
                    {
                        AvatarInstance.ChangeState(_ai.focusedState);
                    }
                }
                else
                {
                    if (rand < 0.0001f)
                    {
                        AvatarInstance.ChangeState(_ai.focusedState);
                    }
                }
            }
        }
    }
}
