namespace Assets.Infrastructure.Validation.UnaryOperators {
	public class NotValidator : BaseUnaryValidator {
		public NotValidator(IValidator operand) : base(operand) { }

		public override ValidationResult IsValid(object obj, ValidationContext context) {
			return ValidationResult.Accepted == Operand.IsValid(obj, context)
				? ValidationResult.Rejected
				: ValidationResult.Accepted;
		}
	}
}