using System.Collections.Generic;
using System.Linq;
using Assets.Infrastructure.Scripts.CQRS;
using Assets.Infrastructure.UnitTests.Editor.MessageBus.UniRx.Messages;

namespace Assets.Infrastructure.UnitTests.Editor.MessageBus.UniRx.Handlers
{
    internal class SomeDrivedMessageHandler : IMessageHandler<SomeDrivedMessage>
    {
        public readonly string Name;
        private Queue<SomeDrivedMessage> _handledMessages;
        public IReadOnlyList<SomeDrivedMessage> HandledMessages => _handledMessages.ToList();

        public int HandleCallCount => _handledMessages.Count;

        public SomeDrivedMessageHandler(string name)
        {
            Name = name;
            _handledMessages = new Queue<SomeDrivedMessage>();
        }

        public void Handle(SomeDrivedMessage message)
        {
            _handledMessages.Enqueue(message);
        }
    }
}