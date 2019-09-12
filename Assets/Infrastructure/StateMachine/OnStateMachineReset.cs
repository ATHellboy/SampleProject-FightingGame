using Assets.Infrastructure.Scripts.CQRS;
using System;
using UnityEngine;

public class OnStateMachineReset : IEvent
{
    public string EntityId { get; }
    public DateTime UtcOccureTime { get; }
    public IStateMachineContext Context { get; }

    public OnStateMachineReset(IStateMachineContext context)
    {
        Context = context;
    }
}