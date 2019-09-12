using Assets.Infrastructure.Scripts.CQRS.Validators;
using Assets.Infrastructure.UnitTests.Editor.MessageBus.UniRx.Handlers;
using Assets.Infrastructure.UnitTests.Editor.MessageBus.UniRx.Messages;

namespace Assets.Infrastructure.UnitTests.Editor.MessageBus.UniRx.Validators
{
    internal class SampleConditionValidator : BaseConditionValidator<SomeMessageHandler, SomeMessage>
    {
        private readonly string _handlerName;
        private readonly string _messageString;

        public SampleConditionValidator(string handlerName, string messageString)
        {
            _handlerName = handlerName;
            _messageString = messageString;
        }

        public override ValidationResult Validate(SomeMessageHandler handler, SomeMessage message)
        {
            return handler.Name == _handlerName && message.Message == _messageString
                ? ValidationResult.Accepted
                : ValidationResult.Rejected;
        }
    }
}