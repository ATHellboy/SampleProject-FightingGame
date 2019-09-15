using UnityEngine;

namespace Infrastructure.StateMachine
{
    public class BaseStateMachineContext : IStateMachineContext
    {
        public GameObject GO { get; private set; }
        public IState StartState { get; private set; }
        public IState CurrentState { get; set; }
        public bool IsActive { get; private set; } = false;
        public bool CanControl { get; private set; } = false;
        public bool DebugStateMachine { get; private set; }

        public BaseStateMachineContext(GameObject go, IState startState, bool debug)
        {
            GO = go;
            StartState = startState;
            DebugStateMachine = debug;
        }

        public void ToggleContext(bool active)
        {
            IsActive = active;
        }

        public void ToggleControl(bool active)
        {
            CanControl = active;
        }
    }
}