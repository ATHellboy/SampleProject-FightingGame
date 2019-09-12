using System;
using Assets.Infrastructure.Scripts.CQRS;

namespace Assets.Infrastructure.UnitTests.Editor.MessageBus.UniRx.Messages
{
    internal class SomeOtherMessage : IEvent
    {
        private string _orginalMessage;

        public string Message
        {
            get
            {
                return _orginalMessage.ToUpper();
            }
        }

        public SomeOtherMessage(string message, string entityId, DateTime utcOccureTime)
        {
            _orginalMessage = message;
            EntityId = entityId;
            UtcOccureTime = utcOccureTime;
        }

        public string EntityId { get; }
        public DateTime UtcOccureTime { get; }
    }
}