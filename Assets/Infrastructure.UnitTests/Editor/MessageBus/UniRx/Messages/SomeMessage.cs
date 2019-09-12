using System;
using Assets.Infrastructure.Scripts.CQRS;

namespace Assets.Infrastructure.UnitTests.Editor.MessageBus.UniRx.Messages
{
    internal class SomeMessage : IEvent
    {
        public string Message { get; private set; }

        public SomeMessage(string message, string entityId, DateTime utcOccureTime)
        {
            Message = message;
            EntityId = entityId;
            UtcOccureTime = utcOccureTime;
        }

        public string EntityId { get; }
        public DateTime UtcOccureTime { get; }
    }
}