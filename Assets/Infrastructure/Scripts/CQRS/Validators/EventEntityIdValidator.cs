namespace Assets.Infrastructure.Scripts.CQRS.Validators {
	public class EventEntityIdValidator<TMessage> : BaseConditionValidator<TMessage> where TMessage : class, IMessage, IEvent {
		private readonly string _predictedId;

		public EventEntityIdValidator(string predictedId) {
			_predictedId = predictedId;
		}

		public override ValidationResult Validate(IMessageHandler handler, TMessage message) {
			return message.EntityId == _predictedId ? ValidationResult.Accepted : ValidationResult.Rejected;
		}
	}
}