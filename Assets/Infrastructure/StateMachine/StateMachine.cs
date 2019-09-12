using UnityEngine;
using Assets.Infrastructure.Scripts.CQRS;

public class StateMachine : IEventHandler<OnStateMachineReset>
{
    public StateMachine(IMessageBus messageBus)
    {
        SetupMessages(messageBus);
    }

    private void SetupMessages(IMessageBus messageBus)
    {
        messageBus.AddRule(MessageRouteRule.Create<OnStateMachineReset, StateMachine>(string.Empty, false));
        messageBus.Subscribe<StateMachine, OnStateMachineReset>(this, new MessageHandlerActionExecutor<OnStateMachineReset>(Handle));
    }

    public void Start(IStateMachineContext context)
    {
        ChangeState(null, context.StartState, context);
    }

    public void Update(IStateMachineContext context)
    {
        if (!context.UpdateStateMachine)
            return;

        if (context.CurrentState != null) context.CurrentState.Update(this, context);
    }

    public void FixedUpdate(IStateMachineContext context)
    {
        if (!context.UpdateStateMachine)
            return;

        if (context.CurrentState != null) context.CurrentState.FixedUpdate(this, context);
    }

    private void ChangeState(IState newState, IStateMachineContext context)
    {
        //Debug.Log(context.GO.name + "  " + context.GetType().Name + "  " + newState.GetType().Name);

        if (context.CurrentState != null) context.CurrentState.Exit(context);
        context.CurrentState = newState;
        if (context.CurrentState != null) context.CurrentState.Enter(context);
    }

    public void ChangeState(object sender, IState newState, IStateMachineContext context)
    {
        if (!context.UpdateStateMachine && sender != null)
            return;

        if (sender == context.CurrentState)
        {
            ChangeState(newState, context);
        }
    }

    public void Reset(IStateMachineContext context)
    {
        ChangeState(context.StartState, context);
    }

    public void Handle(OnStateMachineReset @event)
    {
        Reset(@event.Context);
    }
}