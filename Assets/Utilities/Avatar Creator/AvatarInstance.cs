using System.Collections;
using System.Collections.Generic;
using System.Linq;
using StandardLogging;
using UnityEngine;
using UnityEngine.UI;

public class AvatarInstance : StateMachine
{
    #region Fields and Properties
    [SerializeField] public GameObject AvatarModel = null;

    [SerializeField] protected List<GameObject> Avatar = new List<GameObject>();
    [SerializeField] public Slider GazeSlider;
    [SerializeField] protected int MaxGazeBar = 100;
    [SerializeField] protected Queue<float> CurrentGazeBar = new Queue<float>();
    [SerializeField] protected float averageGaze = 0f;

    [SerializeField] private bool isGazing = false;
    [SerializeField] private bool directGazing = false;

    [SerializeField] public Slider ProximitySlider;
    [SerializeField] protected int MaxProximityBar = 100;
    [SerializeField] protected float CurrentProximity = 0f;
    [SerializeField] protected Queue<float> CurrentProximityBar = new Queue<float>();
    [SerializeField] protected float averageProximity = 0f;

    [SerializeField] protected GameObject Player = null;
    [SerializeField] protected GameObject Canvas = null;

    [SerializeField] public RoomManager roomManager = null;
    [SerializeField] public string currentState = null;

    [HideInInspector] public IdleState idleState = null;
    [HideInInspector] public FocusedState focusedState = null;
    [HideInInspector] public UnFocusedState unFocusedState = null;
    #endregion

    public RoomState GetRoomState()
    {
        if (roomManager)
        {
            return roomManager.GetRoomState();
        }
        else
        {
            return RoomState.Idle;
        }
    }

    public float GetAverageProximity()
    {
        return averageProximity;
    }

    public float GetAverageGaze()
    {
        return averageGaze;
    }

    public void FixedUpdate()
    {
        UIRotation();
        currentState = State != null ? State.Name : "(no current state)";
    }

    private void Awake()
    {
        idleState = new IdleState(this);
        focusedState = new FocusedState(this);
        unFocusedState = new UnFocusedState(this);
    }

    public override void ApplyValue(string type, string value)
    {
        base.ApplyValue(type, value);
        if (type == logtype.State.ToString())
        {
            switch (value)
            {
                case "IdleState":
                    if (idleState != null)
                        ChangeState(idleState);
                    break;
                case "FocusedState":
                    if (focusedState != null)
                        ChangeState(focusedState);
                    break;
                case "UnFocusedState":
                    if (unFocusedState != null)
                        ChangeState(unFocusedState);
                    break;
                default:
                    if (idleState != null)
                        ChangeState(idleState);
                    break;
            }
        }
    }

    protected override State GetInitialState()
    {
        return idleState;
    }

    private void UIRotation()
    {
        if (Player)
        {
            Canvas.transform.LookAt(Player.transform, -Vector3.up);
            Canvas.transform.localEulerAngles = new Vector3(0, Canvas.transform.localEulerAngles.y, 0);
        }
    }

    public void Setup()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        AvatarModel = Instantiate(Avatar[Random.Range(0, Avatar.Count)], transform.position, transform.rotation);
        AvatarModel.transform.SetParent(transform);
        if (Player)
        {
            AvatarModel.GetComponent<HeadController>().SetLookObject(Player.transform.GetComponentInChildren<Camera>().gameObject.transform);
            AvatarModel.GetComponent<HeadController>().SetActive(true);
        }
        GazeSlider.value = 0;
        ProximitySlider.value = 0;
        for (int i = 0; i < 200; i++)
        {
            CurrentGazeBar.Enqueue(0.0f);
            CurrentProximityBar.Enqueue(0.0f);
        }
        if (Player)
        {
            StartCoroutine(getProximity());
        }
        roomManager = FindObjectOfType<RoomManager>();
        //SetState(new IdleState(this));
        //SwitchToTheNextState(gameObject.AddComponent<IdleState>());
    }

    IEnumerator incrementGaze()
    {
        while (true)
        {
            CurrentGazeBar.Dequeue();
            CurrentGazeBar.Enqueue(isGazing ? directGazing ? 1.0f : CurrentProximity < 3 ? 0.7f : 0.3f : 0.0f);
            averageGaze = CurrentGazeBar.Sum() / CurrentGazeBar.Count;
            SetGazeSlider(averageGaze);
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator getProximity()
    {
        while (true)
        {
            CurrentProximityBar.Dequeue();
            CurrentProximity = Vector3.Distance(transform.position, Player.transform.position);
            switch (CurrentProximity)
            {
                case float d when d >= 5 && d < 6:
                    CurrentProximityBar.Enqueue(0.1f);
                    break;

                case float d when d >= 4 && d < 5:
                    CurrentProximityBar.Enqueue(0.3f);
                    break;

                case float d when d >= 3 && d < 4:
                    CurrentProximityBar.Enqueue(0.5f);
                    break;

                case float d when d >= 2 && d < 3:
                    CurrentProximityBar.Enqueue(0.7f);
                    break;

                case float d when d >= 1 && d < 2:
                    CurrentProximityBar.Enqueue(1f);
                    break;

                default:
                    CurrentProximityBar.Enqueue(0f);
                    break;
            }
            averageProximity = CurrentProximityBar.Sum() / CurrentProximityBar.Count;
            SetProximitySlider(averageProximity);
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void SetDirectLook(bool value)
    {
        directGazing = value;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag.Equals("Vision") == true)
        {
            isGazing = true;
            StartCoroutine(incrementGaze());
        }
    }

    public void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag.Equals("Vision") == true)
        {
            isGazing = false;
        }
    }

    public void SetGazeSlider(float value)
    {
        GazeSlider.value = value;
    }

    public void SetProximitySlider(float value)
    {
        ProximitySlider.value = value;
    }
}
