namespace Assets.Infrastructure.Scripts.CQRS.Validators {
	public class OrValidator<TMessage> : BaseConditionValidator<TMessage> where TMessage : class, IMessage {
		private readonly IMessageConditionValidator _leftOp;
		private readonly IMessageConditionValidator _rightOp;

		public OrValidator(IMessageConditionValidator leftOp, IMessageConditionValidator rightOp) {
			_leftOp = leftOp;
			_rightOp = rightOp;
		}
		public override ValidationResult Validate(IMessageHandler handler, TMessage message) {
			return ValidationResult.Accepted == _leftOp.Validate(handler, message) ||
				   ValidationResult.Accepted == _rightOp.Validate(handler, message)
				? ValidationResult.Accepted
				: ValidationResult.Rejected;
		}
	}
}