public interface IState
{
    IState NextState { get; }

    void Enter(IStateMachineContext context);

    void Update(StateMachine stateMachine, IStateMachineContext context);

    void FixedUpdate(StateMachine stateMachine, IStateMachineContext context);

    void Exit(IStateMachineContext context);
}