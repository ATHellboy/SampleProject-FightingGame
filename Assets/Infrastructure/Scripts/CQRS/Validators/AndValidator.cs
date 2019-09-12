namespace Assets.Infrastructure.Scripts.CQRS.Validators
{
    public class AndValidator<TMessage> : BaseConditionValidator<TMessage> where TMessage : class, IMessage
    {
        private readonly IMessageConditionValidator _leftOp;
        private readonly IMessageConditionValidator _rightOp;

        public AndValidator(IMessageConditionValidator leftOp, IMessageConditionValidator rightOp)
        {
            _leftOp = leftOp;
            _rightOp = rightOp;
        }
        public override ValidationResult Validate(IMessageHandler handler, TMessage message)
        {
            return _leftOp.Validate(handler, message) == ValidationResult.Accepted && _rightOp.Validate(handler, message) == ValidationResult.Accepted
                ? ValidationResult.Accepted
                : ValidationResult.Rejected;
        }
    }
}