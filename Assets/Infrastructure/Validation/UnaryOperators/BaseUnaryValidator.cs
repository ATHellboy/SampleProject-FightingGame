namespace Assets.Infrastructure.Validation.UnaryOperators {
	public abstract class BaseUnaryValidator : IValidator {
		protected readonly IValidator Operand;

		protected BaseUnaryValidator(IValidator operand) {
			Operand = operand;
		}

		public abstract ValidationResult IsValid(object obj, ValidationContext context);
	}

	public abstract class BaseTypedUnaryValidator<T> : BaseTypedValidator<T> {
		protected readonly IValidator Operand;

		protected BaseTypedUnaryValidator(IValidator operand) {
			Operand = operand;
		}
	}
}