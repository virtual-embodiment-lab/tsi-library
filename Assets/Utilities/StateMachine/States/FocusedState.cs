using UnityEngine;

public class FocusedState : State
{
    private AvatarInstance _ai;

    public FocusedState(AvatarInstance avatarInstance) : base("Focused", avatarInstance)
    {
        _ai = avatarInstance;
    }

    public override void Enter()
    {
        base.Enter();
        _ai.AvatarModel.GetComponent<HeadController>().SetActive(true);
        _ai.AvatarModel.GetComponent<Animator>().SetBool("Focused", true);
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

        if (gaze < 0.8 && proximity < 0.8)
        {
            if (gaze > 0.3)
            {
                if (proximity > 0.4)
                {
                    if (rand < 0.000001f)
                    {
                        AvatarInstance.ChangeState(_ai.unFocusedState);
                    }
                }
                else
                {
                    if (rand < 0.0000001f)
                    {
                        AvatarInstance.ChangeState(_ai.unFocusedState);
                    }
                }
            }
            else
            {
                if (proximity > 0.4)
                {
                    if (rand < 0.00005f)
                    {
                        AvatarInstance.ChangeState(_ai.unFocusedState);
                    }
                }
                else
                {
                    if (rand < 0.00001f)
                    {
                        AvatarInstance.ChangeState(_ai.unFocusedState);
                    }
                }
            }
        }
    }
}
