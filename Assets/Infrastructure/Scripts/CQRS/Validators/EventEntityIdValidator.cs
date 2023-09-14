namespace Assets.Infrastructure.Scripts.CQRS.Validators {
    public class EventEntityIdValidator<TMessage> : BaseConditionValidator<TMessage> where TMessage : class, IMessage, IEvent
    {
        public override ValidationResult Validate(IMessageHandler handler, TMessage message)
        {
            var entityIdHandler = handler as IEntityIdProperty;
            if (null == handler)
            {
                return ValidationResult.Rejected;
            }
            return message.EntityId == entityIdHandler.EntityId ? ValidationResult.Accepted : ValidationResult.Rejected;
        }
    }
}