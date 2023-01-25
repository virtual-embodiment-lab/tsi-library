using StandardLogging;
using UnityEngine;

public abstract class StateMachine : Tracker
{
    protected State State;
    [SerializeField] protected bool replayMode = false;

    public override void StartReplayMode()
    {
        base.StartReplayMode();
        replayMode = true;
    }

    void Start()
    {
        if (replayMode)
        {
            return;
        }
        State = GetInitialState();
        if (State != null)
            State.Enter();
    }

    protected override void Update()
    {
        if (replayMode)
        {
            return;
        }
        base.Update();
        Map.UpdateOrCreate(new KVPair<logtype, string>(logtype.State, State.ToString()));
        if (State != null)
            State.Update();
    }

    public void ChangeState(State newState)
    {
        //State.Exit();
        State = newState;
        State.Enter();
    }

    protected virtual State GetInitialState()
    {
        return null;
    }

    //private void OnGUI()
    //{
    //    string content = State != null ? State.Name : "(no current state)";
    //    GUILayout.Label($"<color='black'><size=40>{content}</size></color>");
    //}

    //public void SetState(State state)
    //{
    //    State = state;
    //    //StartCoroutine(State.Run());
    //}

    //// map from states to indexe  s in the list of states [states]
    //private Dictionary<State, int> stateIndexMap;
    //private List<int> weights;
    //private List<State> states;
    //private State currentState;
    //// unique identifier for this state machine
    //private int id;

    ///**
    //Creates a state machine with the given id and start states with the given weight.
    //**/
    //public StateMachine(int id, State startState, int startStateWeight) {
    //    this.stateMap = new Dictionary<State, int>();
    //    this.weights = new List<int>();
    //    this.weights.add(startStateWeight);
    //    this.stateMap.Add(startState, 0);
    //    this.currentState = startState;
    //    this.id = id;
    //}

    ///**
    //Returns a random index within [0, weights.Length) with weighted probabilities for each index given in the array.
    //The probability index i is selected is weights[i] / weights.Sum().
    //Requires: weights[i] >= 1
    //**/
    //private int getRandomIndex() {
    //    int indexVal = Random.Range(1, weights[weights.Length-1]+1);
    //    // can replace with binary search
    //    for(int i=0; i<weights.Length; i++) {
    //        if(indexVal <= weights[i]) return i;
    //    }
    //    return -1;
    //}

    //public int getID() {
    //    return this.id;
    //}

    //public State getCurrentState() {
    //    return this.currentState;
    //}

    //public List<State> getAllStates() {
    //    return this.states;
    //}

    //public List<int> getWeights() {
    //    return this.weights;
    //}

    //public void addState(State state, int weight) {
    //    this.stateIndexMap.add(state, this.states.Count);
    //    this.states.add(state);
    //    this.weights.add(weight + weights[weights.Count-1]);
    //}


    ///**
    //Transitions to a random state with the given weights among all of the possible states.
    //**/
    //public void randomTransition() {
    //    int nextStateIndex = getRandomIndex();
    //    this.currentState.ExitState();
    //    this.currentState = states[nextStateIndex];
    //    this.currentState.EnterState();
    //}

    //public void setCurrentState(State state) {
    //    if(this.currentState == state) return;
    //    this.currentState.ExitState();
    //    this.currentState = state;
    //    this.currentState.EnterState();
    //}
}