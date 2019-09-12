using Assets.Infrastructure.Validation.UnaryOperators;

namespace Assets.Infrastructure.Validation.StringValidators {
	public class IsNotNullOrWhiteSpace : BaseTypedValidator<string> {
		public override ValidationResult IsValid(string obj, ValidationContext context) {
			return new BooleanToValidationResult()
				.IsValid(string.IsNullOrWhiteSpace(obj), context);
		}
	}
}