using System;
using UnityEngine;

namespace Assets.Infrastructure.Scripts.CQRS.Validators {
	public class EventTypeValidator<TMessage> : BaseConditionValidator<TMessage> where TMessage : class, IMessage, IEvent, ITypeEvent {
		private readonly Type _predictedType;

		public EventTypeValidator(Type predictedType) {
			_predictedType = predictedType;
		}

		public override ValidationResult Validate(IMessageHandler handler, TMessage message) {
			return message.Type == _predictedType ? ValidationResult.Accepted : ValidationResult.Rejected;
		}
	}
}