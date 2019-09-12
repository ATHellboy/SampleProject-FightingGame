using System;

namespace Assets.Infrastructure.Scripts.CQRS.Validators {
	public class BoolLambdaValidator<TMessage> : BaseConditionValidator<TMessage> where TMessage : class, IMessage {
		private readonly Func<IMessageHandler, TMessage, bool> _func;

		public BoolLambdaValidator(Func<IMessageHandler, TMessage, bool> func) {
			_func = func;
		}
		public override ValidationResult Validate(IMessageHandler handler, TMessage message) {
			return _func(handler, message) ? ValidationResult.Accepted : ValidationResult.Rejected;
		}
	}
}