using Assets.Infrastructure.Scripts.CQRS.Validators;
using Assets.Infrastructure.UnitTests.Editor.MessageBus.UniRx.Handlers;
using Assets.Infrastructure.UnitTests.Editor.MessageBus.UniRx.Messages;

namespace Assets.Infrastructure.UnitTests.Editor.MessageBus.UniRx.Validators
{
    internal class AlwaysConstResultConditionValidator : BaseConditionValidator<SomeMessageHandler, SomeMessage>
    {
        private readonly ValidationResult _result;

        public AlwaysConstResultConditionValidator(ValidationResult result)
        {
            _result = result;
        }

        public override ValidationResult Validate(SomeMessageHandler handler, SomeMessage message)
        {
            return _result;
        }
    }
}