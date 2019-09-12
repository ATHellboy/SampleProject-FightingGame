using System;

namespace Assets.Infrastructure.UnitTests.Editor.MessageBus.UniRx.Messages
{
    internal class SomeDrivedMessage : SomeMessage
    {
        public SomeDrivedMessage(string message, string entityId, DateTime utcOccureTime)
            : base("Drived Type, " + message, entityId, utcOccureTime)
        {
        }

        public string EntityId { get; }
        public DateTime UtcOccureTime { get; }
    }
}