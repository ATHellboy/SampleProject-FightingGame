using Assets.Infrastructure.Validation.UnaryOperators;

namespace Assets.Infrastructure.Validation.StringValidators {
	public class IsNullOrEmpty : BaseTypedValidator<string> {
		public override ValidationResult IsValid(string obj, ValidationContext context) {
			return new BooleanToValidationResult()
				.IsValid(string.IsNullOrEmpty(obj), context);
		}
	}
}