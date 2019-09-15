using UnityEngine;

namespace Infrastructure.StateMachine
{
    public interface IStateMachineContext
    {
        GameObject GO { get; }
        IState StartState { get; }
        IState CurrentState { get; set; }
        bool IsActive { get; }
        bool DebugStateMachine { get; }
    }
}