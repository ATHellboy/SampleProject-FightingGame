using System;

namespace Assets.Infrastructure.Scripts.CQRS.Validators {
	public interface IMessageConditionValidator {
		Type MessageType { get; }
		Type MessageHandlerType { get; }
		ValidationResult Validate(IMessageHandler handler, IMessage message);
	}

	public interface IMessageConditionValidator<in TMessage> : IMessageConditionValidator
		where TMessage : class, IMessage {
		ValidationResult Validate(IMessageHandler handle, TMessage message);
	}

	public interface IMessageConditionValidator<in TMessageHandler, in TMessage> : IMessageConditionValidator
		where TMessageHandler : class, IMessageHandler<TMessage>
		where TMessage : class, IMessage {
		ValidationResult Validate(TMessageHandler handle, TMessage message);
	}
}