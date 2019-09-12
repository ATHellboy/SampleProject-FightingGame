using System.Collections.Generic;
using System.Linq;
using Assets.Infrastructure.Scripts.CQRS;
using Assets.Infrastructure.UnitTests.Editor.MessageBus.UniRx.Messages;

namespace Assets.Infrastructure.UnitTests.Editor.MessageBus.UniRx.Handlers
{
    internal class SomeMessageHandler : IMessageHandler<SomeMessage>
    {
        public readonly string Name;
        private Queue<SomeMessage> _handledMessages;
        public IReadOnlyList<SomeMessage> HandledMessages => _handledMessages.ToList();

        public int HandleCallCount => _handledMessages.Count;

        public SomeMessageHandler(string name)
        {
            Name = name;
            _handledMessages = new Queue<SomeMessage>();
        }
        
        public void Handle(SomeMessage message)
        {
            _handledMessages.Enqueue(message);
        }
    }
}