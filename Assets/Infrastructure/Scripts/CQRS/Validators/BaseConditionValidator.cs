using System;

namespace Assets.Infrastructure.Scripts.CQRS.Validators {
	public abstract class BaseConditionValidator : IMessageConditionValidator {
		public Type MessageType { get; }
		public Type MessageHandlerType { get; }

		protected BaseConditionValidator(Type messageHandlerType, Type messageType) {
			MessageType = messageType;
			MessageHandlerType = messageHandlerType;
		}

		public abstract ValidationResult Validate(IMessageHandler handler, IMessage message);
	}

	public abstract class BaseConditionValidator<TMessage> : IMessageConditionValidator<TMessage>
		where TMessage : class, IMessage {
		public virtual Type MessageHandlerType => typeof(IMessageHandler);
		public Type MessageType { get; }

		protected BaseConditionValidator() {
			MessageType = typeof(TMessage);
		}

		public abstract ValidationResult Validate(IMessageHandler handler, TMessage message);

		public ValidationResult Validate(IMessageHandler handler, IMessage message) {
			return Validate(handler, (TMessage)message);
		}
	}

	public abstract class BaseConditionValidator<TMessageHandler, TMessage> : IMessageConditionValidator<TMessageHandler, TMessage>
		where TMessageHandler : class, IMessageHandler<TMessage>
		where TMessage : class, IMessage {
		public Type MessageHandlerType { get; }
		public Type MessageType { get; }

		protected BaseConditionValidator() {
			MessageHandlerType = typeof(TMessageHandler);
			MessageType = typeof(TMessage);
		}

		public abstract ValidationResult Validate(TMessageHandler handler, TMessage message);

		public ValidationResult Validate(IMessageHandler handler, IMessage message) {
			return Validate((TMessageHandler)handler, (TMessage)message);
		}
	}
}