using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStateMachineContext : IStateMachineContext
{
    public GameObject GO { get; private set; }
    public IState StartState { get; private set; }
    public IState CurrentState { get; set; }
    public bool UpdateStateMachine { get; private set; }

    public BaseStateMachineContext(GameObject go, IState startState)
    {
        GO = go;
        StartState = startState;
        UpdateStateMachine = false;
    }

    public void ToggleStateMachineUpdate(bool update)
    {
        UpdateStateMachine = update;
    }
}