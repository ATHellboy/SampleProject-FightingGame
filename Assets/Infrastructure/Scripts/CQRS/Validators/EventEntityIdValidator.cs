namespace Assets.Infrastructure.Scripts.CQRS.Validators {
    public class EventEntityIdValidator<TMessage> : BaseConditionValidator<TMessage> where TMessage : class, IMessage, IEvent
    {
        public override ValidationResult Validate(IMessageHandler handler, TMessage message)
        {
            var playerIdHandler = handler as IEntityIdProperty;
            if (null == handler)
            {
                return ValidationResult.Rejected;
            }
            return message.EntityId == playerIdHandler.EntityId ? ValidationResult.Accepted : ValidationResult.Rejected;
        }
    }
}