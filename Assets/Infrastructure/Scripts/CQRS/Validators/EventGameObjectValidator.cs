using System;
using UnityEngine;

namespace Assets.Infrastructure.Scripts.CQRS.Validators
{
    public class EventGameObjectValidator<TMessage> : BaseConditionValidator<TMessage> where TMessage : class, IMessage, IEvent, IGameObjectProperty
    {
        public override ValidationResult Validate(IMessageHandler handler, TMessage message)
        {
            var gameObjectHandler = handler as IGameObjectProperty;
            if (null == handler)
            {
                return ValidationResult.Rejected;
            }
            var result = message.GameObject == gameObjectHandler.GameObject ? ValidationResult.Accepted : ValidationResult.Rejected;
            return result;
        }
    }
}