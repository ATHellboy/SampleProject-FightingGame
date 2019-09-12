namespace Assets.Infrastructure.Validation.UnaryOperators {
	public class BooleanToValidationResult : BaseTypedValidator<bool> {
		public override ValidationResult IsValid(bool obj, ValidationContext context) {
			return obj ? ValidationResult.Accepted : ValidationResult.Rejected;
		}
	}
}