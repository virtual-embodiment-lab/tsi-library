using StandardLogging;
using UnityEngine;
using UtilityTypes;

public class RoomManager : Tracker
{
    [Header("Room")]
    [SerializeField] public RoomState roomState;
    [HideInInspector] public AvatarCreator avatarCreator = null;
    [HideInInspector] public Logger logger = null;

    [Header("Avatar Creator")]
    [SerializeField] protected AvatarCreatorData avatarCreatorData = null;

    [Header("Logger")]
    [SerializeField] protected LoggerData loggerData = null;

    public RoomState GetRoomState()
    {
        return roomState;
    }

    protected void Awake()
    {
        avatarCreator = gameObject.AddComponent(typeof(AvatarCreator)) as AvatarCreator;
        avatarCreator.Setup(avatarCreatorData);
        logger = gameObject.AddComponent(typeof(Logger)) as Logger;
        logger.Setup(loggerData);
    }

    public override void ApplyValue(string type, string value)
    {
        base.ApplyValue(type, value);
        if (type == logtype.State.ToString())
        {
            roomState = (RoomState)System.Enum.Parse(typeof(RoomState), value);
        }
    }

    protected override void Update()
    {
        base.Update();
        Map.UpdateOrCreate(new KVPair<logtype, string>(logtype.State, roomState.ToString()));

    }
}
