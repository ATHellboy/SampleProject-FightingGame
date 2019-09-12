using System;

namespace Assets.Infrastructure.Scripts.CQRS.Validators {
	public class LambdaValidator<TMessage> : BaseConditionValidator<TMessage> where TMessage : class, IMessage, ICommand {
		private readonly Func<IMessageHandler, TMessage, ValidationResult> _func;

		public LambdaValidator(Func<IMessageHandler, TMessage, ValidationResult> func) {
			_func = func;
		}
		public override ValidationResult Validate(IMessageHandler handler, TMessage message) {
			return _func(handler, message);
		}
	}
}
