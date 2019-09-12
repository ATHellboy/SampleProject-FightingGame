using AlirezaTarahomi.FightingGame.Player.Event;
using Assets.Infrastructure.Scripts.CQRS;
using Assets.Infrastructure.Scripts.CQRS.Validators;
using UnityEngine;

namespace AlirezaTarahomi.FightingGame.Player.Validator
{
    public class EventPlayerIdValidator<TMessage> : BaseConditionValidator<TMessage> where TMessage : class, IMessage, IEvent, IPlayerIdProperty
    {
        public override ValidationResult Validate(IMessageHandler handler, TMessage message)
        {
            var playerIdHandler = handler as IPlayerIdProperty;
            if (null == handler)
            {
                return ValidationResult.Rejected;
            }
            var result = message.PlayerId == playerIdHandler.PlayerId ? ValidationResult.Accepted : ValidationResult.Rejected;
            return result;
        }
    }
}