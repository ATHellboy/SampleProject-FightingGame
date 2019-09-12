using System;

namespace Assets.Infrastructure.Scripts.CQRS {
	public class MessageHandlerActionExecutor<TMessage> : IMessageHandlerActionExecutor
		where TMessage : IMessage {
		private Action<TMessage> _action;
		private Type _messageType;

		public MessageHandlerActionExecutor(Action<TMessage> action) {
			if (action == null) throw new ArgumentNullException(nameof(action));
			_action = action;
		}

		public bool CanExecuteForParameters(params object[] parameters) {
			if (parameters == null) throw new ArgumentNullException(nameof(parameters));

			if (parameters.Length != 1) return false;
			return parameters[0] is TMessage;
		}

		public void Execute(params object[] parameters) {
			if (parameters == null) throw new ArgumentNullException(nameof(parameters));
			if (parameters.Length != 1)
				throw new ArgumentOutOfRangeException(nameof(parameters));

			_action((TMessage)parameters[0]);
		}

		public Type ParameterType(int index) {
			if (index != 0)
				throw new ArgumentOutOfRangeException(nameof(index));

			return _messageType;
		}
	}
}