public abstract class State
{
    public string Name;
    protected AvatarInstance AvatarInstance;

    public State(string name, AvatarInstance avatarInstance)
    {
        Name = name;
        AvatarInstance = avatarInstance;
    }

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }
}