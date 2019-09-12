namespace Assets.Infrastructure.Scripts.CQRS.Validators
{
    public class NotValidator<TMessage> : BaseConditionValidator<TMessage> where TMessage : class, IMessage
    {
        private readonly IMessageConditionValidator _validator;

        public NotValidator(IMessageConditionValidator validator)
        {
            _validator = validator;
        }
        public override ValidationResult Validate(IMessageHandler handler, TMessage message)
        {
            return _validator.Validate(handler, message) == ValidationResult.Accepted
                ? ValidationResult.Rejected
                : ValidationResult.Accepted;
        }
    }
}