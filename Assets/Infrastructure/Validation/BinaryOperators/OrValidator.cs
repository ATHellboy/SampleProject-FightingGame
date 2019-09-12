namespace Assets.Infrastructure.Validation.BinaryOperators {
	public class OrValidator : BaseBinaryValidator {
		public OrValidator(IValidator leftOperand, IValidator rightOperand) : base(leftOperand, rightOperand) { }

		public override ValidationResult IsValid(object obj, ValidationContext context) {
			return (ValidationResult.Accepted == LeftOperand.IsValid(obj, context)) ||
				   (ValidationResult.Accepted == RightOperand.IsValid(obj, context))

				? ValidationResult.Accepted
				: ValidationResult.Rejected;
		}
	}
}