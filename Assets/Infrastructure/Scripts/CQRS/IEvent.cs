using System;

namespace Assets.Infrastructure.Scripts.CQRS
{
    public interface IEvent : IMessage, IEntityIdProperty
    {
        DateTime UtcOccureTime { get; }
    }
}