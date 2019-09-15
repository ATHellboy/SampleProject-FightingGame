using AlirezaTarahomi.FightingGame.Character.Event;
using Assets.Infrastructure.Scripts.CQRS;
using Assets.Infrastructure.Scripts.CQRS.Validators;

namespace AlirezaTarahomi.FightingGame.Character.Validator
{
    public class EventCharacterIdValidator<TMessage> : BaseConditionValidator<TMessage> where TMessage : class, IMessage, IEvent, ICharacterIdProperty
    {
        public override ValidationResult Validate(IMessageHandler handler, TMessage message)
        {
            var playerIdHandler = handler as ICharacterIdProperty;
            if (null == handler)
            {
                return ValidationResult.Rejected;
            }
            return message.CharacterId == playerIdHandler.CharacterId ? ValidationResult.Accepted : ValidationResult.Rejected;
        }
    }
}