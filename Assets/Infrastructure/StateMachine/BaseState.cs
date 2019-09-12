using System;

public abstract class BaseState<TContext> : IState where TContext : IStateMachineContext
{
    public IState NextState { get; protected set; }

    private readonly Type _contextType;

    protected BaseState()
    {
        _contextType = typeof(TContext);
    }

    public void Enter(IStateMachineContext context)
    {
        if (!_contextType.IsAssignableFrom(context.GetType()))
        {
            // Log Or throw exception
            return;
        }

        Enter((TContext)context);
    }

    public abstract void Enter(TContext context);

    public void Update(StateMachine stateMachine, IStateMachineContext context)
    {
        if (!_contextType.IsAssignableFrom(context.GetType()))
        {
            // Log Or throw exception
            return;
        }

        Update(stateMachine, (TContext)context);
    }

    public abstract void Update(StateMachine stateMachine, TContext context);

    public void FixedUpdate(StateMachine stateMachine, IStateMachineContext context)
    {
        if (!_contextType.IsAssignableFrom(context.GetType()))
        {
            // Log Or throw exception
            return;
        }

        FixedUpdate(stateMachine, (TContext)context);
    }

    public abstract void FixedUpdate(StateMachine stateMachine, TContext context);

    public void Exit(IStateMachineContext context)
    {
        if (!_contextType.IsAssignableFrom(context.GetType()))
        {
            // Log Or throw exception
            return;
        }

        Exit((TContext)context);
    }

    public abstract void Exit(TContext context);
}
