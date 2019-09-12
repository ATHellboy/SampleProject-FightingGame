using UnityEngine;

public interface IStateMachineContext
{
    GameObject GO { get; }
    IState StartState { get; }
    IState CurrentState { get; set; }
    bool UpdateStateMachine { get; }
}