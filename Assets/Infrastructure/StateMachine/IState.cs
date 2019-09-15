namespace Infrastructure.StateMachine
{
    public interface IState
    {
        IState NextState { get; }

        void Enter(IStateMachineContext context);

        void Update(float deltaTime, StateMachine stateMachine, IStateMachineContext context);

        void FixedUpdate(float deltaTime, StateMachine stateMachine, IStateMachineContext context);

        void LateUpdate(float deltaTime, StateMachine stateMachine, IStateMachineContext context);

        void Exit(IStateMachineContext context);
    }
}