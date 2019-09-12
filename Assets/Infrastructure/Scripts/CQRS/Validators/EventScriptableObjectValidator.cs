using System;
using UnityEngine;

namespace Assets.Infrastructure.Scripts.CQRS.Validators {
	public class EventScriptableObjectValidator<TMessage> :
		BaseConditionValidator<TMessage> where TMessage : class, IMessage, IEvent, IScriptableObjectProperty {
		public override ValidationResult Validate(IMessageHandler handler, TMessage message) {
			var scriptableObjectHandler = handler as IScriptableObjectProperty;
			if (null == handler) {
				return ValidationResult.Rejected;
			}
			var result = scriptableObjectHandler.GetScriptableObject() == message.GetScriptableObject() ? ValidationResult.Accepted : ValidationResult.Rejected;
            //if (message.GetType() == typeof(OnAttackToggled))
            //    Debug.Log(handler + "  " + message + "  " + scriptableObjectHandler.GetScriptableObject() + "  " + message.GetScriptableObject() +"  " + result);
            return result;
        }
	}
}