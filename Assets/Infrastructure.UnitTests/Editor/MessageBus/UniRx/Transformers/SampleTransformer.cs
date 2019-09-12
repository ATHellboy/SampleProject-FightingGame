using Assets.Infrastructure.Scripts.CQRS.Transformers;
using Assets.Infrastructure.UnitTests.Editor.MessageBus.UniRx.Messages;

namespace Assets.Infrastructure.UnitTests.Editor.MessageBus.UniRx.Transformers
{
    internal class SampleTransformer :
            BaseTransformer<SomeOtherMessage, SomeMessage>
        //IMessageTransformer<SomeMessage, SomeOtherMessage>
    {
        public override SomeMessage Transform(SomeOtherMessage message)
        {
            return new SomeMessage(message.Message, message.EntityId, message.UtcOccureTime);
        }
    }
}